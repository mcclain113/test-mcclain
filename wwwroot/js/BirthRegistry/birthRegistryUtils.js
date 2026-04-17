class BirthRegistryUtils {
    static getAlertIcon(type) {
        const icons = {
            success: "check-circle",
            info: "info-circle",
            warning: "exclamation-triangle",
            danger: "exclamation-circle",
        };
        return icons[type] || "info-circle";
    }

    static async handleSaveWithFeedback(
        containerSelector,
        saveFunction,
        onSuccess = null,
        displayDuration = 1000
    ) {
        try {
            const result = await saveFunction();

            if (result.success) {
                await new Promise((resolve) => setTimeout(resolve, displayDuration));

                if (onSuccess && typeof onSuccess === "function") {
                    onSuccess(result);
                }

                return { success: true, result };
            } else {
                this.showGlobalAlert(
                    result.message || "Save failed",
                    "danger"
                );
                return { success: false, result };
            }
        } catch (error) {
            this.showGlobalAlert(
                "An error occurred while saving. Please try again.",
                "danger"
            );
            return { success: false, error };
        }
    }

    static showAlert(
        containerSelector,
        message,
        type = "info",
        autoHide = true,
        persistent = false
    ) {
        const alertHtml = `
            <div class="alert alert-${type} alert-dismissible fade show${
            persistent ? " alert-persistent" : ""
        }" role="alert">
                <i class="fas fa-${this.getAlertIcon(type)}"></i> ${message}
                <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
        `;

        if (!persistent) {
            $(containerSelector).find(".alert:not(.alert-persistent)").remove();
        } else {
            $(containerSelector).find(".alert").remove();
        }

        $(containerSelector).prepend(alertHtml);

        if (autoHide && (type === "success" || type === "info")) {
            setTimeout(() => {
                $(containerSelector)
                    .find(".alert:not(.alert-persistent)")
                    .fadeOut(300, function () {
                        $(this).remove();
                    });
            }, 5000);
        }
    }

    static showGlobalAlert(message, type = "info", autoHide = false) {
        this.showAlert("#global-alert-container", message, type, autoHide, true);
    }

    static clearGlobalAlerts() {
        $("#global-alert-container .alert").fadeOut(300, function () {
            $(this).remove();
        });
    }

    static formatDate(dateString) {
        if (!dateString) return "";
        try {
            const normalized = dateString.includes("-")
                ? dateString
                : dateString.replace(/(\d{2})\/(\d{2})\/(\d{4})/, "$3-$1-$2");
            const date = new Date(normalized);
            if (isNaN(date)) return dateString;
            return date.toLocaleDateString("en-US", {
                year: "numeric",
                month: "2-digit",
                day: "2-digit",
            });
        } catch (e) {
            return dateString;
        }
    }

    static formatSSN(ssn) {
        if (!ssn) return "";
        const cleaned = ssn.replace(/\D/g, "");
        if (cleaned.length === 9) {
            return `${cleaned.slice(0, 3)}-${cleaned.slice(3, 5)}-${cleaned.slice(
                5
            )}`;
        }
        return ssn;
    }

    static initializeTooltips(containerSelector = "body") {
        $(`${containerSelector} [data-toggle="tooltip"]`).tooltip("dispose");

        $(`${containerSelector} [data-toggle="tooltip"]`).tooltip({
            trigger: "hover",
            container: "body",
            html: true,
            boundary: "window",
        });
    }

    static clearValidationErrors(containerSelector) {
        $(containerSelector).find(".error").removeClass("error");
        $(containerSelector)
            .find(".validation-summary-custom, .tab-validation-summary")
            .remove();
    }

    static updateCheckmark($card, isValid) {
        const $checkmark = $card.find(".checkmark");

        if (isValid) {
            $checkmark.addClass("show");
        } else {
            $checkmark.removeClass("show");
        }
    }

    static setupSkipDemographicsHandler(containerSelector, moduleInstance) {
        $(document)
            .off("change", `${containerSelector} [data-special-trigger="skipDemo"]`)
            .on(
                "change",
                `${containerSelector} [data-special-trigger="skipDemo"]`,
                (e) => {
                    this.handleSkipDemographics(e, moduleInstance);
                }
            );
    }

    static handleSkipDemographics(e, moduleInstance) {
        const $checkbox = $(e.target);
        const isSkipped = $checkbox.is(":checked");
        const validationGroup = $checkbox.data("validation-group");
        const $cardBody = $checkbox.closest(".card-body");
        const $skipCard = $checkbox.closest(".card.border-left-warning");
        const $infoText = $skipCard.nextAll("p.text-muted").first();
        const $demographicRow = $skipCard.nextAll(".row").first();

        if (isSkipped) {
            $infoText.fadeOut(300);
            $demographicRow.fadeOut(300);

            $demographicRow.find("select").each(function () {
                $(this).val("").prop("disabled", true);
            });

            $demographicRow.find('input[type="checkbox"]').each(function () {
                $(this).prop("checked", false).prop("disabled", true);
            });

            this.showAlert(
                $skipCard,
                "<strong>Demographic section skipped.</strong> All fields have been cleared and hidden. Uncheck to re-enable.",
                "info",
                false
            );
        } else {
            $infoText.fadeIn(300);
            $demographicRow.fadeIn(300);

            $demographicRow.find("select").each(function () {
                $(this).prop("disabled", false);
            });

            $demographicRow.find('input[type="checkbox"]').each(function () {
                $(this).prop("disabled", false);
            });

            $skipCard.find(".alert").fadeOut(300, function () {
                $(this).remove();
            });

            this.showAlert(
                $skipCard,
                "Demographic fields re-enabled. You can now fill in the information.",
                "success",
                true
            );
        }

        if (moduleInstance && typeof moduleInstance.validateGroup === "function") {
            moduleInstance.validateGroup(validationGroup);
        }
    }

    static validateGroupWithSkipOption(
        groupName,
        moduleInstance,
        skipCheckboxId,
        ...validationSelectors
    ) {
        const skipDemo = $(`#${skipCheckboxId}`).is(":checked");

        if (skipDemo) {
            return true;
        }

        let hasAnyValue = false;

        for (const selector of validationSelectors) {
            if (selector.type === "select") {
                if ($(selector.query).val()) {
                    hasAnyValue = true;
                    break;
                }
            } else if (selector.type === "checkbox") {
                if ($(selector.query + ":checked").length > 0) {
                    hasAnyValue = true;
                    break;
                }
            } else if (selector.type === "input") {
                if ($(selector.query).val().trim()) {
                    hasAnyValue = true;
                    break;
                }
            }
        }
        return true;
    }
}