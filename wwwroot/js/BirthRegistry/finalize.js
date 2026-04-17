class FinalizeModule {
    constructor(mode = "edit", birthRegistryManager = null) {
        this.mode = mode;
        this.birthRegistryManager = birthRegistryManager;
        this.saveEndpoint = "/BirthRegistry/SaveFinalizeData";
    }

    init() {
        if (this.mode === "view") {
            BirthRegistryUtils.initializeTooltips("#finalize-content");
            return;
        }

        this.setupEventHandlers();
        BirthRegistryUtils.initializeTooltips("#finalize-content");
    }

    setupEventHandlers() {
        this.setupSaveHandler();
        this.setupSignatureHandler();
    }

    setupSaveHandler() {
        $(document)
            .off("click", "#finalize-content .save-finalize-btn")
            .on("click", "#finalize-content .save-finalize-btn", async (e) => {
                e.preventDefault();
                await this.saveFinalizeData();
            });
    }

    setupSignatureHandler() {
        $(document)
            .off("input", "#RegistrarSignature")
            .on("input", "#RegistrarSignature", function() {
                if ($(this).val().trim().length > 0) {
                    $(this).addClass("signature-input");
                } else {
                    $(this).removeClass("signature-input");
                }
            });

        if ($("#RegistrarSignature").val() && $("#RegistrarSignature").val().trim().length > 0) {
            $("#RegistrarSignature").addClass("signature-input");
        }
    }

    async saveSection() {
        return await this.saveFinalizeData();
    }

    async saveFinalizeData() {
        const birthId = this.birthRegistryManager?.birthId;

        if (!birthId) {
            BirthRegistryUtils.showGlobalAlert(
                "Birth record not found. Please save other sections first.",
                "danger"
            );
            return false;
        }

        const signature = $("#RegistrarSignature").val();
        const dateValue = $("#DateOfRegistrarSignature").val(); // Get the date from the input

        if (!signature || signature.trim().length < 2) {
            BirthRegistryUtils.showGlobalAlert(
                "Please provide your signature (minimum 2 characters)",
                "warning"
            );
            $("#RegistrarSignature").focus();
            return false;
        }

        if (!dateValue || dateValue.trim() === '') {
            BirthRegistryUtils.showGlobalAlert(
                "Please select a date of signature",
                "warning"
            );
            $("#DateOfRegistrarSignature").focus();
            return false;
        }

        const saveFunction = () => {
            return new Promise((resolve, reject) => {
                const data = {
                    BirthId: birthId,
                    RegistrarSignature: signature.trim(),
                    DateOfRegistrarSignature: dateValue // Send the date as yyyy-mm-dd string
                };

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
            BirthRegistryUtils.showGlobalAlert(
                "Finalization data saved successfully",
                "success"
            );
        };

        const saveResult = await BirthRegistryUtils.handleSaveWithFeedback(
            "#finalize-content",
            saveFunction,
            onSuccess
        );

        return saveResult.success;
    }

    validateAll() {
        if (this.mode === "view") {
            return true;
        }
        
        const signature = $("#RegistrarSignature").val();
        const dateValue = $("#DateOfRegistrarSignature").val();
        return signature && signature.trim().length >= 2 && dateValue && dateValue.trim() !== '';
    }
}