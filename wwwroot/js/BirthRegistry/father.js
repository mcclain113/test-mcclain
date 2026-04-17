class FatherModule {
    constructor(mode = "edit", birthRegistryManager = null) {
        this.mode = mode;
        this.birthRegistryManager = birthRegistryManager;
        this.saveEndpoint = "/BirthRegistry/SaveFatherData";
    }

    init() {
        if (this.mode === "view") {
            BirthRegistryUtils.initializeTooltips("#father-content");
            return;
        }

        this.setupEventHandlers();
        this.setupValidation();
        BirthRegistryUtils.initializeTooltips("#father-content");
    }

    setupEventHandlers() {
        this.setupSpecialTriggerHandlers();
        this.setupValidationTriggers();
        this.setupSaveHandler();
    }

    setupSpecialTriggerHandlers() {
        $(document)
            .off("change", '[data-special-trigger="paternity"]')
            .on("change", '[data-special-trigger="paternity"]', (e) => {
                this.handlePaternityChange(e);
            });
        BirthRegistryUtils.setupSkipDemographicsHandler("#father-content", this);
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

    setupSaveHandler() {
        $(document)
            .off("click", "#father-content .next-tab-btn, #father-content .save-tab-btn")
            .on("click", "#father-content .next-tab-btn, #father-content .save-tab-btn", async (e) => {
                e.preventDefault();
                await this.saveFatherData();
            });
    }

    async saveSection() {
        return await this.saveFatherData();
    }

    setupValidation() {
        this.validateGroup("fatherPaternity");
        this.validateGroup("fatherName");
        this.validateGroup("fatherPersonal");
        this.validateGroup("fatherDemo");

        $("#collapseFatherPersonal .next-accordion-btn").prop("disabled", false);
        $("#collapseFatherDemographic .next-tab-btn").prop("disabled", false);

        BirthRegistryUtils.updateCheckmark(
            $("#collapseFatherPersonal").closest(".card"),
            true
        );
        BirthRegistryUtils.updateCheckmark(
            $("#collapseFatherDemographic").closest(".card"),
            true
        );
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
            case "fatherPaternity":
                isValid =
                    $('input[name="Father.HasPaternityAcknowledgement"]:checked').length >
                    0;
                break;

            case "fatherName":
                const firstName = $('input[name="Father.FirstName"]').val();
                const lastName = $('input[name="Father.LastName"]').val();
                isValid = firstName && lastName && firstName.trim() && lastName.trim();
                break;

            case "fatherPersonal":
                isValid = true;
                break;

            case "fatherDemo":
                const skipDemo = $("#skipFatherDemo").is(":checked");

                if (skipDemo) {
                    isValid = true;
                } else {
                    isValid = true;
                }
                break;

            default:
                isValid =
                    $inputs.filter(":checked").length > 0 ||
                    $inputs.filter((i, el) => $(el).val() && $(el).val().trim() !== "").length > 0;
        }

        $nextBtn.prop("disabled", !isValid);
        BirthRegistryUtils.updateCheckmark($card, isValid);

        return isValid;
    }

    handlePaternityChange(e) {
        const hasPaternity = $(e.target).val() === "True";

        $("#paternityStatusMessage").fadeIn(300);

        if (hasPaternity) {
            $("#paternityYesMessage").fadeIn(300);
            $("#paternityNoMessage").hide();
        } else {
            $("#paternityYesMessage").hide();
            $("#paternityNoMessage").fadeIn(300);
        }

        setTimeout(() => {
            $("#paternityStatusMessage").fadeOut(300);
        }, 4000);

        this.validateGroup("fatherPaternity");
    }

    async saveFatherData() {
        const birthId = this.birthRegistryManager?.birthId;

        if (!birthId) {
            BirthRegistryUtils.showGlobalAlert(
                "Birth record not found. Please save mother information first.",
                "danger"
            );
            return false;
        }

        const hasPaternityValue = $(
            'input[name="Father.HasPaternityAcknowledgement"]:checked'
        ).val();

        if (!hasPaternityValue) {
            BirthRegistryUtils.showGlobalAlert(
                "Please indicate whether paternity acknowledgement has been completed",
                "warning"
            );
            return false;
        }

        const paternityAcknowledged = hasPaternityValue === "True";
        const firstName = $('input[name="Father.FirstName"]').val();
        const lastName = $('input[name="Father.LastName"]').val();

        if (paternityAcknowledged && (!firstName || !lastName)) {
            BirthRegistryUtils.showGlobalAlert(
                "Father's first and last name are required when paternity is acknowledged",
                "warning"
            );
            return false;
        }

        const saveFunction = () => {
            return new Promise((resolve, reject) => {
                const educationId = $('select[name="Father.EducationLevel.EducationId"]').val();
                const ethnicityId = $('select[name="Father.Ethnicity.EthnicityId"]').val();
                
                const data = {
                    BirthId: birthId,
                    HasPaternityAcknowledgement: paternityAcknowledged,
                    FirstName: firstName || null,
                    MiddleName: $('input[name="Father.MiddleName"]').val() || null,
                    LastName: lastName || null,
                    Suffix: $('input[name="Father.Suffix"]').val() || null,
                    DateOfBirth: $('input[name="Father.DateOfBirth"]').val() || null,
                    FatherBirthplace: $('input[name="Father.FatherBirthplace"]').val() || null,
                    SSN: $('input[name="Father.SSN"]').val() || null,
                    EducationLevel: educationId ? { EducationId: parseInt(educationId) } : null,
                    Ethnicity: ethnicityId ? { EthnicityId: parseInt(ethnicityId) } : null,
                    SelectedRaceIds: []
                };
                
                $(".father-race:checked").each(function () {
                    data.SelectedRaceIds.push(parseInt($(this).val()));
                });

                $.ajax({
                    url: this.saveEndpoint,
                    type: "POST",
                    contentType: "application/json",
                    data: JSON.stringify(data),
                    headers: {
                        RequestVerificationToken: $(
                            'input[name="__RequestVerificationToken"]'
                        ).val(),
                    },
                    success: resolve,
                    error: reject,
                });
            });
        };

        const onSuccess = (result) => {
            BirthRegistryUtils.clearGlobalAlerts();
            
            const alertType = result.paternityAcknowledged ? "success" : "info";
            BirthRegistryUtils.showGlobalAlert(result.message, alertType);
        };

        const saveResult = await BirthRegistryUtils.handleSaveWithFeedback(
            "#father-content",
            saveFunction,
            onSuccess
        );

        return saveResult.success;
    }

    validateAll() {
        if (this.mode === "view") {
            return true;
        }
        
        return (
            this.validateGroup("fatherPaternity") &&
            this.validateGroup("fatherName") &&
            this.validateGroup("fatherPersonal") &&
            this.validateGroup("fatherDemo")
        );
    }
}