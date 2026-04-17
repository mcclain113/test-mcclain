
console.log('mother.js loaded');

class MotherModule {
    clearMotherFormFields() {
        // Clear all main mother fields
        $("#motherFirstName, #motherMiddleName, #motherLastName, #motherSuffix, #birthFirstName, #birthMiddleName, #birthLastName, #birthSuffix, #motherDob, #motherBirthPlace, #motherSSN, #motherMrn, #motherMaritalStatusName, #motherTelephoneNumber, #motherResidentialApartmentNumber, #motherResidentialCounty, #motherResidentialCountry, #motherMailingApartmentNumber, #motherMailingCounty, #motherMailingCountry").val("");
        // Clear address fields
        $("[name='Mother.ResidentialAddress.Address1'], [name='Mother.ResidentialAddress.Address2'], [name='Mother.ResidentialAddress.City'], [name='Mother.ResidentialAddress.PostalCode'], [name='Mother.ResidentialAddress.County'], [name='Mother.ResidentialAddress.AddressStateID'], [name='Mother.ResidentialAddress.CountryId'], [name='Mother.MailingAddress.Address1'], [name='Mother.MailingAddress.Address2'], [name='Mother.MailingAddress.City'], [name='Mother.MailingAddress.PostalCode'], [name='Mother.MailingAddress.County'], [name='Mother.MailingAddress.AddressStateID'], [name='Mother.MailingAddress.CountryId']").val("");
        // Uncheck checkboxes
        $("#motherIsMarried, #marriedInPast, #marriedDuringPregnancy, #fatherIsHusband, #motherPaternityAcknowledgementSigned, #sameAsResidential, .race-checkbox").prop("checked", false);
        // Reset selects (use form name selectors)
        $("select[name='Mother.ReligionId'], select[name='Mother.EthnicityId'], select[name='Mother.EducationId']").val("");
        // Remove validation errors if any
        $(".is-invalid, .is-valid").removeClass("is-invalid is-valid");
    }
        setupMarriageInfoHandlers() {
            // Robust show/hide logic for parent2Options
            function updateParent2Options() {
                var marriedNo = $('#marriedNo');
                var parent2Options = $('#parent2Options');
                if (marriedNo.is(':checked')) {
                    parent2Options.show();
                } else {
                    parent2Options.hide();
                }
            }
            $(document).on('change', 'input[name="Mother.MaritalStatusAtDelivery"]', updateParent2Options);
            $(document).ready(function() {
                updateParent2Options();
            });
        }
    constructor(
        mode = "edit",
        preselectedMotherMrn = null,
        birthRegistryManager = null
    ) {
        this.mode = mode;
        this.preselectedMotherMrn = preselectedMotherMrn;
        this.birthRegistryManager = birthRegistryManager;
        this.searchEndpoint = "/BirthRegistry/SearchMother";
        this.detailsEndpoint = "/BirthRegistry/GetMotherDetails";
        this.saveEndpoint = "/BirthRegistry/SaveMotherData";
        this.createPatientUrl = "/Patient/CreatePatient";
    }

    init() {
        if (this.mode === "view") {
            BirthRegistryUtils.initializeTooltips("#mother-content");
            return;
        }

        this.setupEventHandlers();
        this.setupModalHandler();
        this.setupValidation();
        this.setupSaveHandler(); // Only call once here
        BirthRegistryUtils.initializeTooltips("#mother-content");
        this.setupMarriageInfoHandlers();

        const hasMotherMrn = $("#motherMrn").val();
        const isExistingPatient = $("#isExistingPatient").val() === "true";

        if (hasMotherMrn && isExistingPatient) {
            $("#motherInformationAccordion").removeClass("d-none");
            $("#patientSearchSection").addClass("d-none");
            $('input[name="motherPatientStatus"]').closest(".row").addClass("d-none");
            this.validateGroup("motherDemo");
            this.validateGroup("motherAddress");
            this.validateGroup("motherCultural");
            this.validateGroup("motherMarriage");
        } else {
            // Ensure correct accordion visibility on page load for not-in-database
            const checkedRadio = $('input[name="motherPatientStatus"]:checked');
            if (checkedRadio.length) {
                this.handleMotherStatusChange({ target: checkedRadio[0] });
            }
        }
    }

    setupEventHandlers() {
        this.setupMotherStatusHandlers();
        this.setupAddressHandlers();
        this.setupPatientSearchHandlers();
        this.setupMotherLookupModalHandlers();
        this.setupValidationTriggers();
        this.setupSpecialTriggerHandlers();
        this.setupSaveHandler();
    }

    setupModalHandler() {
    $(document)
        .off("click", "#openMotherLookupBtn")
        .on("click", "#openMotherLookupBtn", (e) => {
            e.preventDefault();
            $("#motherLookupModal").modal("show");
        });
    }

    setupMotherStatusHandlers() {
        $(document)
            .off("change", 'input[name="motherPatientStatus"]')
            .on("change", 'input[name="motherPatientStatus"]', (e) => {
                this.handleMotherStatusChange(e);
            });
    }

    setupAddressHandlers() {
        $(document)
            .off(
                "input change",
                'input[name*="ResidentialAddress"], select[name*="ResidentialAddress"]'
            )
            .on(
                "input change",
                'input[name*="ResidentialAddress"], select[name*="ResidentialAddress"]',
                () => {
                    if ($("#sameAsResidential").is(":checked")) {
                        this.copyResidentialToMailingAddress();
                    }
                    this.validateGroup("motherAddress");
                }
            );
    }

    setupPatientSearchHandlers() {
       
        $(document)
            .off("click", "#createNewPatientBtn")
            .on("click", "#createNewPatientBtn", (e) => {
                this.handleCreateNewPatient(e);
            });

        $(document)
            .off("click", "#motherLookupSearchBtn")
            .on("click", "#motherLookupSearchBtn", (e) => {
                e.preventDefault();
                this.handleMotherLookupSearch();
            });

        $(document)
            .off("keypress", "#mlFirstName, #mlLastName, #mlMRN, #mlSSN")
            .on("keypress", "#mlFirstName, #mlLastName, #mlMRN, #mlSSN", (e) => {
                if (e.which === 13) {
                    e.preventDefault();
                    this.handleMotherLookupSearch();
                }
            });

        $(document)
            .off("click", ".btn-select-mother")
            .on("click", ".btn-select-mother", (e) => {
                const mrn = $(e.currentTarget).data("mrn");
                this.selectPatient(mrn);        
                $("#motherLookupModal").modal("hide");
            });
    }

    //motherlookup handlers
        setupMotherLookupModalHandlers() {
            const modalSelector = "#motherLookupModal";

            $(document)
                .off("click", "#motherLookupSearchBtn")
                .on("click", "#motherLookupSearchBtn", async (e) => {
                    e.preventDefault();
                    await this.runMotherLookupSearch();
                });

            $(document)
                .off("keypress", `${modalSelector} input`)
                .on("keypress", `${modalSelector} input`, async (e) => {
                    if (e.which === 13) {
                        e.preventDefault();
                        await this.runMotherLookupSearch();
                    }
                });

            $(document)
                .off("click", ".select-patient")
                .on("click", ".select-patient", async (e) => {
                    e.preventDefault();
                    e.stopPropagation();

                    const mrn = $(e.currentTarget).data("mrn");
                    console.log("Select clicked, MRN:", mrn);

                    await this.selectPatient(mrn);
                    $("#motherLookupModal").modal("hide");
                });

            $(document)
                .off("show.bs.modal", "#motherLookupModal")
                .on("show.bs.modal", "#motherLookupModal", () => {
                    $("#motherLookupResultsHost").empty();
                    $("#motherLookupResultsWrap").addClass("d-none");
                    $("#motherLookupModal input").val("");
                });
        }

        async runMotherLookupSearch() {
            const $btn = $("#motherLookupSearchBtn");
            const original = $btn.html();

            const params = new URLSearchParams({
                searchFirst: $("#searchFirst").val()?.trim() || "",
                searchLast: $("#searchLast").val()?.trim() || "",
                searchMRN: $("#searchMRN").val()?.trim() || "",
                searchSSN: $("#searchSSN").val()?.trim() || "",
                searchDOB: $("#searchDOB").val() || "",
                searchDOBBefore: $("#searchDOBBefore").val() || ""
            });

            if (!params.get("searchLast") &&
                !params.get("searchFirst") &&
                !params.get("searchMRN") &&
                !params.get("searchSSN") &&
                !params.get("searchDOB") &&
                !params.get("searchDOBBefore")) {

                BirthRegistryUtils.showAlert(
                    "#mother-content",
                    "Enter at least one search field.",
                    "warning"
                );
                return;
            }

            $btn.html('<i class="fas fa-spinner fa-spin"></i> Searching...').prop("disabled", true);

            try {
                const url = `/BirthRegistry/SearchMothers?${params.toString()}`;
                const resp = await fetch(url, { method: "GET" });

                if (!resp.ok) throw new Error(`HTTP ${resp.status}`);

                const html = await resp.text();

                $("#motherLookupResultsHost").html(html);
                $("#motherLookupResultsWrap").removeClass("d-none");
            } catch (err) {
                BirthRegistryUtils.showAlert("#mother-content", "Search failed. Try again.", "danger");
            } finally {
                $btn.html(original).prop("disabled", false);
            }
        }

    setupSpecialTriggerHandlers() {
        $(document)
            .off("change", '[data-special-trigger="sameAddress"]')
            .on("change", '[data-special-trigger="sameAddress"]', (e) => {
                this.handleSameAddressChange(e);
            });
        BirthRegistryUtils.setupSkipDemographicsHandler("#mother-content", this);
    }

    setupSaveHandler() {
        if (this.mode === 'edit') {
            $(document)
                .off("click", "#mother-content .save-section-btn")
                .on("click", "#mother-content .save-section-btn", async (e) => {
                    e.preventDefault();
                    await this.saveMotherData();
                });
            // Only bind to .save-section-btn, not .next-tab-btn, to avoid double save
        }
        // Removed next-tab-btn handler to avoid double save
    }

    async saveSection() {
        return await this.saveMotherData();
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
        this.validateGroup("motherDemo");
        this.validateGroup("motherAddress");
        this.validateGroup("motherCultural");
        this.validateGroup("motherMarriage");
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

        const isExistingPatient = $('#isExistingPatient').val() === 'true';
        if (isExistingPatient) {
            isValid = true;
            $nextBtn.prop("disabled", false);
            BirthRegistryUtils.updateCheckmark($card, true);
            return true;
        }

        switch (groupName) {
            case "motherDemo":
                const firstName = $("#motherFirstName").val();
                const lastName = $("#motherLastName").val();
                const dob = $("#motherDob").val();
                isValid = firstName && lastName && dob && 
                         firstName.trim() && lastName.trim();
                break;

            case "motherAddress":
                const address1 = $('input[name="Mother.ResidentialAddress.Address1"]').val();
                const city = $('input[name="Mother.ResidentialAddress.City"]').val();
                const state = $('select[name="Mother.ResidentialAddress.AddressStateID"]').val();
                const zip = $('input[name="Mother.ResidentialAddress.PostalCode"]').val();

                const zipValid = zip && /^[0-9]{5}(-[0-9]{4})?$/.test(zip);
                isValid = address1 && city && state && zip && 
                         address1.trim() && city.trim() && zipValid;
                break;

            case 'motherCultural':
                if ($inputs.length === 0) {
                    isValid = true;
                } else {
                    isValid = BirthRegistryUtils.validateGroupWithSkipOption(
                        groupName,
                        this,
                        'skipMotherDemo',
                        { type: 'select', query: 'select[name="Mother.ReligionId"]' },
                        { type: 'select', query: 'select[name="Mother.EthnicityId"]' },
                        { type: 'select', query: 'select[name="Mother.EducationId"]' },
                        { type: 'checkbox', query: '.race-checkbox' }
                    );
                }
                break;

            default:
                isValid =
                    $inputs.filter(":checked").length > 0 ||
                    $inputs.filter((i, el) => {
                        const val = $(el).val();
                        return val && val.trim() !== "";
                    }).length > 0;
        }

        $nextBtn.prop("disabled", !isValid);
        BirthRegistryUtils.updateCheckmark($card, isValid);

        return isValid;
    }

    handleMotherStatusChange(e) {
        const selectedValue = $(e.target).val();

        if (selectedValue === "notInDatabase") {
            $("#patientSearchSection").addClass("d-none");
            $("#searchResults").addClass("d-none");
            $("#searchResultsBody").empty();
            $("#patientSearchTerm").val("");

            this.clearMotherFormFields();
            $("#motherInformationAccordion").removeClass("d-none");
            $("#isExistingPatient").val("false");
            $("#motherMrn").val("");

            setTimeout(() => {
                $("#motherFirstName").focus();
            }, 100);

            BirthRegistryUtils.showAlert(
                "#mother-content",
                "Mother information will be entered manually (not saved to patient database)",
                "info"
            );
        } else {
            $("#patientSearchSection").removeClass("d-none");
            $("#motherInformationAccordion").addClass("d-none");
            this.clearMotherFormFields();

            BirthRegistryUtils.showAlert(
                "#mother-content",
                "Search for existing female patients or create new patient record",
                "info"
            );
        }
    }

    handleSameAddressChange(e) {
        const isChecked = $(e.target).is(":checked");

        if (isChecked) {
            $("#mailingAddressSection").addClass("d-none");
            this.copyResidentialToMailingAddress();
        } else {
            $("#mailingAddressSection").removeClass("d-none");
        }
    }

    handleCreateNewPatient(e) {
        window.open(this.createPatientUrl, "_blank");
        BirthRegistryUtils.showAlert(
            "#mother-content",
            "New patient creation page opened. After creating the patient, return here and search for them.",
            "info"
        );
    }

    handlePatientSearch(e) {
        const searchTerm = $("#patientSearchTerm").val();

        if (!searchTerm || searchTerm.trim() === "") {
            BirthRegistryUtils.showAlert(
                "#mother-content",
                "Please enter a search term",
                "warning"
            );
            return;
        }

        this.searchPatients(searchTerm);
    }

    async searchPatients(searchTerm) {
        const $searchBtn = $('#searchPatientBtn');
        const originalText = $searchBtn.html();

        $searchBtn
            .html('<i class="fas fa-spinner fa-spin"></i> Searching...')
            .prop("disabled", true);

        try {
            const response = await fetch(`${this.searchEndpoint}?searchTerm=${encodeURIComponent(searchTerm)}`);
            const result = await response.json();

            if (result.success && result.patients && result.patients.length > 0) {
                this.displaySearchResults(result.patients);
            } else {
                BirthRegistryUtils.showAlert('#mother-content', 
                    'No female patients found matching your search criteria.', 'info');
                $('#searchResults').addClass('d-none');
            }
        } catch (error) {
            BirthRegistryUtils.showAlert('#mother-content', 
                'An error occurred during the search. Please try again.', 'danger');
        } finally {
            $searchBtn.html(originalText).prop('disabled', false);
        }
    }

    displaySearchResults(patients) {
        const tbody = $("#searchResultsBody");
        tbody.empty();

        if (!patients || patients.length === 0) {
            tbody.append(
                '<tr><td colspan="5" class="text-center text-muted">No patients found</td></tr>'
            );
        } else {
            patients.forEach((patient) => {
                const row = `
                    <tr>
                        <td><strong>${patient.mrn || "N/A"}</strong></td>
                        <td>
                            <strong>${patient.firstName || ""} ${patient.middleName || ""} ${patient.lastName || ""}</strong>
                            <br><small class="text-muted">${patient.sex || "Not specified"}</small>
                        </td>
                        <td>${patient.dob ? BirthRegistryUtils.formatDate(patient.dob) : "N/A"}</td>
                        <td>${patient.ssn ? BirthRegistryUtils.formatSSN(patient.ssn) : "N/A"}</td>
                        <td>
                            <button class="btn btn-sm btn-primary select-patient" data-mrn="${patient.mrn}">
                                <i class="fas fa-check"></i> Select
                            </button>
                        </td>
                    </tr>
                `;
                tbody.append(row);
            });
        }

        $("#searchResults").removeClass("d-none");
    }

    async selectPatient(mrn) {
        const $selectBtn = $(`.select-patient[data-mrn="${mrn}"]`);
        const originalText = $selectBtn.html();

        $selectBtn
            .html('<i class="fas fa-spinner fa-spin"></i> Loading...')
            .prop("disabled", true);

        const birthId = this.birthRegistryManager?.birthId || null;
        const url = `${this.detailsEndpoint}?mrn=${encodeURIComponent(mrn)}&mode=${this.mode}${birthId ? `&birthId=${birthId}` : ''}`;
        try {
            const response = await fetch(url);
            const html = await response.text();
            $('#motherInformationAccordion').html(html);

            // Check for backend error message in the HTML
            if (html.includes('alert-danger')) {
                // Try to extract error message from HTML
                let errorMsg = 'Unknown error loading mother information.';
                const match = html.match(/<div class='alert alert-danger'>(.*?)<\/div>/);
                if (match && match[1]) {
                    errorMsg = match[1];
                }
                BirthRegistryUtils.showAlert('#mother-content', errorMsg, 'danger', false);
                return;
            }

            // Explicitly set IsExistingPatient and FirstName after HTML injection
            $('#motherAccordion #isExistingPatient').val('true');
            // Try to set FirstName from the patient row (if available)
            const selectedRow = $(`.select-patient[data-mrn="${mrn}"]`).closest('tr');
            let firstName = null;
            if (selectedRow.length) {
                // Try to extract first name from the Name column (second td)
                const nameText = selectedRow.find('td').eq(1).text().trim();
                if (nameText) {
                    // Split by space and take the first part
                    firstName = nameText.split(' ')[0];
                }
            }
            if (firstName) {
                $('#motherAccordion #motherFirstName').val(firstName);
            }
            $('#patientSearchSection').addClass('d-none');
            $('#motherInformationAccordion').removeClass('d-none');
            $('#searchResults').addClass('d-none');
            $('input[name="motherPatientStatus"]').closest(".row").addClass("d-none");

            this.setupValidationTriggers();
            this.setupSpecialTriggerHandlers();
            // this.setupSaveHandler(); // Only call once here

            BirthRegistryUtils.initializeTooltips("#mother-content");
            // this.markAllSectionsValid(); // Removed: function does not exist. Use validateAll if needed.
            this.validateAll();

            BirthRegistryUtils.showAlert('#mother-content', 'Mother information loaded successfully', 'success', true);
        } catch (error) {
            // Only show the error alert if the AJAX call itself fails (network or JS error)
            BirthRegistryUtils.showAlert('#mother-content', 
                'AJAX/network error: ' + (error && error.message ? error.message : 'Unknown error'), 'danger', false);
        } finally {
            $selectBtn.html(originalText).prop('disabled', false);
        }
    }

    async saveMotherData() {
        const $scope = $('#motherAccordion');
        const birthId = this.birthRegistryManager?.birthId;

        // Gather all mother info fields from the form
        const residentialStateId = $scope.find("[name='Mother.ResidentialAddress.AddressStateID']").val();
        const mailingStateId = $scope.find("[name='Mother.MailingAddress.AddressStateID']").val();
        const data = {
            BirthId: birthId,
            Mrn: $scope.find("#motherMrn").val(),
            IsExistingPatient: $scope.find("#isExistingPatient").val() === "true",
            FirstName: $scope.find("#motherFirstName").val(),
            MiddleName: $scope.find("#motherMiddleName").val(),
            LastName: $scope.find("#motherLastName").val(),
            Suffix: $scope.find("#motherSuffix").val(),
            BirthFirstName: $scope.find("#birthFirstName").val(),
            BirthMiddleName: $scope.find("#birthMiddleName").val(),
            BirthLastName: $scope.find("#birthLastName").val(),
            BirthSuffix: $scope.find("#birthSuffix").val(),
            IsMarried: $scope.find("#motherIsMarried").is(":checked"),
            DateOfBirth: $scope.find("#motherDob").val(),
            BirthPlace: $scope.find("#motherBirthPlace").val(),
            SSN: $scope.find("#motherSSN").val(),
            MailingAddressSameAsResidential: $scope.find("#sameAsResidential").is(":checked"),
            HouseholdInsideCityLimits: $scope.find("#motherHouseholdInsideCityLimits").val(),
            // Address fields
            ResidentialAddress: {
                Address1: $scope.find("[name='Mother.ResidentialAddress.Address1']").val(),
                Address2: $scope.find("[name='Mother.ResidentialAddress.Address2']").val(),
                City: $scope.find("[name='Mother.ResidentialAddress.City']").val(),
                PostalCode: $scope.find("[name='Mother.ResidentialAddress.PostalCode']").val(),
                County: $scope.find("[name='Mother.ResidentialAddress.County']").val(),
                AddressStateID: residentialStateId === "" ? null : residentialStateId,
                CountryId: $scope.find("[name='Mother.ResidentialAddress.CountryId']").val(),
            },
            MailingAddress: {
                Address1: $scope.find("[name='Mother.MailingAddress.Address1']").val(),
                Address2: $scope.find("[name='Mother.MailingAddress.Address2']").val(),
                City: $scope.find("[name='Mother.MailingAddress.City']").val(),
                PostalCode: $scope.find("[name='Mother.MailingAddress.PostalCode']").val(),
                County: $scope.find("[name='Mother.MailingAddress.County']").val(),
                AddressStateID: mailingStateId === "" ? null : mailingStateId,
                CountryId: $scope.find("[name='Mother.MailingAddress.CountryId']").val(),
            },
            ResidentialApartmentNumber: $scope.find("#motherResidentialApartmentNumber").val(),
            ResidentialCounty: $scope.find("#motherResidentialCounty").val(),
            ResidentialCountry: $scope.find("#motherResidentialCountry").val(),
            MailingApartmentNumber: $scope.find("#motherMailingApartmentNumber").val(),
            MailingCounty: $scope.find("#motherMailingCounty").val(),
            MailingCountry: $scope.find("#motherMailingCountry").val(),
            MotherTelephoneNumber: $scope.find("#motherTelephoneNumber").val(),
            Religion: (function() { var v = $scope.find("select[name='Mother.ReligionId']").val(); return v ? { ReligionId: v } : null; })(),
            Ethnicity: (function() { var v = $scope.find("select[name='Mother.EthnicityId']").val(); return v ? { EthnicityId: v } : null; })(),
            EducationLevel: (function() { var v = $scope.find("select[name='Mother.EducationId']").val(); return v ? { EducationId: v } : null; })(),
            MotherMarriedInPast: $scope.find("#marriedInPast").is(":checked"),
            MotherMarriedDuringPregnancy: $scope.find("#marriedDuringPregnancy").is(":checked"),
            IsFatherHusbandOfMother: $scope.find("#fatherIsHusband").is(":checked"),
            MaritalStatusName: $scope.find("#motherMaritalStatusName").val(),
            PaternityAcknowledgementSigned: $scope.find("#motherPaternityAcknowledgementSigned").is(":checked"),
            SelectedRaceIds: $scope.find(".race-checkbox:checked").map(function() { return $(this).val(); }).get(),
            // Add more fields as needed
        };

        // Debug: log the entire accordion HTML and key field values

        // Frontend validation for new mothers
        const isExistingPatient = $scope.find("#isExistingPatient").val() === "true";
        const firstName = $scope.find("#motherFirstName").val();
        if (!isExistingPatient && (!firstName || !firstName.trim())) {
            BirthRegistryUtils.showAlert(
                "#mother-content",
                "Mother first name is required (client-side validation)",
                "danger"
            );
            return false;
        }

        // Get anti-forgery token from the form
        var token = $("input[name='__RequestVerificationToken']").val();
        const saveFunction = () => {
            return new Promise((resolve, reject) => {
                $.ajax({
                    url: this.saveEndpoint,
                    type: "POST",
                    contentType: "application/json",
                    data: JSON.stringify(data),
                    headers: {
                        'RequestVerificationToken': token
                    },
                    success: function(res) { resolve(res); },
                    error: function(xhr) { reject(xhr); },
                });
            });
        };

        const onSuccess = async (result) => {
            if (this.birthRegistryManager && result.birthId) {
                this.birthRegistryManager.setBirthId(result.birthId);
            }
            BirthRegistryUtils.clearGlobalAlerts();
            BirthRegistryUtils.showGlobalAlert(
                "Mother information saved successfully",
                "success"
            );
            // Reload mother data (including races) from backend
            const mrn = $("#motherMrn").val();
            if (mrn) {
                await this.selectPatient(mrn);
            }
        };

        const result = await BirthRegistryUtils.handleSaveWithFeedback(
            "#mother-content",
            saveFunction,
            onSuccess
        );

        return result.success;
    }

    validateAll() {
        if (this.mode === "view") {
            return true;
        }
        
        return (
            this.validateGroup("motherDemo") &&
            this.validateGroup("motherAddress") &&
            this.validateGroup("motherCultural")
        );
    }

    async handleMotherLookupSearch() {
    const first = $("#mlFirstName").val() || "";
    const last = $("#mlLastName").val() || "";
    const mrn = $("#mlMRN").val() || "";
    const ssn = $("#mlSSN").val() || "";
    const dobAfter = $("#mlDobAfter").val() || "";
    const dobBefore = $("#mlDobBefore").val() || "";

    const qs = new URLSearchParams({
        searchFirst: first,
        searchLast: last,
        searchMRN: mrn,
        searchSSN: ssn,
        searchDOB: dobAfter,
        searchDOBBefore: dobBefore
    });

    try {
        $("#motherLookupResultsContainer").html(`<div class="text-muted"><i class="fas fa-spinner fa-spin"></i> Searching...</div>`);

        const response = await fetch(`/BirthRegistry/SearchMothers?${qs.toString()}`);

        if (!response.ok) throw new Error(`HTTP ${response.status}`);

        const html = await response.text();
        $("#motherLookupResultsContainer").html(html);

    } catch (err) {
        $("#motherLookupResultsContainer").html(`<div class="alert alert-danger">Search failed. Try again.</div>`);
    }
}
}

window.MotherModule = MotherModule;
console.log("mother.js loaded, MotherModule attached to window");


