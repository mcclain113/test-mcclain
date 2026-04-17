class NewbornModule {
    constructor(mode = "edit", birthRegistryManager = null) {
        this.mode = mode;
        this.searchEndpoint = "/BirthRegistry/SearchNewborn";
        this.detailsEndpoint = "/BirthRegistry/NewbornDetails";
        this.createPatientUrl = "/Patient/CreatePatient";
        this.saveEndpoint = "/BirthRegistry/SaveNewbornInformation";
        this.birthRegistryManager = birthRegistryManager;
    }

    init() {
        this.checkInitialState();
        this.setupEventHandlers();
        this.setupValidation();
        BirthRegistryUtils.initializeTooltips("#newborn-content");
    }

    checkInitialState() {
        const hasNewbornMrn = $('#newbornMrn').val();
        const isExistingPatient = $('#newbornIsExistingPatient').val() === 'true';
        
        if (hasNewbornMrn && isExistingPatient) {
            $('#newbornSelectionWrapper').addClass('d-none');
            $('#newbornInformationAccordion').removeClass('d-none');
            this.markAllSectionsValid();
        } else {
            $('#newbornSelectionWrapper').removeClass('d-none');
            $('#newbornInformationAccordion').addClass('d-none');
        }
    }

    setupEventHandlers() {
        this.setupNewbornStatusHandlers();
        this.setupMutuallyExclusiveHandlers();
        this.setupSpecialTriggerHandlers();
        this.setupPatientSearchHandlers();
        this.setupValidationTriggers();
        this.setupSaveHandler();
    }

    setupNewbornStatusHandlers() {
        $(document)
            .off("change", 'input[name="newbornPatientStatus"]')
            .on("change", 'input[name="newbornPatientStatus"]', (e) => {
                this.handleNewbornStatusChange(e);
            });
    }

    handleNewbornStatusChange(e) {
        const selectedValue = $(e.target).val();

        if (selectedValue === "notInDatabase") {
            $("#newbornSearchSection").addClass("d-none");
            $("#newbornSearchResults").addClass("d-none");
            $("#newbornSearchResultsBody").empty();
            $("#newbornSearchTerm").val("");

            this.clearNewbornFormFields();
            $("#newbornInformationAccordion").removeClass("d-none");
            $("#newbornIsExistingPatient").val("false");
            $("#newbornMrn").val("");

            setTimeout(() => {
                $("#nbFirstName").focus();
            }, 100);

            BirthRegistryUtils.showAlert(
                "#newborn-content",
                "Newborn information will be entered manually (not saved to patient database)",
                "info"
            );
        } else {
            $("#newbornSearchSection").removeClass("d-none");
            $("#newbornInformationAccordion").addClass("d-none");
            this.clearNewbornFormFields();

            BirthRegistryUtils.showAlert(
                "#newborn-content",
                "Search for existing newborn patients or create new patient record",
                "info"
            );
        }
    }

    setupPatientSearchHandlers() {
        $(document)
            .off("click", "#searchNewbornBtn")
            .on("click", "#searchNewbornBtn", (e) => {
                this.handlePatientSearch(e);
            });

        $(document)
            .off("click", "#createNewNewbornBtn")
            .on("click", "#createNewNewbornBtn", (e) => {
                this.handleCreateNewPatient(e);
            });

        $(document)
            .off("click", ".select-newborn")
            .on("click", ".select-newborn", (e) => {
                const mrn = $(e.target).data("mrn");
                this.selectPatient(mrn);
            });

        $(document)
            .off("keypress", "#newbornSearchTerm")
            .on("keypress", "#newbornSearchTerm", (e) => {
                if (e.which === 13) {
                    e.preventDefault();
                    this.handlePatientSearch(e);
                }
            });
    }

    handleCreateNewPatient(e) {
        window.open(this.createPatientUrl, "_blank");
        BirthRegistryUtils.showAlert(
            "#newborn-content",
            "New patient creation page opened. After creating the patient, return here and search for them.",
            "info"
        );
    }

    handlePatientSearch(e) {
        const searchTerm = $("#newbornSearchTerm").val();

        if (!searchTerm || searchTerm.trim() === "") {
            BirthRegistryUtils.showAlert(
                "#newborn-content",
                "Please enter a search term",
                "warning"
            );
            return;
        }

        this.searchPatients(searchTerm);
    }

    searchPatients(searchTerm) {
        const $searchBtn = $("#searchNewbornBtn");
        const originalText = $searchBtn.html();

        $searchBtn
            .html('<i class="fas fa-spinner fa-spin"></i> Searching...')
            .prop("disabled", true);

        const birthId = this.birthRegistryManager?.birthId || null;

        $.ajax({
            url: this.searchEndpoint,
            type: "GET",
            data: { 
                query: searchTerm,
                birthId: birthId  // Add birthId to filter out newborns from other births
            },
            success: (result) => {
                this.displaySearchResults(result);
            },
            error: (xhr) => {
                BirthRegistryUtils.showAlert(
                    "#newborn-content",
                    xhr.responseText,
                    "warning"
                );
            },
            complete: () => {
                $searchBtn.html(originalText).prop("disabled", false);
            },
        });
    }

    displaySearchResults(patients) {
        const tbody = $("#newbornSearchResultsBody");
        tbody.empty();

        if (!patients || patients.length === 0) {
            tbody.append(
                '<tr><td colspan="5" class="text-center text-muted">No newborn patients found</td></tr>'
            );
        } else {
            patients.forEach((patient) => {
                const row = `
                    <tr>
                        <td><strong>${patient.mrn || "N/A"}</strong></td>
                        <td>
                            <strong>${patient.firstName || ''} ${patient.middleName || ''} ${patient.lastName || ''}</strong>
                        </td>
                        <td>${patient.dob ? BirthRegistryUtils.formatDate(patient.dob) : "N/A"}</td>
                        <td>${patient.ssn ? BirthRegistryUtils.formatSSN(patient.ssn) : "N/A"}</td>
                        <td>
                            <button class="btn btn-sm btn-primary select-newborn" data-mrn="${patient.mrn}">
                                <i class="fas fa-check"></i> Select
                            </button>
                        </td>
                    </tr>
                `;
                tbody.append(row);
            });
        }

        $("#newbornSearchResults").removeClass("d-none");
    }

    async selectPatient(mrn) {
        const $selectBtn = $(`.select-newborn[data-mrn="${mrn}"]`);
        const originalText = $selectBtn.html();

        $selectBtn
            .html('<i class="fas fa-spinner fa-spin"></i> Loading...')
            .prop("disabled", true);

        try {
            const birthId = this.birthRegistryManager?.birthId || null;
            let url = `/BirthRegistry/NewbornDetails?mrn=${encodeURIComponent(mrn)}&mode=${this.mode}`;
            
            if (birthId) {
                url += `&birthId=${birthId}`;
            }
            
            const response = await fetch(url);
            
            if (!response.ok) {
                throw new Error(`HTTP ${response.status}`);
            }
            
            const html = await response.text();
            
            $('#newbornInformationAccordion').html(html);
            $('#newbornSelectionWrapper').addClass('d-none');
            $('#newbornInformationAccordion').removeClass('d-none');
            
            $('#newbornMrn').val(mrn);
            $('#newbornIsExistingPatient').val('true');
            
            this.setupValidationTriggers();
            this.setupMutuallyExclusiveHandlers();
            this.setupSpecialTriggerHandlers();
            this.setupSaveHandler();
            
            BirthRegistryUtils.initializeTooltips("#newborn-content");
            this.markAllSectionsValid();
            
            BirthRegistryUtils.showAlert('#newborn-content', 
                'Newborn information loaded successfully', 'success', true);
                
        } catch (error) {
            BirthRegistryUtils.showAlert('#newborn-content', 
                `Error loading newborn information: ${error.message}`, 'danger', false);
            $selectBtn.html(originalText).prop('disabled', false);
        }
    }

    markAllSectionsValid() {
        const isExistingPatient = $('#newbornIsExistingPatient').val() === 'true';
        
        if (isExistingPatient) {
            $('#newbornAccordion .card').each(function() {
                const $card = $(this);
                const $nextBtn = $card.find('.next-accordion-btn, .next-tab-btn');
                
                $nextBtn.prop('disabled', false);
                BirthRegistryUtils.updateCheckmark($card, true);
            });
        }
    }

    clearNewbornFormFields() {
        $("#FirstName, #MiddleName, #LastName").val("");
        $("#DateAndTimeOfBirth, #BirthWeightInLbs, #GestationalAgeEstimateInWeeks").val("");
        $("#ApgarScoreAt1Minute, #ApgarScoreAt5Minutes, #ApgarScoreAt10Minutes").val("");
        $('input[name="SexId"], input[name="InfantTransferredWithin24Hours"], input[name="IsInfantStillLiving"], input[name="IsInfantBeingBreastfed"]').prop("checked", false);
        $("#IsSingleBirth").prop("checked", false);
        $("#Plurality, #BirthOrder, #NameOfFacilityTransferredTo").val("");
        $('input[name="SelectedAbnormalConditionIds"], input[name="SelectedCongenitalAnomalyIds"]').prop("checked", false);
        $("#IsTimeUnknown").prop("checked", false);
        $("#NoAbnormalities, #NoCongenitalAnomalies").prop("checked", false);

        $("#newbornIsExistingPatient").val("false");
        $("#newbornMrn").val("");

        $("#transfer-facility-group").hide();

        BirthRegistryUtils.clearValidationErrors("#newborn-content");
    }

    setupMutuallyExclusiveHandlers() {
        $(document)
            .off("change", '[data-mutually-exclusive="true"]')
            .on("change", '[data-mutually-exclusive="true"]', (e) => {
                const $target = $(e.target);
                const validationGroup = $target.data("validation-group");

                if ($target.is(":checked")) {
                    $(`.validation-trigger[data-validation-group="${validationGroup}"]`)
                        .not($target)
                        .prop("checked", false);

                    if ($target.attr("id") === "IsTimeUnknown") {
                        $("#DateAndTimeOfBirth").val("");
                    }
                    if ($target.attr("id") === "IsSingleBirth") {
                        $("#Plurality, #BirthOrder").val("");
                    }
                    if ($target.attr("id") === "NoAbnormalities") {
                        $('input[name="SelectedAbnormalConditionIds"]').prop("checked", false);
                    }
                    if ($target.attr("id") === "NoCongenitalAnomalies") {
                        $('input[name="SelectedCongenitalAnomalyIds"]').prop("checked", false);
                    }
                }
            });

        $(document)
            .off("change", '#DateAndTimeOfBirth')
            .on("change", '#DateAndTimeOfBirth', (e) => {
                const $target = $(e.target);
                if ($target.val()) {
                    $("#IsTimeUnknown").prop("checked", false);
                }
                this.validateGroup("newborn1");
            });

        $(document)
            .off("change", '#Plurality, #BirthOrder')
            .on("change", '#Plurality, #BirthOrder', (e) => {
                const $target = $(e.target);
                if ($target.val()) {
                    $("#IsSingleBirth").prop("checked", false);
                }
                this.validateGroup("newborn2");
            });

        $(document)
            .off("change", 'input[name="SelectedAbnormalConditionIds"]:not([data-mutually-exclusive])')
            .on("change", 'input[name="SelectedAbnormalConditionIds"]:not([data-mutually-exclusive])', (e) => {
                const $target = $(e.target);
                if ($target.is(":checked")) {
                    $("#NoAbnormalities").prop("checked", false);
                }
                this.validateGroup("newborn3");
            });

        $(document)
            .off("change", 'input[name="SelectedCongenitalAnomalyIds"]:not([data-mutually-exclusive])')
            .on("change", 'input[name="SelectedCongenitalAnomalyIds"]:not([data-mutually-exclusive])', (e) => {
                const $target = $(e.target);
                if ($target.is(":checked")) {
                    $("#NoCongenitalAnomalies").prop("checked", false);
                }
                this.validateGroup("newborn4");
            });
    }

    validateGroup(groupName) {
        const $inputs = $(
            `.validation-trigger[data-validation-group="${groupName}"]`
        );
        const $nextBtn = $inputs
            .closest(".card-body")
            .find(".next-accordion-btn, .next-tab-btn");
        const $card = $inputs.closest(".card");

        let isValid = false;

        switch (groupName) {
            case "newborn1":
                const timeKnown = $("#DateAndTimeOfBirth").val() !== "";
                const timeUnknown = $("#IsTimeUnknown").is(":checked");
                const timeOk = timeKnown || timeUnknown;
                
                const hasBasicInfo =
                    $("#BirthWeightInLbs").val() &&
                    $("#GestationalAgeEstimateInWeeks").val() &&
                    $('input[name="SexId"]:checked').length > 0 &&
                    $("#ApgarScoreAt5Minutes").val();

                const apgar5Value = parseInt($("#ApgarScoreAt5Minutes").val(), 10);
                const apgar10Required = !isNaN(apgar5Value) && apgar5Value < 6;
                const apgar10Ok = apgar10Required ? $("#ApgarScoreAt10Minutes").val() !== "" : true;

                isValid = timeOk && hasBasicInfo && apgar10Ok;
                break;

            case "newborn2":
                const single = $("#IsSingleBirth").is(":checked");
                const hasPlurality =
                    $("#Plurality").val() && $("#BirthOrder").val();
                const pluralityOk = single || hasPlurality;

                const hasSelections =
                    $('input[name="InfantTransferredWithin24Hours"]:checked').length > 0 &&
                    $('input[name="IsInfantStillLiving"]:checked').length > 0 &&
                    $('input[name="IsInfantBeingBreastfed"]:checked').length > 0;

                const xferYes = $('input[name="InfantTransferredWithin24Hours"]:checked').val() === 'true';
                const facilityOk = xferYes ? $("#NameOfFacilityTransferredTo").val() !== "" : true;

                isValid = pluralityOk && hasSelections && facilityOk;
                break;

            case "newborn3":
            case "newborn4":
            default:
                isValid = $inputs.filter(":checked").length > 0;
        }

        $nextBtn.prop("disabled", !isValid);
        BirthRegistryUtils.updateCheckmark($card, isValid);

        return isValid;
    }

    setupSpecialTriggerHandlers() {
        $(document)
            .off("change", '[data-special-trigger="apgar5"]')
            .on("change", '[data-special-trigger="apgar5"]', (e) => {
                const apgar5Value = parseInt($(e.target).val(), 10);
                if (apgar5Value < 6) {
                    $("#ApgarScoreAt10Minutes")
                        .addClass("validation-trigger")
                        .attr("data-validation-group", "newborn1");
                } else {
                    $("#ApgarScoreAt10Minutes")
                        .removeClass("validation-trigger")
                        .removeAttr("data-validation-group");
                }
                this.validateGroup("newborn1");
            });

        $(document)
            .off("change", '[data-special-trigger="transfer"]')
            .on("change", '[data-special-trigger="transfer"]', (e) => {
                const $target = $(e.target);
                if ($target.val() === "true") {
                    $("#transfer-facility-group").show();
                    $("#NameOfFacilityTransferredTo").addClass("validation-trigger")
                        .attr("data-validation-group", "newborn2");
                } else {
                    $("#transfer-facility-group").hide();
                    $("#NameOfFacilityTransferredTo").val("")
                        .removeClass("validation-trigger")
                        .removeAttr("data-validation-group");
                }
                this.validateGroup("newborn2");
            });

        $(document)
            .off("change", "#IsTimeUnknown")
            .on("change", "#IsTimeUnknown", () => {
                this.validateGroup("newborn1");
            });
    }

    setupValidationTriggers() {
        $(document)
            .off("change", ".validation-trigger")
            .on("change", ".validation-trigger", (e) => {
                const $target = $(e.target);
                const validationGroup = $target.data("validation-group");
                this.validateGroup(validationGroup);
            });
    }

    setupValidation() {
        this.validateGroup("newborn1");
        this.validateGroup("newborn2");
        this.validateGroup("newborn3");
        this.validateGroup("newborn4");
    }

    validateAll() {
        return (
            this.validateGroup("newborn1") &&
            this.validateGroup("newborn2") &&
            this.validateGroup("newborn3") &&
            this.validateGroup("newborn4")
        );
    }

    setupSaveHandler() {
        if (this.mode === 'edit') {
            $(document)
                .off("click", "#newborn-content .save-section-btn")
                .on("click", "#newborn-content .save-section-btn", async (e) => {
                    e.preventDefault();
                    await this.saveNewbornInformation();
                });
        }
        
        $(document)
            .off("click", "#newborn-content .next-tab-btn")
            .on("click", "#newborn-content .next-tab-btn", async (e) => {
                e.preventDefault();
                await this.saveNewbornInformation();
            });
    }

    async saveSection() {
        return await this.saveNewbornInformation();
    }

    async saveNewbornInformation() {
        const birthId = this.birthRegistryManager?.birthId;

        if (!birthId) {
            BirthRegistryUtils.showAlert(
                "#newborn-content",
                "Birth record not found. Please save mother information first.",
                "danger"
            );
            return false;
        }

        const saveFunction = () => {
            return new Promise((resolve, reject) => {
                const birthDateTimeValue = $('#DateAndTimeOfBirth').val();
                const isTimeUnknown = $('#IsTimeUnknown').is(":checked");
                
                const newbornMrn = $('#newbornMrn').val();
                const newbornId = $('#newbornId').val();
                const newbornIsExistingPatient = $('#newbornIsExistingPatient').val();

                let formattedDateTime = null;
                if (!isTimeUnknown && birthDateTimeValue) {
                    formattedDateTime = birthDateTimeValue + ':00';
                }

                const data = {
                    BirthId: parseInt(birthId, 10) || null,
                    NewbornId: newbornId ? parseInt(newbornId, 10) : null,
                    NewbornMrn: newbornMrn?.trim() || null,
                    IsExistingPatient: newbornIsExistingPatient === 'true',
                    
                    FirstName: $('#FirstName').val()?.trim() || null,
                    MiddleName: $('#MiddleName').val()?.trim() || null,
                    LastName: $('#LastName').val()?.trim() || null,
                    
                    DateAndTimeOfBirth: formattedDateTime,
                    IsTimeUnknown: isTimeUnknown,
                    
                    BirthWeightInLbs: parseFloat($('#BirthWeightInLbs').val()) || null,
                    GestationalAgeEstimateInWeeks: parseInt($('#GestationalAgeEstimateInWeeks').val(), 10) || null,
                    SexId: parseInt($('input[name="SexId"]:checked').val(), 10) || null,
                    
                    ApgarScoreAt1Minute: parseInt($('#ApgarScoreAt1Minute').val(), 10) || null,
                    ApgarScoreAt5Minutes: parseInt($('#ApgarScoreAt5Minutes').val(), 10) || null,
                    ApgarScoreAt10Minutes: parseInt($('#ApgarScoreAt10Minutes').val(), 10) || null,
                    
                    IsSingleBirth: $('#IsSingleBirth').is(":checked"),
                    Plurality: parseInt($('#Plurality').val(), 10) || null,
                    BirthOrder: parseInt($('#BirthOrder').val(), 10) || null,
                    
                    InfantTransferredWithin24Hours: $('input[name="InfantTransferredWithin24Hours"]:checked').val() === 'true',
                    NameOfFacilityTransferredTo: $('#NameOfFacilityTransferredTo').val()?.trim() || null,
                    IsInfantStillLiving: $('input[name="IsInfantStillLiving"]:checked').val() === 'true',
                    IsInfantBeingBreastfed: $('input[name="IsInfantBeingBreastfed"]:checked').val() === 'true',
                    SsnrequestedForChild: $('input[name="SsnrequestedForChild"]:checked').val() === 'true',
                    
                    Comments: $('#Comments').val()?.trim() || null,
                    
                    SelectedAbnormalConditionIds: this.getSelectedAbnormalConditionIds() || [],
                    SelectedCongenitalAnomalyIds: this.getSelectedCongenitalAnomalyIds() || [],
                    NoAbnormalities: $('#NoAbnormalities').is(':checked'),
                    NoCongenitalAnomalies: $('#NoCongenitalAnomalies').is(':checked')
                };

                $.ajax({
                    url: this.saveEndpoint,
                    type: "POST",
                    contentType: "application/json",
                    headers: {
                        RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val(),
                    },
                    data: JSON.stringify(data),
                    success: resolve,
                    error: reject,
                });
            });
        };

        const onSuccess = (result) => {
            if (result.newbornId) {
                $('#newbornId').val(result.newbornId);
            }
            
            BirthRegistryUtils.clearGlobalAlerts();
            BirthRegistryUtils.showGlobalAlert(
                "Newborn information saved successfully",
                "success"
            );
        };

        const result = await BirthRegistryUtils.handleSaveWithFeedback(
            "#newborn-content",
            saveFunction,
            onSuccess
        );

        return result.success;
    }

    getSelectedAbnormalConditionIds() {
        return $('input[name="SelectedAbnormalConditionIds"]:checked')
            .map(function () {
                return parseInt(this.value, 10);
            })
            .get();
    }

    getSelectedCongenitalAnomalyIds() {
        return $('input[name="SelectedCongenitalAnomalyIds"]:checked')
            .map(function () {
                return parseInt(this.value, 10);
            })
            .get();
    }
}