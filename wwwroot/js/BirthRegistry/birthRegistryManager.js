console.log("birthRegistryManager.js LOADED");


class BirthRegistryManager {
    constructor(mode = "edit", birthId, activeTab = "mother") {
        this.mode = mode;
        this.birthId = birthId;
        this.activeTab = activeTab;
        this.modules = new Map();
        this.preselectedMotherMrn = null;
        this.loadedTabs = new Set();
        this.validationStates = new Map();
        this.tabFlow = [
            "mother",
            "newborn", 
            "facility",
            "father",
            "prenatal",
            "labor",
            "finalize"
        ];
        console.log("TABFLOW:", this.tabFlow);

        this.init();
    }

    setBirthId(birthId) {
        this.birthId = birthId;
    }

    init() {
        this.setupTabNavigation();
        this.setupAccordionNavigation();
        this.setupModeSwitchHandlers();
        this.initializeActiveTabModule();
    }

    initializeActiveTabModule() {
        setTimeout(() => {
            const activeTabId = this.activeTab;
            const module = this.modules.get(activeTabId);
            
            if (module && typeof module.init === 'function') {
                module.init();
                this.loadedTabs.add(activeTabId);
            }
        }, 100);
    }

    setupModeSwitchHandlers() {
        $(document).on('click', '.mode-switch-btn', (e) => {
            e.preventDefault();
            
            const targetMode = $(e.currentTarget).data('target-mode');
            const currentTab = this.getCurrentActiveTab();
            
            const url = targetMode === 'edit' 
                ? `/BirthRegistry/Edit/${this.birthId}?tab=${currentTab}`
                : `/BirthRegistry/ViewBirthRegistry/${this.birthId}?tab=${currentTab}`;
            
            window.location.href = url;
        });
    }

    getCurrentActiveTab() {
        const activeTabHref = $('.nav-link.active').attr('href');
        return activeTabHref ? activeTabHref.replace('#', '') : 'mother';
    }

    updateUrlWithTab(tabName) {
        const url = new URL(window.location);
        url.searchParams.set('tab', tabName);
        window.history.replaceState({}, '', url);
    }


    setupTabNavigation() {
        $(".nav-link").on("shown.bs.tab", (e) => {
            const targetTab = $(e.target).attr("href").replace("#", "");
            this.activeTab = targetTab;
            
            this.updateUrlWithTab(targetTab);

            if (!this.loadedTabs.has(targetTab)) {
                const module = this.modules.get(targetTab);
                if (module && typeof module.init === 'function') {
                    module.init();
                    this.loadedTabs.add(targetTab);
                }
            }

            this.restoreValidationState(targetTab);
        });
    }

    setupAccordionNavigation() {
        $(document).on('click', '.next-accordion-btn', (e) => {
            if ($(e.target).closest('#facilityAccordion').length) {
                return;
            }
            e.preventDefault();

            const $btn = $(e.target);
            const targetCollapse = $btn.data("target-collapse");
            const currentCard = $btn.closest(".card");
            const currentCollapse = currentCard.find(".collapse.show");

            BirthRegistryUtils.updateCheckmark(currentCard, true);
            this.storeValidationState(currentCard);

            if (targetCollapse) {
                const targets = targetCollapse.split(",");
                let nextTarget = targets[0];

                if (targets.length > 1) {
                    for (let target of targets) {
                        if (
                            $(target.trim()).is(":visible") &&
                            $(target.trim()).closest(".card").is(":visible")
                        ) {
                            nextTarget = target.trim();
                            break;
                        }
                    }
                }

                currentCollapse.one("hidden.bs.collapse", function () {
                    $(nextTarget).collapse("show");
                });

                currentCollapse.collapse("hide");
            }
        });

        $(document).on("click", ".next-tab-btn", async (e) => {
            e.preventDefault();

            const currentTab = $(".nav-link.active").attr("href").replace("#", "");
            const module = this.modules.get(currentTab);

            if (module && typeof module.saveSection === 'function') {
                const success = await module.saveSection();
                if (!success) {
                    return;
                }
            }

            this.markCurrentTabComplete();

            const currentIndex = this.tabFlow.indexOf(currentTab);
            if (currentIndex < this.tabFlow.length - 1) {
                const nextTab = this.tabFlow[currentIndex + 1];
                $(`#${nextTab}-tab`).tab("show");
            }
        });

        $(document).on("click", ".save-tab-btn", async (e) => {
            e.preventDefault();

            const currentTab = $(".nav-link.active").attr("href").replace("#", "");
            const module = this.modules.get(currentTab);

            if (module && typeof module.saveSection === 'function') {
                await module.saveSection();
            }
        });
    }

    markCurrentTabComplete() {
        const activeTab = $(".tab-pane.active");
        activeTab.find(".card").each((index, card) => {
            const $card = $(card);
            const $nextBtn = $card.find(".next-accordion-btn, .next-tab-btn");

            if (!$nextBtn.prop("disabled")) {
                BirthRegistryUtils.updateCheckmark($card, true);
                this.storeValidationState($card);
            }
        });
    }

    storeValidationState($card) {
        const cardId = $card.attr("id") || $card.find("[id]").first().attr("id");
        if (cardId) {
            const isComplete = $card.find(".checkmark").hasClass("show");
            this.validationStates.set(cardId, isComplete);
        }
    }

    restoreValidationState(tabName) {
        $(`#${tabName}-content .card`).each((index, card) => {
            const $card = $(card);
            const cardId = $card.attr("id") || $card.find("[id]").first().attr("id");

            if (cardId && this.validationStates.get(cardId)) {
                BirthRegistryUtils.updateCheckmark($card, true);
            }
        });
    }

    registerModule(tabName, moduleInstance) {
        this.modules.set(tabName, moduleInstance);
        
        if (tabName === this.activeTab) {
            this.initializeActiveTabModule();
        }
    }
    
}