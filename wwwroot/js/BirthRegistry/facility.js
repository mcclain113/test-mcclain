class FacilityModule {
  constructor(mode, birthRegistryManager) {
    this.mode = mode || 'edit';
    this.birthRegistryManager = birthRegistryManager || null;
    this.saveEndpoint = '/BirthRegistry/SaveFacilityData';
    this.bindEvents();
  }

  wireFacilityReminder() {
    $(document).off('click.facilityReminder');
    $(document).on('click.facilityReminder', '#facility-reminder-close', function () {
      $('#facility-reminder').addClass('d-none');
      $('#facility-reminder-chip').removeClass('d-none');
    });
    $(document).on('click.facilityReminder', '#facility-reminder-chip', function () {
      $('#facility-reminder').removeClass('d-none');
      $('#facility-reminder-chip').addClass('d-none');
    });
  }

  sectionAlertFor($elInSection) {
    const $body = $elInSection.closest('.card-body');
    let $alert = $body.find('.section-alert').first();
    if (!$alert.length) {
      $alert = $('<div class="section-alert mb-2"></div>').prependTo($body);
    }
    return $alert;
  }

  showSectionAlert($inSectionEl, message, type = 'warning') {
    const $alert = this.sectionAlertFor($inSectionEl);
    $alert.html(
      '<div class="alert alert-' + type + ' alert-dismissible fade show" role="alert">' +
      '<i class="fas fa-' + BirthRegistryUtils.getAlertIcon(type) + '"></i> ' + message +
      '<button type="button" class="close" data-dismiss="alert" aria-label="Close">' +
      '<span aria-hidden="true">&times;</span>' +
      '</button>' +
      '</div>'
    );
  }

  clearSectionAlert($inSectionEl) {
    this.sectionAlertFor($inSectionEl).empty();
  }

  setInvalid($els, invalid) {
    $els.each(function (_, el) {
      $(el).toggleClass('is-invalid', invalid);
    });
  }

  isTenDigitNpi(v) {
    return /^\d{10}$/.test(String(v || '').trim());
  }

  textOfSelected($sel) {
    return ($sel.find('option:selected').text() || '').toLowerCase().trim();
  }

  dateNotInFuture(val) {
    if (!val) return false;
    const d = new Date(val);
    if (isNaN(d)) return false;
    const t = new Date();
    t.setHours(0, 0, 0, 0);
    d.setHours(0, 0, 0, 0);
    return d <= t;
  }

  setHeaderCheck(sectionIndex, valid) {
    const $check = $('#facility-section-' + sectionIndex + ' .accordion-checkmark');
    $check.toggleClass('d-none', !valid);
  }

  setSectionState($cardBody, isValid, sectionIndex) {
    this.setHeaderCheck(sectionIndex, isValid);
  }

  readAttendantFromSection3() {
    const ex = $('#attendantSummaryName').data('attendant-info');
    if (ex && ex.fullName) {
      return {
        name: ex.fullName,
        npi: (ex.npi || '').toString(),
        titles: ex.titles || ''
      };
    }

    const name = ($('#attendantSummaryName').text() || '').trim();
    const npi = ($('#attendantSummaryNpi').text() || '').trim();
    const titles = ($('#attendantSummaryTitles').text() || '').trim();
    return { name: name, npi: npi, titles: titles };
  }

  renderCertifierSummaryFromSection3() {
    const a = this.readAttendantFromSection3();
    $('#certifierSummaryName').text(a.name || '—');
    $('#certifierSummaryNpi').text(a.npi || '—');
    $('#certifierSummaryTitles').text(a.titles || '—');
    $('#certifierSummary').removeClass('d-none');
  }

  refreshVisibility() {
    const place = $('select[name="PlaceOfBirth"]').val();
    if (place === 'WCTMC') {
      $('#wctmcDetails').removeClass('d-none');
      $('#nonFacilityAddressGroup').addClass('d-none');

      $('#NonWctmcFacilityId').val('');
      $('#nonWctmcFacilitySearch').val('');
      $('#nonWctmcFacilityResults').empty();
      $('#nonWctmcFacilitySummaryWrap').addClass('d-none');
    } else if (place === 'NonFacility') {
      $('#wctmcDetails').addClass('d-none');
      $('#nonFacilityAddressGroup').removeClass('d-none');
    } else {
      $('#wctmcDetails, #nonFacilityAddressGroup').addClass('d-none');
    }

    const placeTypeText = this.textOfSelected($('#BirthPlaceTypeId'));
    const isHome = /home/i.test(placeTypeText);
    const isOther = /other/i.test(placeTypeText);

    $('#homePlannedGroup').toggleClass('d-none', !isHome);
    if (!isHome) {
      $('input[name="IsPlannedHomeBirth"]').prop('checked', false).removeClass('is-invalid');
    }

    $('#birthPlaceTypeOtherWrap').toggleClass('d-none', !isOther);
    if (!isOther) {
      $('[name="BirthPlaceTypeOtherText"]').val('').removeClass('is-invalid');
    }

    const isTransferYes = $('input[name="IsMotherTransferred"]:checked').val() === 'true';
    $('#transferFacilityGroup').toggleClass('d-none', !isTransferYes);
    if (!isTransferYes) {
      $('#FacilityTransferredFromId').val('');
      $('#transferFacilitySearch').val('');
      $('#transferFacilityResults').empty();
      $('#transferFacilitySummaryWrap').addClass('d-none');
    }

    const certPick = $('input[name="IsCertifierAttendant"]:checked').val();
    const isSame = certPick === 'true';
    const isDifferent = certPick === 'false';

    $('#certifierSummary').toggleClass('d-none', !isSame);
    $('#certifierSearchGroup').toggleClass('d-none', !isDifferent);

    if (isSame) {
      this.renderCertifierSummaryFromSection3();
      const attendingId = ($('#DeliveringAttendantId').val() || '').trim();
      if (attendingId) {
        $('#CertifierOfBirthId').val(attendingId);
      } else {
        $('#CertifierOfBirthId').val('');
      }
      $('#certifierSearch').val('').removeClass('is-invalid');
      $('#certifierResults').empty();
      $('#manualCertifierSummary').addClass('d-none');
    }

    if (!isDifferent) {
      $('#certifierSearch').removeClass('is-invalid');
      $('#certifierResults').empty();
      $('#manualCertifierSummary').addClass('d-none');
    }
  }

  validateSection1(suppress = false) {
    const sectionIndex = 1;
    const $body = $('#facility-collapse1 .card-body');
    const $place = $('select[name="PlaceOfBirth"]');
    const $placeType = $('#BirthPlaceTypeId');

    const place = $place.val();
    const placeTypeText = this.textOfSelected($placeType);

    let ok = true;

    this.setInvalid($place, !place);
    ok = ok && !!place;

    this.setInvalid($placeType, !$placeType.val());
    ok = ok && !!$placeType.val();

    if (place === 'NonFacility') {
      const facilityId = ($('#NonWctmcFacilityId').val() || '').trim();
      const hasFacility = !!facilityId;
      this.setInvalid($('#nonWctmcFacilitySearch'), !hasFacility);
      ok = ok && hasFacility;
    }

    if (placeTypeText.indexOf('home') !== -1) {
      const plannedPicked = $('input[name="IsPlannedHomeBirth"]:checked').length > 0;
      const $homeGroup = $('#homePlannedGroup');
      $homeGroup.find('input[type="radio"]').each(function () {
        $(this).toggleClass('is-invalid', !plannedPicked);
      });
      ok = ok && plannedPicked;
    }

    if (placeTypeText.indexOf('other') !== -1) {
      const $other = $('[name="BirthPlaceTypeOtherText"]');
      const hasOther = ($other.val() || '').trim().length > 0;
      this.setInvalid($other, !hasOther);
      ok = ok && hasOther;
    }

    if (!ok && !suppress) {
      this.showSectionAlert($body, 'Please complete Place of Birth before saving.', 'warning');
    }
    if (ok) {
      this.clearSectionAlert($body);
    }

    this.setSectionState($body, ok, sectionIndex);
    return ok;
  }

  validateSection2(suppress = false) {
    const sectionIndex = 2;
    const $body = $('#facility-collapse2 .card-body');
    const $radios = $('input[name="IsMotherTransferred"]');

    let ok = $radios.filter(':checked').length > 0;
    this.setInvalid($radios, !ok);

    if (ok && $radios.filter(':checked').val() === 'true') {
      const facilityId = ($('#FacilityTransferredFromId').val() || '').trim();
      const hasFrom = !!facilityId;
      this.setInvalid($('#transferFacilitySearch'), !hasFrom);
      ok = ok && hasFrom;
    }

    if (!ok && !suppress) {
      this.showSectionAlert($body, 'Please answer transfer question (and select a facility if Yes).', 'warning');
    }
    if (ok) {
      this.clearSectionAlert($body);
    }

    this.setSectionState($body, ok, sectionIndex);
    return ok;
  }

  validateSection3(suppress = false) {
    const sectionIndex = 3;
    const $body = $('#facility-collapse3 .card-body');
    const $search = $('#physicianSearch');

    const attendantId = ($('#DeliveringAttendantId').val() || '').trim();
    const ok = !!attendantId;

    this.setInvalid($search, !ok);

    if (!ok && !suppress) {
      this.showSectionAlert($body, 'Please search for and select a delivering attendant.', 'warning');
    }
    if (ok) {
      this.clearSectionAlert($body);
    }

    this.setSectionState($body, ok, sectionIndex);
    return ok;
  }

  validateSection4(suppress = false) {
    const sectionIndex = 4;
    const $body = $('#facility-collapse4 .card-body');
    const $radios = $('input[name="IsCertifierAttendant"]');
    let ok = true;

    const pickedCount = $radios.filter(':checked').length;
    const hasPick = pickedCount > 0;
    this.setInvalid($radios, !hasPick);

    if (!hasPick) {
      ok = false;
      if (!suppress) {
        this.showSectionAlert($body, 'Please indicate if the certifier is the same as the delivering attendant.', 'warning');
      }
    } else {
      const value = $radios.filter(':checked').val();
      const deliveringId = ($('#DeliveringAttendantId').val() || '').trim();
      const $certSearch = $('#certifierSearch');
      const certifierId = ($('#CertifierOfBirthId').val() || '').trim();

      if (value === 'true') {
        const hasAttendant = !!deliveringId;
        if (!hasAttendant) {
          ok = false;
          if (!suppress) {
            this.showSectionAlert($body, 'Please select a delivering attendant before marking the certifier as the same.', 'warning');
          }
        } else {
          $('#CertifierOfBirthId').val(deliveringId);
        }
        this.setInvalid($certSearch, false);
      } else if (value === 'false') {
        const hasCertifier = !!certifierId;
        this.setInvalid($certSearch, !hasCertifier);
        if (!hasCertifier) {
          ok = false;
          if (!suppress) {
            this.showSectionAlert($body, 'Please search for and select a certifier.', 'warning');
          }
        }
      }
    }

    if (ok) {
      this.clearSectionAlert($body);
    }

    this.setSectionState($body, ok, sectionIndex);
    return ok;
  }

  validateSection5(suppress = false) {
    const sectionIndex = 5;
    const $body = $('#facility-collapse5 .card-body');
    const $sig = $('input[name="CertifierSignature"]');
    const $date = $('input[name="DateCertified"]');

    const hasSig = ($sig.val() || '').trim().length > 0;
    const okDate = this.dateNotInFuture($date.val());

    this.setInvalid($sig, !hasSig);
    this.setInvalid($date, !okDate);

    const ok = hasSig && okDate;

    if (!ok && !suppress) {
      if (!okDate) {
        this.showSectionAlert($body, 'Date Certified is required and cannot be in the future.', 'warning');
      } else {
        this.showSectionAlert($body, 'Please provide signature and a valid Date Certified.', 'warning');
      }
    }
    if (ok) {
      this.clearSectionAlert($body);
    }

    this.setSectionState($body, ok, sectionIndex);
    return ok;
  }

  validateAll() {
    const v1 = this.validateSection1(false);
    const v2 = this.validateSection2(false);
    const v3 = this.validateSection3(false);
    const v4 = this.validateSection4(false);
    const v5 = this.validateSection5(false);

    return v1 && v2 && v3 && v4 && v5;
  }

  bindEvents() {
    $(document).off('submit.facility').on('submit.facility', '#facilityForm', function (e) {
      e.preventDefault();
      return false;
    });

    $(document).off('click.facilitySave').on('click.facilitySave', '#saveFacilityBtn', async (e) => {
      e.preventDefault();
      await this.saveFacilityData();
    });

    $(document).off('change.facilityPlace').on('change.facilityPlace', 'select[name="PlaceOfBirth"]', () => {
      this.refreshVisibility();
      this.validateSection1(true);
    });

    $(document).off('change.facilityPlaceType').on('change.facilityPlaceType', '#BirthPlaceTypeId', () => {
      this.refreshVisibility();
      this.validateSection1(true);
    });

    $(document).off('change.facilityHomeBirth').on('change.facilityHomeBirth', 'input[name="IsPlannedHomeBirth"]', () => {
      this.refreshVisibility();
      this.validateSection1(true);
    });

    $(document).off('change.facilityTransfer').on('change.facilityTransfer', 'input[name="IsMotherTransferred"]', () => {
      this.refreshVisibility();
      this.validateSection2(true);
    });

    $(document).off('change.facilityCertifierAttendant').on('change.facilityCertifierAttendant', 'input[name="IsCertifierAttendant"]', () => {
      this.refreshVisibility();
      this.validateSection4(true);
    });

    $(document).off('change.facilitySectionInputs input.facilitySectionInputs')
      .on('change.facilitySectionInputs input.facilitySectionInputs', '#facilityAccordion select, #facilityAccordion input', (e) => {
        const $parent = $(e.target).closest('.collapse');

        if ($parent.is('#facility-collapse1')) {
          this.validateSection1(true);
        } else if ($parent.is('#facility-collapse2')) {
          this.validateSection2(true);
        } else if ($parent.is('#facility-collapse3')) {
          this.validateSection3(true);
        } else if ($parent.is('#facility-collapse4')) {
          this.validateSection4(true);
        } else if ($parent.is('#facility-collapse5')) {
          this.validateSection5(true);
        }
      });

    let nonWctmcFacilitySearchTimer = null;

    $(document).off('input.facilityNonWctmcSearch').on('input.facilityNonWctmcSearch', '#nonWctmcFacilitySearch', (e) => {
      const term = $(e.target).val().trim();
      clearTimeout(nonWctmcFacilitySearchTimer);
      $('#nonWctmcFacilityResults').empty();

      if (term.length < 2) return;

      nonWctmcFacilitySearchTimer = setTimeout(() => {
        $.getJSON('/BirthRegistry/SearchFacilities', { term: term }, (resp) => {
          const $ul = $('#nonWctmcFacilityResults').empty();
          if (!resp || !resp.success || !resp.results || !resp.results.length) {
            $ul.append('<li class="list-group-item text-muted">No facilities found</li>');
            return;
          }

          $.each(resp.results, function (_, f) {
            const city = f.city || '';
            const state = f.state || '';
            const postal = f.postal || '';
            const addressLine = [f.street, city, state, postal].filter(Boolean).join(', ');

            const $li = $(
              '<li class="list-group-item facility-result-nonwctmc" ' +
              'data-id="' + f.id + '" ' +
              'data-name="' + (f.name || '') + '" ' +
              'data-city="' + city + '" ' +
              'data-postal="' + postal + '" ' +
              'data-identifier="' + f.id + '">' +
              '<strong>' + (f.name || '') + '</strong><br/>' +
              '<small>' + addressLine + '</small>' +
              '</li>'
            );
            $ul.append($li);
          });
        });
      }, 300);
    });

    $(document).off('click.facilityNonWctmcResult').on('click.facilityNonWctmcResult', '.facility-result-nonwctmc', (e) => {
      const $item = $(e.currentTarget);
      const id = $item.data('id');
      const name = $item.data('name') || '—';
      const city = $item.data('city') || '—';
      const identifier = $item.data('identifier') || id;

      $('#NonWctmcFacilityId').val(id);
      $('#nonWctmcFacilitySearch').val(name).removeClass('is-invalid');

      $('#nonWctmcFacilitySummaryName').text(name);
      $('#nonWctmcFacilitySummaryCity').text(city);
      $('#nonWctmcFacilitySummaryPostal').text($item.data('postal') || '—');
      $('#nonWctmcFacilitySummaryId').text(identifier);

      $('#nonWctmcFacilitySummaryWrap').removeClass('d-none');
      $('#nonWctmcFacilityResults').empty();

      this.validateSection1(true);
    });

    $(document).off('click.facilityChangeNonWctmc').on('click.facilityChangeNonWctmc', '#changeNonWctmcFacilityBtn', () => {
      $('#NonWctmcFacilityId').val('');
      $('#nonWctmcFacilitySearch').val('').removeClass('is-invalid').focus();
      $('#nonWctmcFacilitySummaryWrap').addClass('d-none');
      $('#nonWctmcFacilityResults').empty();
      this.validateSection1(true);
    });

    let transferFacilitySearchTimer = null;

    $(document).off('input.facilityTransferSearch').on('input.facilityTransferSearch', '#transferFacilitySearch', (e) => {
      const term = $(e.target).val().trim();
      clearTimeout(transferFacilitySearchTimer);
      $('#transferFacilityResults').empty();

      if (term.length < 2) return;

      transferFacilitySearchTimer = setTimeout(() => {
        $.getJSON('/BirthRegistry/SearchFacilities', { term: term }, (resp) => {
          const $ul = $('#transferFacilityResults').empty();
          if (!resp || !resp.success || !resp.results || !resp.results.length) {
            $ul.append('<li class="list-group-item text-muted">No facilities found</li>');
            return;
          }

          $.each(resp.results, function (_, f) {
            const city = f.city || '';
            const state = f.state || '';
            const postal = f.postal || '';
            const addressLine = [f.street, city, state, postal].filter(Boolean).join(', ');

            const $li = $(
              '<li class="list-group-item facility-result-transfer" ' +
              'data-id="' + f.id + '" ' +
              'data-name="' + (f.name || '') + '" ' +
              'data-city="' + city + '" ' +
              'data-postal="' + postal + '" ' +
              'data-identifier="' + f.id + '">' +
              '<strong>' + (f.name || '') + '</strong><br/>' +
              '<small>' + addressLine + '</small>' +
              '</li>'
            );
            $ul.append($li);
          });
        });
      }, 300);
    });

    $(document).off('click.facilityTransferResult').on('click.facilityTransferResult', '.facility-result-transfer', (e) => {
      const $item = $(e.currentTarget);
      const id = $item.data('id');
      const name = $item.data('name') || '—';
      const city = $item.data('city') || '—';
      const identifier = $item.data('identifier') || id;

      $('#FacilityTransferredFromId').val(id);
      $('#transferFacilitySearch').val(name).removeClass('is-invalid');

      $('#transferFacilitySummaryName').text(name);
      $('#transferFacilitySummaryCity').text(city);
      $('#transferFacilitySummaryPostal').text($item.data('postal') || '—');
      $('#transferFacilitySummaryId').text(identifier);

      $('#transferFacilitySummaryWrap').removeClass('d-none');
      $('#transferFacilityResults').empty();

      this.validateSection2(true);
    });

    $(document).off('click.facilityChangeTransfer').on('click.facilityChangeTransfer', '#changeTransferFacilityBtn', () => {
      $('#FacilityTransferredFromId').val('');
      $('#transferFacilitySearch').val('').removeClass('is-invalid').focus();
      $('#transferFacilitySummaryWrap').addClass('d-none');
      $('#transferFacilityResults').empty();
      this.validateSection2(true);
    });

    let physSearchTimer = null;

    $(document).off('input.facilityPhysicianSearch').on('input.facilityPhysicianSearch', '#physicianSearch', (e) => {
      const $input = $(e.target);
      if ($input.prop('disabled')) return;

      const term = $input.val().trim();
      clearTimeout(physSearchTimer);
      $('#physicianResults').empty();

      if (term.length < 2) return;

      physSearchTimer = setTimeout(() => {
        $.get('/BirthRegistry/SearchPhysicians', { term: term }, (data) => {
          const $ul = $('#physicianResults').empty();
          (data || []).forEach(function (p) {
            $ul.append(
              '<li class="list-group-item physician-result" ' +
              'data-id="' + p.id + '" ' +
              'data-fullname="' + (p.fullName || '') + '" ' +
              'data-npi="' + (p.npi || '') + '" ' +
              'data-titles="' + (p.titles || '') + '">' +
              (p.fullName || '') + ' — NPI ' + (p.npi || '') + (p.titles ? ' — ' + p.titles : '') +
              '</li>'
            );
          });
        });
      }, 200);
    });

    $(document).off('click.facilityPhysicianResult').on('click.facilityPhysicianResult', '.physician-result', (e) => {
      const $item = $(e.currentTarget);
      const id = $item.data('id');
      const fullName = $item.data('fullname') || '—';
      const npi = $item.data('npi') || '—';
      const titles = $item.data('titles') || '—';

      $('#DeliveringAttendantId').val(id);
      $('#physicianSearch')
        .val(fullName + ' — NPI ' + npi)
        .prop('disabled', true)
        .addClass('bg-light');

      $('#existingAttendantSummary').removeClass('d-none');
      $('#changeAttendantBtn').removeClass('d-none');

      $('#attendantSummaryName')
        .text(fullName)
        .data('attendant-info', { fullName: fullName, npi: npi, titles: titles });
      $('#attendantSummaryNpi').text(npi);
      $('#attendantSummaryTitles').text(titles);

      $('#physicianResults').empty();

      this.validateSection3(false);

      if ($('input[name="IsCertifierAttendant"]:checked').val() === 'true') {
        this.renderCertifierSummaryFromSection3();
        this.validateSection4(true);
      }
    });

    $(document).off('click.facilityChangeAttendant').on('click.facilityChangeAttendant', '#changeAttendantBtn', () => {
      $('#DeliveringAttendantId').val('');
      $('#physicianSearch')
        .val('')
        .prop('disabled', false)
        .removeClass('bg-light')
        .focus();
      $('#existingAttendantSummary').addClass('d-none');
      $('#physicianResults').empty();
      $('#changeAttendantBtn').addClass('d-none');

      this.validateSection3(false);

      if ($('input[name="IsCertifierAttendant"]:checked').val() === 'true') {
        this.renderCertifierSummaryFromSection3();
        this.validateSection4(true);
      }
    });

    let certSearchTimer = null;

    $(document).off('input.facilityCertifierSearch').on('input.facilityCertifierSearch', '#certifierSearch', (e) => {
      const $input = $(e.target);
      if ($input.prop('disabled')) return;

      const term = $input.val().trim();
      clearTimeout(certSearchTimer);
      $('#certifierResults').empty();

      if (term.length < 2) return;

      certSearchTimer = setTimeout(() => {
        $.get('/BirthRegistry/SearchPhysicians', { term: term }, (data) => {
          const $ul = $('#certifierResults').empty();
          (data || []).forEach(function (p) {
            $ul.append(
              '<li class="list-group-item certifier-result" ' +
              'data-id="' + p.id + '" ' +
              'data-fullname="' + (p.fullName || '') + '" ' +
              'data-npi="' + (p.npi || '') + '" ' +
              'data-titles="' + (p.titles || '') + '">' +
              (p.fullName || '') + ' — NPI ' + (p.npi || '') + (p.titles ? ' — ' + p.titles : '') +
              '</li>'
            );
          });
        });
      }, 200);
    });

    $(document).off('click.facilityCertifierResult').on('click.facilityCertifierResult', '.certifier-result', (e) => {
      const $item = $(e.currentTarget);
      const id = $item.data('id');
      const fullName = $item.data('fullname') || '—';
      const npi = $item.data('npi') || '—';
      const titles = $item.data('titles') || '—';

      $('#CertifierOfBirthId').val(id);
      $('#certifierSearch')
        .val(fullName + ' — NPI ' + npi)
        .removeClass('is-invalid');

      $('#manualCertifierSummaryName').text(fullName);
      $('#manualCertifierSummaryNpi').text(npi);
      $('#manualCertifierSummaryTitles').text(titles);
      $('#manualCertifierSummary').removeClass('d-none');

      $('#certifierResults').empty();

      this.validateSection4(false);
    });

    $(document).off('click.facilityDisabledAccordion').on('click.facilityDisabledAccordion', '#facilityAccordion .card-header .btn[disabled]', function (e) {
      e.preventDefault();
      e.stopPropagation();
      return false;
    });

    $('#facilityAccordion button').attr('type', 'button');

    $(document).off('hide.facilityCollapse3').on('hide.bs.collapse.facility', '#facility-collapse3', function (e) {
      const typingInSearch = $('#physicianSearch').is(':focus');
      const hasResults = $('#physicianResults').children().length > 0;

      if (typingInSearch || hasResults) {
        e.preventDefault();
      }
    });

    $(document).off('hide.facilityCollapse4').on('hide.bs.collapse.facility', '#facility-collapse4', function (e) {
      const typingInSearch = $('#certifierSearch').is(':focus');
      const hasResults = $('#certifierResults').children().length > 0;
      const hasSelectedCertifier = ($('#CertifierOfBirthId').val() || '').trim().length > 0;

      if (!hasSelectedCertifier && (typingInSearch || hasResults)) {
        e.preventDefault();
      }
    });
  }

    async saveFacilityData() {
    const isValid = this.validateAll();

    if (!isValid) {
      BirthRegistryUtils.showAlert(
        '#facility-card .card-body',
        'Please correct the highlighted errors in the Facility tab before saving.',
        'warning'
      );
      return;
    }

    let birthIdFromManager = null;

    const mgr = window.birthRegistryManager || this.birthRegistryManager || null;

    if (mgr) {
      if (typeof mgr.getBirthId === 'function') {
        birthIdFromManager = mgr.getBirthId();
      } else if (typeof mgr.birthId !== 'undefined') {
        birthIdFromManager = mgr.birthId;
      }
    }

    const birthIdFromHidden = parseInt(
      $('input[name="BirthId"]').val() || '0',
      10
    );

    const birthId =
      birthIdFromManager && birthIdFromManager > 0
        ? birthIdFromManager
        : (!isNaN(birthIdFromHidden) && birthIdFromHidden > 0
          ? birthIdFromHidden
          : 0);

    if (!birthId || birthId <= 0) {
      BirthRegistryUtils.showAlert(
        '#facility-card .card-body',
        'Birth record could not be determined. Please save Mother Information first, then try again.',
        'danger',
        false,
        true
      );
      console.error('Facility save: invalid birthId', {
        fromManager: birthIdFromManager,
        fromHidden: birthIdFromHidden
      });
      return;
    }

    const placeOfBirth = $('select[name="PlaceOfBirth"]').val() || null;
    const birthPlaceTypeVal = $('#BirthPlaceTypeId').val();
    const nonWctmcIdVal = $('#NonWctmcFacilityId').val();
    const transferFromIdVal = $('#FacilityTransferredFromId').val();
    const deliveringIdVal = $('#DeliveringAttendantId').val();
    const certifierIdVal = $('#CertifierOfBirthId').val();
    const isMotherTransferredVal = $('input[name="IsMotherTransferred"]:checked').val();
    const isCertifierAttendantVal = $('input[name="IsCertifierAttendant"]:checked').val();
    const isPlannedHomeBirthVal = $('input[name="IsPlannedHomeBirth"]:checked').val();

    const viewModel = {
      BirthId: birthId,
      AddressId: parseInt($('input[name="AddressId"]').val() || '0', 10) || null,
      FacilityId: parseInt($('input[name="FacilityId"]').val() || '0', 10) || null,

      PlaceOfBirth: placeOfBirth,
      BirthPlaceTypeId: birthPlaceTypeVal ? parseInt(birthPlaceTypeVal, 10) : null,
      BirthPlaceTypeOtherText: ($('[name="BirthPlaceTypeOtherText"]').val() || '').trim() || null,
      NonWctmcFacilityId: nonWctmcIdVal ? parseInt(nonWctmcIdVal, 10) : null,
      IsPlannedHomeBirth:
        isPlannedHomeBirthVal === 'true' ? true :
        isPlannedHomeBirthVal === 'false' ? false :
        null,

      IsMotherTransferred:
        isMotherTransferredVal === 'true' ? true :
        isMotherTransferredVal === 'false' ? false :
        null,
      FacilityTransferredFromId: transferFromIdVal ? parseInt(transferFromIdVal, 10) : null,

      DeliveringAttendantId: deliveringIdVal ? parseInt(deliveringIdVal, 10) : null,

      IsCertifierAttendant:
        isCertifierAttendantVal === 'true' ? true :
        isCertifierAttendantVal === 'false' ? false :
        null,
      CertifierOfBirthId: certifierIdVal ? parseInt(certifierIdVal, 10) : null,

      CertifierSignature: ($('input[name="CertifierSignature"]').val() || '').trim() || null,
      DateCertified: $('input[name="DateCertified"]').val() || null
    };

    const token = $('input[name="__RequestVerificationToken"]').val();

    const saveFunction = () =>
      new Promise(function (resolve, reject) {
        $.ajax({
          url: '/BirthRegistry/SaveFacilityData',
          type: 'POST',
          contentType: 'application/json; charset=utf-8',
          data: JSON.stringify(viewModel),
          headers: {
            RequestVerificationToken: token
          },
          success: resolve,
          error: reject
        });
      });

    const onSuccess = (result) => {
      if (this.birthRegistryManager && result && result.birthId) {
        this.birthRegistryManager.setBirthId(result.birthId);
      }

      if (result && result.birthId) {
        $('input[name="BirthId"]').val(result.birthId);
      }

      BirthRegistryUtils.showAlert(
        '#facility-card',
        'Facility data saved successfully.',
        'success',
        true
      );
    };

    await BirthRegistryUtils.handleSaveWithFeedback(
      '#facility-card .card-body',
      saveFunction,
      onSuccess
    );
  }

  init() {
    this.wireFacilityReminder();
    this.refreshVisibility();
    this.validateSection1(true);
    this.validateSection2(true);
    this.validateSection3(true);
    this.validateSection4(true);
    this.validateSection5(true);
  }
}

function initFacility() {
  $('#facility-collapse1').addClass('show');
  $('#facility-collapse2,#facility-collapse3,#facility-collapse4,#facility-collapse5').removeClass('show');

  $('#facility-section-1,#facility-section-2,#facility-section-3,#facility-section-4,#facility-section-5')
    .removeClass('d-none');

  if (window.facilityModule) {
    window.facilityModule.init();
  }
}

$(document).ready(function () {
  const mgr = window.birthRegistryManager || null;
  window.facilityModule = new FacilityModule('edit', mgr);

  if (mgr && typeof mgr.registerModule === 'function') {
    mgr.registerModule('facility', window.facilityModule);
  }


  initFacility();
});