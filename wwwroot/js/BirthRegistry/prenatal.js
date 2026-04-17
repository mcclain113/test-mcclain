class PrenatalModule {
	constructor(mode = "edit", birthRegistryManager = null) {
		this.mode = mode;
		this.saveEndpoint = "/BirthRegistry/SavePrenatalData";
		this.birthRegistryManager = birthRegistryManager;
		this.initialized = false;
	}

	init() {
		if (!this.initialized) {
			this.setupEventHandlers();
			this.setupSaveHandler();
			this.initialized = true;
		}

		this.initializeConditionalFields();
	}

	/**
	 * Initializes conditional fields based on current checkbox states.
	 */
	initializeConditionalFields() {
		this._applyConditionalFields();
		const groups = [
			"prenatal1",
			"prenatal2",
			"prenatal3",
			"prenatal4",
			"prenatal5",
			"prenatal6",
			"prenatal7",
			"prenatal8",
		];
		groups.forEach((group) => this.validateGroup(group));
	}

	_applyConditionalFields() {
		const $noPrenatalCare = $("#no-prenatal-care");
		if ($noPrenatalCare.length && $noPrenatalCare.is(":checked")) {
			this._disablePrenatalFields();
		}

		const $noRiskFactors = $("#none-risk");
		if ($noRiskFactors.length && $noRiskFactors.is(":checked")) {
			this._disableRiskFields();
		}

		const $noInfections = $("#none-infection");
		if ($noInfections.length && $noInfections.is(":checked")) {
			this._disableInfectionFields();
		}

		const $noObProcedures = $("#none-ob");
		if ($noObProcedures.length && $noObProcedures.is(":checked")) {
			this._disableObFields();
		}

		const $previousCesarean = $("#PreviousCesareanDelivery");
		if ($previousCesarean.length) {
			this._toggleCesareanGroup($previousCesarean.is(":checked"));
		}

		const $ecv = $("#ExternalCephalicVersion");
		if ($ecv.length) {
			this._toggleEcvGroup($ecv.is(":checked"));
		}
	}

	setupEventHandlers() {
		this.setupMutuallyExclusiveHandlers();
		this.setupSpecialTriggerHandlers();
		this.setupValidationTriggers();
	}

	//#region EVENT HANDLERS AND TRIGGERS
	setupSaveHandler() {
		$(document)
			.off(
				"click",
				"#prenatal-content .next-tab-btn, #prenatal-content .save-tab-btn"
			)
			.on(
				"click",
				"#prenatal-content .next-tab-btn, #prenatal-content .save-tab-btn",
				async (e) => {
					e.preventDefault();
					await this.savePrenatalData();
				}
			);
	}

	async saveSection() {
		return await this.savePrenatalData();
	}

	setupMutuallyExclusiveHandlers() {
		// Handle "none" checkboxes
		$(document)
			.off("change", '#prenatal-content [data-mutually-exclusive="true"]')
			.on(
				"change",
				'#prenatal-content [data-mutually-exclusive="true"]',
				(e) => {
					const $target = $(e.target);
					const validationGroup = $target.data("validation-group");

					if ($target.is(":checked")) {
						this._handleNoneChecked($target.attr("id"));
					} else {
						this._handleNoneUnchecked($target.attr("id"));
					}

					this.validateGroup(validationGroup);
				}
			);

		// Handle individual checkboxes
		$(document)
			.off(
				"change",
				"#prenatal-content .risk-checkbox:not([data-mutually-exclusive]), #prenatal-content .infection-checkbox:not([data-mutually-exclusive]), #prenatal-content .ob-checkbox:not([data-mutually-exclusive])"
			)
			.on(
				"change",
				"#prenatal-content .risk-checkbox:not([data-mutually-exclusive]), #prenatal-content .infection-checkbox:not([data-mutually-exclusive]), #prenatal-content .ob-checkbox:not([data-mutually-exclusive])",
				(e) => {
					const $target = $(e.target);
					const validationGroup = $target.data("validation-group");

					if ($target.is(":checked")) {
						this._uncheckNoneForClass($target);
					}

					this.validateGroup(validationGroup);
				}
			);
	}

	setupSpecialTriggerHandlers() {
		// Cesarean trigger
		$(document)
			.off("change", '#prenatal-content [data-special-trigger="cesarean"]')
			.on(
				"change",
				'#prenatal-content [data-special-trigger="cesarean"]',
				(e) => {
					const isChecked = $(e.target).is(":checked");
					this._toggleCesareanGroup(isChecked);
					this.validateGroup("prenatal5");
				}
			);

		$(document)
			.off("change input", "#prenatal-content #NumberOfPriorCesareanBirths")
			.on(
				"change input",
				"#prenatal-content #NumberOfPriorCesareanBirths",
				() => {
					this.validateGroup("prenatal5");
				}
			);

		// ECV trigger
		$(document)
			.off("change", '#prenatal-content [data-special-trigger="ecv"]')
			.on("change", '#prenatal-content [data-special-trigger="ecv"]', (e) => {
				const isChecked = $(e.target).is(":checked");
				this._toggleEcvGroup(isChecked);
				this.validateGroup("prenatal7");
			});

		$(document)
			.off(
				"change",
				'#prenatal-content input[name="Prenatal.IsExternalCephalicVersionSuccessful"]'
			)
			.on(
				"change",
				'#prenatal-content input[name="Prenatal.IsExternalCephalicVersionSuccessful"]',
				() => {
					this.validateGroup("prenatal7");
				}
			);
	}

	setupValidationTriggers() {
		$(document)
			.off("change input", "#prenatal-content .validation-trigger")
			.on("change input", "#prenatal-content .validation-trigger", (e) => {
				const $target = $(e.target);
				const validationGroup = $target.data("validation-group");
				this.validateGroup(validationGroup);
			});
	}

	//#endregion

	//#region VALIDATION
	/**
	 * Validates a specific group.
	 * groupName - The name of the group to validate.
	 * RETURNS Whether the group is valid.
	 */
	validateGroup(groupName) {
		const $inputs = $(
			`#prenatal-content .validation-trigger[data-validation-group="${groupName}"]`
		);

		if (!$inputs.length) {
			return false;
		}

		const $nextBtn = $inputs
			.closest(".card-body")
			.find(".next-accordion-btn, .next-tab-btn, .save-tab-btn");
		const $card = $inputs.closest(".card");

		const isValid =
			this[
				`_validate${groupName.charAt(0).toUpperCase() + groupName.slice(1)}`
			]();

		$nextBtn.prop("disabled", !isValid);
		BirthRegistryUtils.updateCheckmark($card, isValid);

		return isValid;
	}

	// Validation methods for each group
	_validatePrenatal1() {
		const noCare = $("#no-prenatal-care").is(":checked");
		const hasDateAndVisits =
			$("#DateOfFirstVisit").val() && $("#TotalNumberPrenatalVisits").val();
		return noCare || hasDateAndVisits;
	}

	_validatePrenatal2() {
		const preWeight = $('input[name="Prenatal.PrepregnancyWeightInLbs"]').val();
		const deliveryWeight = $(
			'input[name="Prenatal.WeightAtDeliveryInLbs"]'
		).val();
		const height = $('input[name="Prenatal.MothersHeightInInches"]').val();
		return preWeight && deliveryWeight && height;
	}

	_validatePrenatal3() {
		return $('input[name="Prenatal.DateLastPeriod"]').val() !== "";
	}

	_validatePrenatal4() {
		const liveBirths = $(
			'input[name="Prenatal.PrevLiveBirthsAmountStillLiving"]'
		).val();
		const notLiving = $(
			'input[name="Prenatal.PrevLiveBirthsAmountNotLiving"]'
		).val();
		const otherOutcomes = $(
			'input[name="Prenatal.NumberOfOtherPregnancyOutcomes"]'
		).val();
		return liveBirths !== "" && notLiving !== "" && otherOutcomes !== "";
	}

	_validatePrenatal5() {
		const noneRiskChecked = $("#none-risk").is(":checked");
		const anyRiskChecked =
			$(".risk-checkbox:not(#none-risk):checked").length > 0;
		const cesareanChecked = $("#PreviousCesareanDelivery").is(":checked");
		const cesareanCount = $("#NumberOfPriorCesareanBirths").val();
		const cesareanValid =
			!cesareanChecked || (cesareanCount && parseInt(cesareanCount) >= 1);
		return (noneRiskChecked || anyRiskChecked) && cesareanValid;
	}

	_validatePrenatal6() {
		const noneInfectionChecked = $("#none-infection").is(":checked");
		const anyInfectionChecked =
			$(".infection-checkbox:not(#none-infection):checked").length > 0;
		return noneInfectionChecked || anyInfectionChecked;
	}

	_validatePrenatal7() {
		const noneObChecked = $("#none-ob").is(":checked");
		const anyObChecked = $(".ob-checkbox:not(#none-ob):checked").length > 0;
		const ecvChecked = $("#ExternalCephalicVersion").is(":checked");
		const ecvValid =
			!ecvChecked ||
			$('input[name="Prenatal.IsExternalCephalicVersionSuccessful"]:checked')
				.length > 0;
		return (noneObChecked || anyObChecked) && ecvValid;
	}

	_validatePrenatal8() {
		return $('input[name="Prenatal.IsWicRecipient"]:checked').length > 0;
	}

	//#endregion

	//#region HELPER METHODS
	_disablePrenatalFields() {
		$(
			"#DateOfFirstVisit, #DateOfLastVisit, #TotalNumberPrenatalVisits, #DescPrenatalCare"
		)
			.val("")
			.prop("disabled", true);
	}

	_disableRiskFields() {
		$(".risk-checkbox").prop("checked", false).prop("disabled", true);
		$("#prior-cesarean-group").addClass("d-none").hide();
		$("#NumberOfPriorCesareanBirths").val("");
	}

	_disableInfectionFields() {
		$(".infection-checkbox").prop("checked", false).prop("disabled", true);
	}

	_disableObFields() {
		$(".ob-checkbox").prop("checked", false).prop("disabled", true);
		$("#ecv-outcome-group").addClass("d-none").hide();
		$('input[name="Prenatal.IsExternalCephalicVersionSuccessful"]').prop(
			"checked",
			false
		);
	}

	_toggleCesareanGroup(isChecked) {
		if (isChecked) {
			$("#prior-cesarean-group").removeClass("d-none").show();
		} else {
			$("#prior-cesarean-group").addClass("d-none").hide();
			$("#NumberOfPriorCesareanBirths").val("");
		}
	}

	_toggleEcvGroup(isChecked) {
		if (isChecked) {
			$("#ecv-outcome-group").removeClass("d-none").show();
		} else {
			$("#ecv-outcome-group").addClass("d-none").hide();
			$('input[name="Prenatal.IsExternalCephalicVersionSuccessful"]').prop(
				"checked",
				false
			);
		}
	}

	_handleNoneChecked(id) {
		switch (id) {
			case "no-prenatal-care":
				this._disablePrenatalFields();
				break;
			case "none-risk":
				this._disableRiskFields();
				break;
			case "none-infection":
				this._disableInfectionFields();
				break;
			case "none-ob":
				this._disableObFields();
				break;
		}
	}

	_handleNoneUnchecked(id) {
		switch (id) {
			case "no-prenatal-care":
				$(
					"#DateOfFirstVisit, #DateOfLastVisit, #TotalNumberPrenatalVisits, #DescPrenatalCare"
				).prop("disabled", false);
				break;
			case "none-risk":
				$(".risk-checkbox").prop("disabled", false);
				break;
			case "none-infection":
				$(".infection-checkbox").prop("disabled", false);
				break;
			case "none-ob":
				$(".ob-checkbox").prop("disabled", false);
				break;
		}
	}

	_uncheckNoneForClass($target) {
		if ($target.hasClass("risk-checkbox")) {
			$("#none-risk").prop("checked", false);
		} else if ($target.hasClass("infection-checkbox")) {
			$("#none-infection").prop("checked", false);
		} else if ($target.hasClass("ob-checkbox")) {
			$("#none-ob").prop("checked", false);
		}
	}

	//#endregion

	//#region SAVE METHOD

	async savePrenatalData() {
		const birthId = this.birthRegistryManager?.birthId;

		if (!birthId) {
			BirthRegistryUtils.showAlert(
				"#prenatal-content",
				"Birth record not found. Please save mother information first.",
				"danger"
			);
			return false;
		}

		const saveFunction = () => {
			return new Promise((resolve, reject) => {
				const data = {
					BirthId: birthId,
					PrenatalId: $("#Prenatal_PrenatalId").val() || null,

					NoPrenatalCare: $("#no-prenatal-care").is(":checked"),
					DateOfFirstVisit:
						$('input[name="Prenatal.DateOfFirstVisit"]').val() || null,
					DateOfLastVisit:
						$('input[name="Prenatal.DateOfLastVisit"]').val() || null,
					TotalNumberPrenatalVisits:
						$('input[name="Prenatal.TotalNumberPrenatalVisits"]').val() || null,
					DescPrenatalCare:
						$('textarea[name="Prenatal.DescPrenatalCare"]').val() || null,

					MothersHeightInInches:
						$('input[name="Prenatal.MothersHeightInInches"]').val() || null,
					PrepregnancyWeightInLbs:
						$('input[name="Prenatal.PrepregnancyWeightInLbs"]').val() || null,
					WeightAtDeliveryInLbs:
						$('input[name="Prenatal.WeightAtDeliveryInLbs"]').val() || null,

					DateLastPeriod:
						$('input[name="Prenatal.DateLastPeriod"]').val() || null,

					PrevLiveBirthsAmountStillLiving:
						$('input[name="Prenatal.PrevLiveBirthsAmountStillLiving"]').val() ||
						null,
					PrevLiveBirthsAmountNotLiving:
						$('input[name="Prenatal.PrevLiveBirthsAmountNotLiving"]').val() ||
						null,
					NumberOfOtherPregnancyOutcomes:
						$('input[name="Prenatal.NumberOfOtherPregnancyOutcomes"]').val() ||
						null,
					DateOfLastLiveBirth:
						$('input[name="Prenatal.DateOfLastLiveBirth"]').val() || null,
					DateOfLastOtherPregnancyOutcome:
						$('input[name="Prenatal.DateOfLastOtherPregnancyOutcome"]').val() ||
						null,

					NoRiskFactors: $("#none-risk").is(":checked"),
					SelectedRiskFactorIds: $(".risk-checkbox:not(#none-risk):checked")
						.map((_, el) => parseInt($(el).val()))
						.get()
						.filter((id) => !isNaN(id)),

					InfertilityTreatmentReceived: $("#edit-infertility").is(":checked"),
					PreviousCesareanDelivery: $("#PreviousCesareanDelivery").is(
						":checked"
					),
					NumberOfPriorCesareanBirths:
						$('input[name="Prenatal.NumberOfPriorCesareanBirths"]').val() ||
						null,
					SmokingThreeMonthsBeforePregnancy: $(
						'input[name="Prenatal.SmokingThreeMonthsBeforePregnancy"]'
					).is(":checked"),
					SmokingFirstTrimester: $(
						'input[name="Prenatal.SmokingFirstTrimester"]'
					).is(":checked"),
					SmokingSecondTrimester: $(
						'input[name="Prenatal.SmokingSecondTrimester"]'
					).is(":checked"),
					SmokingThirdTrimester: $(
						'input[name="Prenatal.SmokingThirdTrimester"]'
					).is(":checked"),

					NoInfections: $("#none-infection").is(":checked"),
					SelectedInfectionIds: $(
						".infection-checkbox:not(#none-infection):checked"
					)
						.map((_, el) => parseInt($(el).val()))
						.get()
						.filter((id) => !isNaN(id)),

					NoObstetricProcedures: $("#none-ob").is(":checked"),
					ExternalCephalicVersion: $("#ExternalCephalicVersion").is(":checked"),
					IsExternalCephalicVersionSuccessful:
						$(
							'input[name="Prenatal.IsExternalCephalicVersionSuccessful"]:checked'
						).val() === "true"
							? true
							: $(
									'input[name="Prenatal.IsExternalCephalicVersionSuccessful"]:checked'
							  ).val() === "false"
							? false
							: null,
					CervicalCerclageProcedure: $(
						'input[name="Prenatal.CervicalCerclageProcedure"]'
					).is(":checked"),
					TocolysisProcedure: $('input[name="Prenatal.TocolysisProcedure"]').is(
						":checked"
					),
					DescInfertilityTreatment:
						$('textarea[name="Prenatal.DescInfertilityTreatment"]').val() ||
						null,

					IsWicRecipient:
						$('input[name="Prenatal.IsWicRecipient"]:checked').val() === "true"
							? true
							: $('input[name="Prenatal.IsWicRecipient"]:checked').val() ===
							  "false"
							? false
							: null,
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
				"Prenatal care information saved successfully",
				"success"
			);
		};

		const result = await BirthRegistryUtils.handleSaveWithFeedback(
			"#prenatal-content",
			saveFunction,
			onSuccess
		);

		return result.success;
	}

	//#endregion
}
