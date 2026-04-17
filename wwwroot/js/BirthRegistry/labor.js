class LaborModule {
    constructor(mode = "edit", birthRegistryManager = null) {
        this.mode = mode;
        this.birthRegistryManager = birthRegistryManager;
        this.saveEndpoint = "/BirthRegistry/SaveLaborAndDelivery";
        this.isSaving = false;
    }

    init() {
        if (!$("#laborAccordion").length) return;

        if (this.mode === "view") {
            this.hydrateTooltips();
            return;
        }

        this.setupEventHandlers();
        this.setupValidation();
        this.setupSaveHandler();
        this.hydrateTooltips();
        this.applyInitialState();
    }

    applyInitialState() {
        this.toggleNone("#characteristicsNone", 'input[name="SelectedCharacteristicIds"]');
        this.toggleNone("#morbidityNone", 'input[name="SelectedMaternalMorbidityIds"]');
        this.validateSection1();
        this.validateSection2();
        this.validateSection3();
    }

    setupEventHandlers() {
        $(document).off(".labor");

        $(document).on("labor-hydrate-tooltips.labor", () => this.hydrateTooltips());

        $(document).on("change.labor", 'input[name="SelectedCharacteristicIds"]', (e) => {
            if (e.target.checked) {
                $("#characteristicsNone").prop("checked", false);
            }
            this.toggleNone("#characteristicsNone", 'input[name="SelectedCharacteristicIds"]');
            this.validateSection1();
            this.clearSectionAlert($("#collapseCharacteristics"));
        });

        $(document).on("change.labor", "#characteristicsNone", () => {
            this.toggleNone("#characteristicsNone", 'input[name="SelectedCharacteristicIds"]');
            this.validateSection1();
            this.clearSectionAlert($("#collapseCharacteristics"));
        });

        $(document).on(
            "change.labor",
            'input[name="FetalPresentationAtBirthId"], input[name="FinalRouteAndMethodId"], input[name="TrialOfLaborBeforeCesarean"]',
            () => {
                this.validateSection2();
                this.clearSectionAlert($("#collapseMethod21"));
            }
        );

        $(document).on("change.labor", 'input[name="SelectedMaternalMorbidityIds"]', (e) => {
            if (e.target.checked) {
                $("#morbidityNone").prop("checked", false);
            }
            this.toggleNone("#morbidityNone", 'input[name="SelectedMaternalMorbidityIds"]');
            this.validateSection3();
            this.clearSectionAlert($("#collapseMorbidity"));
        });

        $(document).on("change.labor", "#morbidityNone", () => {
            this.toggleNone("#morbidityNone", 'input[name="SelectedMaternalMorbidityIds"]');
            this.validateSection3();
            this.clearSectionAlert($("#collapseMorbidity"));
        });
    }

    setupValidation() {
        this.validateSection1();
        this.validateSection2();
        this.validateSection3();
    }

    setupSaveHandler() {
        if (this.mode === "edit") {
            $(document)
                .off("click", "#labor-save-btn")
                .on("click", "#labor-save-btn", async (e) => {
                    e.preventDefault();
                    await this.saveLaborAndDelivery();
                });
        }
    }

    async saveSection() {
        return await this.saveLaborAndDelivery();
    }

    validateAll() {
        const s1 = this.validateSection1();
        const s2 = this.validateSection2();
        const s3 = this.validateSection3();
        return s1 && s2 && s3;
    }

    ensureAlertHost() {
        if (!$("#laborAlerts").length) {
            $('<div id="laborAlerts" class="mb-3"></div>')
                .prependTo("#laborAccordion")
                .css("margin-top", "-.25rem");
        }
    }

    showAlert(message, type = "info", timeoutMs = 5000) {
        this.ensureAlertHost();

        const $alert = $(`
            <div class="alert alert-${type} alert-dismissible fade show" role="alert">
                ${message}
                <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
        `);

        $("#laborAlerts").append($alert);

        if (timeoutMs) {
            setTimeout(() => $alert.alert("close"), timeoutMs);
        }
    }

    hydrateTooltips() {
        const $els = $('#laborAccordion [data-toggle="tooltip"]');
        $els.tooltip("dispose");

        $els.each(function () {
            const key = $(this).data("tkey");
            let text = "";

            if (key && window.laborHelp) {
                const [group, id] = key.split("_");
                if (group && id && window.laborHelp[group] && window.laborHelp[group][id]) {
                    text = window.laborHelp[group][id];
                }
            }

            if (!text) text = "No description available.";

            $(this).tooltip({
                title: text,
                trigger: "hover",
                container: "body",
                html: true,
                boundary: "window",
                template: `
                    <div class="tooltip bs-tooltip-auto" role="tooltip">
                        <div class="arrow"></div>
                        <div class="tooltip-inner bg-info text-white p-2 rounded shadow-sm"></div>
                    </div>`
            });
        });
    }

    parseIntOrNull(v) {
        if (v === undefined || v === null || v === "") return null;
        const n = Number(v);
        return Number.isFinite(n) ? n : null;
    }

    getAntiForgeryToken() {
        return $('input[name="__RequestVerificationToken"]').first().val()
            || $('input[name="__RequestVerificationToken"]').val()
            || $('input[name="__RequestVerificationToken"]').attr("value")
            || "";
    }

    findBirthId() {
        const fromManager = this.birthRegistryManager?.birthId || this.birthRegistryManager?.getBirthId?.();
        if (fromManager) return this.parseIntOrNull(fromManager);

        const fromHidden = this.parseIntOrNull(
            $("#HiddenBirthId").val()
            || $('[name="BirthId"]').val()
            || $("#BirthId").val()
        );
        if (fromHidden) return fromHidden;

        const fromContainer = this.parseIntOrNull(
            $("#laborAccordion").data("birthId")
            || $("#laborAccordion").attr("data-birth-id")
        );
        if (fromContainer) return fromContainer;

        const fromWindow = this.parseIntOrNull(window.BIRTH_ID || window.CurrentBirthId || window.CURRENT_BIRTH_ID);
        if (fromWindow) return fromWindow;

        return null;
    }

    findNewbornId() {
        return this.parseIntOrNull(
            $("#HiddenNewbornId").val()
            || $('[name="NewbornId"]').val()
            || $("#NewbornId").val()
        );
    }

    collectLaborPayload() {
        const selectedCharacteristics = $('input[name="SelectedCharacteristicIds"]:checked')
            .map((_, el) => this.parseIntOrNull($(el).val()))
            .get()
            .filter(v => v !== null);

        const selectedMorbidities = $('input[name="SelectedMaternalMorbidityIds"]:checked')
            .map((_, el) => this.parseIntOrNull($(el).val()))
            .get()
            .filter(v => v !== null);

        const fetalId = this.parseIntOrNull($('input[name="FetalPresentationAtBirthId"]:checked').val());
        const methodId = this.parseIntOrNull($('input[name="FinalRouteAndMethodId"]:checked').val());

        let trialOfLabor = null;
        const tolVal = $('input[name="TrialOfLaborBeforeCesarean"]:checked').val();
        if (tolVal === "true") trialOfLabor = true;
        else if (tolVal === "false") trialOfLabor = false;

        return {
            BirthId: this.findBirthId(),
            NewbornId: this.findNewbornId(),
            FetalPresentationAtBirthId: fetalId,
            FinalRouteAndMethodId: methodId,
            TrialOfLaborBeforeCesarean: trialOfLabor,
            Comments: ($("#laborComments").val() || "").toString().trim() || null,
            SelectedCharacteristicIds: selectedCharacteristics,
            SelectedMaternalMorbidityIds: selectedMorbidities,
            NoCharacteristics: $("#characteristicsNone").is(":checked"),
            NoMaternalMorbidities: $("#morbidityNone").is(":checked")
        };
    }

    sectionAlertFor($elInSection) {
        const $body = $elInSection.closest(".card-body");
        let $alert = $body.find(".section-alert").first();

        if (!$alert.length) {
            $alert = $('<div class="section-alert mb-2"></div>').prependTo($body);
        }

        return $alert;
    }

    showSectionAlert($el, message, type = "warning") {
        const $alert = this.sectionAlertFor($el);
        $alert.html(`
            <div class="alert alert-${type} alert-dismissible fade show" role="alert">
                ${message}
                <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
        `);
    }

    clearSectionAlert($el) {
        this.sectionAlertFor($el).empty();
    }

    setHeaderCheckForCollapse(collapseId, on) {
        $(collapseId)
            .closest(".card")
            .children(".card-header")
            .find(".accordion-checkmark")
            .toggleClass("d-none", !on);
    }

    validateSection1() {
        const any = $('input[name="SelectedCharacteristicIds"]:checked').length > 0;
        const none = $("#characteristicsNone").is(":checked");
        const valid = any || none;

        this.setHeaderCheckForCollapse("#collapseCharacteristics", valid);
        return valid;
    }

    validateSection2() {
        const has21 = $('input[name="FetalPresentationAtBirthId"]:checked').length > 0;
        const has22 = $('input[name="FinalRouteAndMethodId"]:checked').length > 0;
        const has23 = $('input[name="TrialOfLaborBeforeCesarean"]:checked').length > 0;

        $("#collapseMethod21").closest(".card").find(".accordion-checkmark").toggleClass("d-none", !has21);
        $("#collapseMethod22").closest(".card").find(".accordion-checkmark").toggleClass("d-none", !has22);
        $("#collapseMethod23").closest(".card").find(".accordion-checkmark").toggleClass("d-none", !has23);

        const fullValid = has21 && has22 && has23;
        this.setHeaderCheckForCollapse("#collapseMethod", fullValid);

        return fullValid;
    }

    validateSection3() {
        const any = $('input[name="SelectedMaternalMorbidityIds"]:checked').length > 0;
        const none = $("#morbidityNone").is(":checked");
        const valid = any || none;

        this.setHeaderCheckForCollapse("#collapseMorbidity", valid);
        return valid;
    }

    toggleNone(noneSelector, optionsSelector, container = "#laborAccordion") {
        const noneChecked = $(noneSelector).is(":checked");
        const $options = $(`${container} ${optionsSelector}`);

        if (noneChecked) {
            $options
                .prop("checked", false)
                .prop("disabled", true)
                .closest(".form-check")
                .addClass("option-disabled");
        } else {
            $options
                .prop("disabled", false)
                .closest(".form-check")
                .removeClass("option-disabled");
        }
    }

    async saveLaborAndDelivery({ silent = false } = {}) {
        const birthId = this.findBirthId();

        if (!birthId) {
            this.showAlert("Birth record not found. Please save Mother Information first.", "danger", 7000);
            return false;
        }

        const s1 = this.validateSection1();
        const s2 = this.validateSection2();
        const s3 = this.validateSection3();

        if (!(s1 && s2 && s3)) {
            if (!silent) {
                this.showAlert("Please complete all required Labor & Delivery sections before saving.", "warning");
            }

            if (!s1) {
                this.showSectionAlert($("#collapseCharacteristics"), 'Please select at least one characteristic or choose "None of the above".');
            }
            if (!s2) {
                this.showSectionAlert($("#collapseMethod21"), "Please complete all Method of Delivery questions.");
            }
            if (!s3) {
                this.showSectionAlert($("#collapseMorbidity"), 'Please select at least one maternal morbidity or choose "None of the above".');
            }

            return false;
        }

        const token = this.getAntiForgeryToken();
        if (!token) {
            this.showAlert("Security token missing. Please refresh the page and try again.", "danger", 8000);
            return false;
        }

        if (this.isSaving) return false;
        this.isSaving = true;

        try {
            if (!silent) {
                this.showAlert("Saving Labor & Delivery…", "info", 2000);
            }

            const payload = this.collectLaborPayload();

            const result = await $.ajax({
                url: this.saveEndpoint,
                type: "POST",
                data: JSON.stringify(payload),
                contentType: "application/json; charset=utf-8",
                headers: { RequestVerificationToken: token }
            });

            if (result && result.success) {
                this.showAlert(result.message || "Labor & Delivery saved.", "success", 3500);
                return true;
            }

            const message = (result && (result.message || result.error)) || "Failed to save Labor & Delivery.";
            this.showAlert(message, "danger", 8000);
            return false;
        } catch (err) {
            const serverMsg =
                (err && err.responseJSON && (err.responseJSON.message || err.responseJSON.error))
                || err.statusText
                || err.toString();

            this.showAlert(`Error saving Labor & Delivery: ${serverMsg}`, "danger", 10000);
            return false;
        } finally {
            this.isSaving = false;
        }
    }
}

window.LaborModule = LaborModule;

$(document).ready(function () {
    const mgr = window.birthRegistryManager || null;
    window.laborModule = new LaborModule("edit", mgr);
    window.laborModule.init();

    if (mgr && typeof mgr.registerModule === "function") {
        mgr.registerModule("labor", window.laborModule);
    }
});

$(document).on("labor-section-loaded", function () {
    const mgr = window.birthRegistryManager || null;
    window.laborModule = new LaborModule("edit", mgr);
    window.laborModule.init();
});