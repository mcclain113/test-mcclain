// this script listens for a selection in the EncounterType field (which is actually Patient Type) and modifies the contents of the PlaceOfService field dropdown list.  Currently set so if INPATIENT is selected, only Inpatient is in the dropdown.  If OUTPATIENT is selected, then Inpatient is NOT in the dropdown.
document.getElementById("EncounterTypeId").addEventListener("change", function () {
    var selectedEncounterType = this.value;
    
    fetch(`/Encounter/GetPlacesOfService?encounterTypeId=` + selectedEncounterType)
        .then(response => response.json())
        .then(data => {
            var placeOfServiceDropdown = document.getElementById("PlaceOfServiceId");
            placeOfServiceDropdown.innerHTML = "";

            var defaultOption = document.createElement("option");
            defaultOption.value = "";
            defaultOption.textContent = "Select Place of Service";
            placeOfServiceDropdown.appendChild(defaultOption);

            data.forEach(item => {
                var option = document.createElement("option");
                option.value = item.placeOfServiceId;
                option.textContent = item.description; // Ensure 'Description' holds actual text
                placeOfServiceDropdown.appendChild(option);
            });
        })
        .catch(error => console.error('Error fetching place of service:', error));
});

// this script handles all validation for the field Room Number and utilizes data in the site.js .validate as do all the other fields on the form.  All other fields in the <form> are validated only using jQuery.validate as set up in site.js
$(function () {
  const $roomNumber = $("#roomNumber");
  const $form = $("#encounter");
  const $submitBtn = $("#btnCreateEncounter");
  let pendingValidation = null;
  let touched = false;
  let submitAttempted = false;

  // Ensure we have a validator instance to use
  function getValidator() {
    return $form.data("validator") || $form.validate();
  }

  function debounce(fn, wait) {
    let t;
    return function (...args) {
      clearTimeout(t);
      t = setTimeout(() => fn.apply(this, args), wait);
    };
  }

  function readRoomValue() {
    // Try common inputmask APIs, then fallback to visible value
    try {
      if ($roomNumber.length && typeof $roomNumber.inputmask === "function") {
        const unmasked = $roomNumber.inputmask("unmaskedvalue");
        if (typeof unmasked === "string" && unmasked.trim() !== "") return unmasked.trim();
      }
    } catch (_) {}
    try {
      if ($roomNumber[0] && typeof $roomNumber[0].unmaskedvalue === "function") {
        const u = $roomNumber[0].unmaskedvalue();
        if (u != null) return u.toString().trim();
      }
    } catch (_) {}
    return ($roomNumber.val() ?? "").toString().trim();
  }

  function fieldName() {
    // Prefer the input's name attribute which jquery-validate uses
    return $roomNumber.attr("name") || $roomNumber.attr("id") || "roomNumber";
  }

  function setInvalid(message) {
    const validator = getValidator();
    const name = fieldName();
    const err = {};
    err[name] = message;
    validator.showErrors(err);
    // Ensure invalid element gets focusable state via validator
    // leave visual handling to jquery-validate
  }

  function clearFieldError() {
    const validator = getValidator();
    const name = fieldName();
    // showErrors with empty string clears the error for that field
    const clear = {};
    clear[name] = "";
    validator.showErrors(clear);
    // Also remove any aria-invalid attribute if present
    $roomNumber.removeAttr("aria-invalid");
  }

  function getEncounterId() {
    const v = $("#encounterId").val();
    const id = Number(v);
    return Number.isFinite(id) && id > 0 ? id : null;
  }

  async function remoteValidateRoomNumber(value) {
    try {
      const encounterId = getEncounterId();
      let url = `/Encounter/ValidateRoomNumber?roomNumber=${encodeURIComponent(value)}`;
      if (encounterId) url += `&encounterId=${encodeURIComponent(encounterId)}`;
      const resp = await fetch(url, { cache: "no-store" });
      if (!resp.ok) throw new Error("Network response not OK");
      const json = await resp.json();
      return { valid: Boolean(json.isValid) };
    } catch (err) {
      console.error("Error validating Room Number:", err);
      return { valid: false, error: true };
    }
  }

  // validateRoomNumber now treats empty as required.
  // showRequired controls whether the "required" message is shown immediately.
  async function validateRoomNumber({ showRequired = false } = {}) {
    const raw = readRoomValue();

    // Required check: empty is now invalid, but message shown only when showRequired === true
    if (raw === "") {
      if (showRequired) {
        setInvalid("Room number is required.");
      } else {
        clearFieldError();
      }
      return { valid: false, reason: "required" };
    }

    // Validate numeric format and range
    const n = parseInt(raw, 10);
    if (!Number.isInteger(n) || n.toString() !== raw) {
      setInvalid("Room number must be an integer of numbers only.");
      return { valid: false, reason: "format" };
    }
    if (n < 1 || n > 9999) {
      setInvalid("Room number must be between 1 and 9999 (inclusive).");
      return { valid: false, reason: "range" };
    }

    // Clear any prior client-side error before remote check so UI is consistent
    clearFieldError();

    // Server uniqueness check
    const result = await remoteValidateRoomNumber(raw);
    if (result.error) {
      setInvalid("Unable to validate room number right now. Please try again.");
      return { valid: false, reason: "network" };
    }
    if (!result.valid) {
      setInvalid("Room number already in use.");
      return { valid: false, reason: "duplicate" };
    }

    // Mark valid by ensuring no error is shown for this field
    clearFieldError();
    return { valid: true };
  }

  const onInput = debounce(async function () {
    pendingValidation = validateRoomNumber({ showRequired: touched || submitAttempted });
    await pendingValidation;
    pendingValidation = null;
  }, 300);

  $roomNumber.on("input", onInput);

  // mark touched on blur so required message appears after user leaves field
  $roomNumber.on("blur", function () {
    touched = true;

    if (pendingValidation) {
      pendingValidation.finally(async () => {
        pendingValidation = validateRoomNumber({ showRequired: touched || submitAttempted });
        await pendingValidation;
        pendingValidation = null;
      });
    } else {
      pendingValidation = validateRoomNumber({ showRequired: touched || submitAttempted });
      pendingValidation.finally(() => {
        pendingValidation = null;
      });
    }
  });

  $submitBtn.on("click", async function (e) {
    e.preventDefault();

    submitAttempted = true;

    // Use jquery-validate's API to check overall form validity first
    const validator = getValidator();
    const isFormLocallyValid = typeof validator.form === "function" ? validator.form() : true;
    if (!isFormLocallyValid) {
      // jquery-validate will have added errors; focus the first invalid element
      const $first = $form.find(".input-validation-error, .error:first");
      if ($first.length) $first.focus();
      return;
    }

    if (pendingValidation) {
      await pendingValidation;
      pendingValidation = null;
    }

    // On submit we want to show required immediately if empty
    const roomResult = await validateRoomNumber({ showRequired: true });
    if (!roomResult.valid) {
      $roomNumber.focus();
      return;
    }

    // Unbind any prevention and submit the form normally
    $form.off("submit.prevented");
    $form.submit();
  });
});