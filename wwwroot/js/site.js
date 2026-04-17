$(function () {
    $('[data-toggle="tooltip"]').tooltip();

    $.validator.addMethod("alphabetsnspace", function (value, element) {
        return this.optional(element) || /^[a-zA-Z\s ]+$/.test(value);
    }, "Only letters and spaces are allowed"); // specify it here just once instead of everywhere the function is applied unless you want to override this message then include it in the message section below

    // Check for DOB of less that today's date
    $.validator.addMethod("maxDate", function (value, element) {
        var curDate = new Date();
        var inputDate = new Date(value);
        if (inputDate < curDate)
            return true;
        return false;
    }, "Invalid Date from the future!");   // error message

    // Check for DOB of 100 years before this year
    $.validator.addMethod("minDate", function (value, element) {
        var testYear = (new Date).getFullYear();
        var testDate = new Date((testYear - 100), 1, 1);
        var inputDate = new Date(value);
        if (inputDate > testDate)
            return true;
        return false;
    }, "Too far in the past!");   // error message

    $.validator.addMethod("valueNotEquals", function (value, element, arg) {
        return arg !== value;
    }, "Value must not equal arg.");

    $.validator.setDefaults({ ignore: ":hidden:not(select)" })

    // CREATE or UPDATE PATIENT:  the <form> must be named 'patient', the jquery.validation must be loaded and the properties for which rules are specified must have an asp-for='Exactly as shown below' for this validation approach to function properly
    $("form[name='patient']").validate({
        // Allow validation of hidden values incase user closes accordion
        ignore: "",
        // Specify validation rules
        rules: {
            "Person.FirstName": {
                required: true,
                alphabetsnspace: true
            },
            "Person.MiddleName": "alphabetsnspace",
            "Person.LastName": {
                required: true,
                alphabetsnspace: true
            },
            "Person.MaidenName": "alphabetsnspace",
            "Person.MothersMaidenName": "alphabetsnspace",
            //MaritalStatusId: "required",
            //SexId: "required",
            //EthnicityId: "required",
            "Person.Dob": {
                //required: true,
                //date: true,
                //maxDate: true,
                //minDate: true
            },
            "Person.Ssn": {
                ///required: true,
            },
            "Patient.FacilityId": "required",
            "PrimaryInsurance.Guarantor": "alphabetsnspace",
            "SecondaryInsurance.Guarantor": "alphabetsnspace",
            "TertiaryInsurance.Guarantor": "alphabetsnspace",
        },
        // Specify validation error messages
        messages: {
            "Person.FirstName": {
                required: "Please provide a first name",
            },
            "Person.LastName": {
                required: "Please provide a last name",
            },
            "Person.Ssn": {
                //required: "Please provide a Social Security Number",
                // minlength: "Your SSN must be at least 11 digits long"
            },

            "Person.Dob": {
                //required: "Please provide a valid date of birth",
                //date: "Please provide a valid date of birth",
                //maxDate: "Invalid Date from the future!",
                //minDate: "Too far in the past!"
            },
            "Patient.FacilityId": {
                required: "Please select a Facility",
            },
            "PrimaryInsurance.Guarantor": {},
            "SecondaryInsurance.Guarantor": {},
            "TertiaryInsurance.Guarantor": {},
            "Person.MiddleName": {},
            "Person.MothersMaidenName": {},
            "Person.MaidenName": {},
            //MaritalStatusId: "Select a Marital Status from the list",
            //EthnicityId: "Select an Ethnicity from the list",
            //SexId: "Select a Sex from the list"

        },
        highlight: function (element, errorClass, validClass) {
            // Show the section with the error
            var $section = $(element).closest(".collapse");
            $section.collapse('show'); // Show the section with the error
        },
        // Make sure the form is submitted to the destination defined
        // in the "action" attribute of the form when valid
        submitHandler: function (form) {
            form.submit();
        }
    });

    var admitDateTime;
    $(document).ready(function () {
        admitDateTime = $("#admitDateTime").val();
    });
    $("#editEncounter").on("submit", function () {
        if ($("#admitDateTime").val() == '') {
            $("#admitDateTime").val(admitDateTime);
        }
    });

    // Add or EditEncounter:  the <form> must be named 'encounter' and the fields must have a 'name' attribute or 'asp-for=' which exactly matches these listed fields;  primarily used in the AddEncounter.cshtml because all fields are already populated in when the EditEncounter.cshtml is used and the User would need to physically clear fields prior to submittal for this validation to be needed.
    $("form[name='encounter']").validate({
        ignore: "", // prevents jQuery Validate from validating this/these field(s);  its validation must be included somewhere else, like encounter.js
        // Specify validation rules
        rules: {
            ChiefComplaint: "required",
            FacilityId: "required",
            RoomNumber: "required",
            DepartmentId: "required",
            PointOfOriginId: "required",
            PlaceOfServiceId: "required",
            AdmitTypeId: "required",
            EncounterPhysiciansId: "required",
            EncounterTypeId: "required",
            attendingProviderId: "required",
            admittingProviderId: "required"
        },
        // Specify validation error messages
        messages: {
            ChiefComplaint: "Please enter the patient's reason for coming to the hospital",
            FacilityId: "Select a Facility from the list",
            RoomNumber: "Please enter a Room Number",
            DepartmentId: "Select a Department from the list",
            PointOfOriginId: "Select a Point of Origin from the list",
            PlaceOfServiceId: "Select a Place of Service from the list",
            AdmitTypeId: "Select an Admit Type from the list",
            EncounterPhysiciansId: "Select a Physician from the list",
            EncounterTypeId: "Select a Patient Type from the list",
            attendingProviderId: "Select the Attending Provider from the list",
            admittingProviderId: "Select the Admitting Provider from the list"

        },
        // Make sure the form is submitted to the destination defined
        // in the "action" attribute of the form when valid
        submitHandler: function (form) {
            form.submit();
        }
    });

    // Calc age from DOB input in add/edit patient when leaving DOB field
    $('.dob').focusout(function () {
        let dob = $('.dob').val();

        if (dob !== '') {
            //Adding the time makes sure it saves the right date; otherwise, it may be off a day
            let birthDate = new Date(dob + 'T00:00:00');
            let today = new Date();

            let dayDiff = Math.ceil(today - birthDate) / (1000 * 60 * 60 * 24 * 365);
            let age = Math.floor(dayDiff);
            $('.age').text(age);

            // Format DOB in patient banner
            $('#calcDOB').text(birthDate.toLocaleDateString());

        }
    });

    // Update first, middle and last names in patient banner with those fields are edited in the Edit Page
    $('#FirstName').focusout(function () {
        var firstname = $('#FirstName').val();
        var middlename = $('#MiddleName').val();
        var lastname = $('#LastName').val();
        $('#fullName').text(firstname + " " + middlename + " " + lastname);
    });
    $('#MiddleName').focusout(function () {
        var firstname = $('#FirstName').val();
        var middlename = $('#MiddleName').val();
        var lastname = $('#LastName').val();
        $('#fullName').text(firstname + " " + middlename + " " + lastname);
    });
    $('#LastName').focusout(function () {
        var firstname = $('#FirstName').val();
        var middlename = $('#MiddleName').val();
        var lastname = $('#LastName').val();
        $('#fullName').text(firstname + " " + middlename + " " + lastname);
    });

    // Calc age in div in Details page
    $(function () {
        var dob = $('.dob').html();

        if (dob != '') {
            var DateCreated = new Date(Date.parse(dob));
            var today = new Date();

            var dayDiff = Math.ceil(today - DateCreated) / (1000 * 60 * 60 * 24 * 365);

            var age = parseInt(dayDiff);

            $('.age').text(age);
        }
    });

    $("#myInput").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#allergylist").filter(function () {
            $(this).toggle($(this).asp - items().toLowerCase().indexOf(value) > -1)
        });
    });

    $('.chosen').chosen({ width: '50%' });

    //========= PCA elements ===========
    // Set Pain values
    $('#cryingButtonGroup').on('click', 'button', function () {
        // Get element that was clicked
        $('#cryingValue').text(this.value);
    });
    // Lock/unlock selected pain assessment
    $('.painAssessment').on('click', function (e) {
        // First close all 
        $('.painAssessment').each(function () {
            $('#collapse' + $(this).attr("value")).removeClass('show');
        });
        // Now open the selected assessment
        $('#collapse' + this.value).addClass('show');
    });
    // PCA informational messages
    $('#PcaInfoModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget) // Button that triggered the modal
        var infoSection = button.data('whatever') // Extract info from data-* attributes
        // Update the modal's content. 
        // TODO: Is there a way to get this information from the database instead?
        var msg = ""
        var ttl = ""
        if (infoSection.substring(0, 5) == "cries") { ttl = "CRIES Scale" }
        else if (infoSection.substring(0, 9) == "nonVerbal") { ttl = "Non-Verbal Assessment" }
        if (infoSection == "WongBaker") {
            ttl = "The FACES Instructions for Usage";
            msg =
                "The FACES Scale Guidelines<br />" +
                "•	The FACES Scale is recommended for people ages three and older; it is not limited to children.<br/>" +
                "•	This self - assessment tool must be understood by the patient, so they are able to choose the face that best illustrates the physical pain they are experiencing. The tool is not for use with infants or patients who are unresponsive; other tools have been provided for this purpose.<br/>" +
                "•	It is a self - assessment tool and should not be used by a third person, parents, healthcare professionals, or caregivers, to assess the patient’s pain.There are other tools for those purposes.<br/><br/>" +
                "Explain to the person that each face represents a person who has no pain(hurt), or some, or a lot of pain. Face 0 doesn’t hurt at all. Face 2 hurts just a little bit. Face 4 hurts a little bit more. Face 6 hurts even more. Face 8 hurts a whole lot. Face 10 hurts as much as you can imagine although you don’t have to be crying to have this worst pain.<br /> <br />" +
                "Ask the person to choose the face that best depicts the pain they are experiencing.<br />"
        } else if (infoSection == "cries") {
            msg = "CRIES assesses crying, oxygenation, vital signs, facial expression, and sleeplessness. It is often used for infants six months old and younger and is widely used in the neonatal intensive care setting."
        } else if (infoSection == "criesCrying") {
            ttl = "Crying - Characteristic cry of pain is high pitched"
            msg = "0 - No cry or cry that is not high-pitched<br/>" +
                "1 - Cry high pitched but baby is easily consolable<br/>" +
                "2 - Cry high pitched and baby is inconsolable<br/>"
        } else if (infoSection == "criesO2") {
            ttl = "Requires O2 for SaO2 < 95% - Babies experiencing pain manifest decreased oxygenation. " +
                "Consider other causes of hypoxemia, e.g., oversedation, atelectasis, pneumothorax"
            msg = "0 - No oxygen required<br/>" +
                "1 - < 30% oxygen required<br/>" +
                "2 - > 30% oxygen required<br/>"
        } else if (infoSection == "criesVitalIncrease") {
            ttl = "Increased vital signs (BP and HR) - Take BP last as this may awaken child making other assessments difficult"
            msg = "0 - Both HR and BP unchanged or less than baseline<br/>" +
                "1 - HR or BP increased but increase is < 20% of baseline<br/>" +
                "2 - HR or BP is increased > 20% over baseline<br/>"
        } else if (infoSection == "criesExpression") {
            ttl = "Expression - The facial expression most often associated with pain is a grimace. " +
                "A grimace may be characterized by brow lowering, eyes squeezed shut, deepening naso-labial furrow, " +
                "or open lips and mouth."
            msg = "0 - No grimace present<br/>" +
                "1 - Grimace alone is present<br/>" +
                "2 - Grimace and non-cry vocalization grunt it present<br/>"
        } else if (infoSection == "criesSleepless") {
            ttl = "Sleepless - Scored based upon the infant's state during the hour preceding this recorded score."
            msg = "0 - Child has been continuously asleep<br/>" +
                "1 - Child has awakened at frequent intervals<br/>" +
                "2 - Child has been awake constantly<br/>"
        } else if (infoSection == "nonVerbal") {
            ttl = "Nonverbal Pain Assessment"
            msg = "Quantifies pain in patients unable to speak (due to intubation, dementia, etc). Each of 5 vitals has up to three fields: vital name, vital value, and vital description."
        } else if (infoSection == "nonVerbalFace") {
            ttl = "Face"
            msg = "0 - No particular expression or smile<br/>" +
                "1 - Occasional grimace, tearing, frowning, wrinkled forehead<br/>" +
                "2 - Frequent grimace, tearing, frowning, wrinkled forehead<br/>"
        } else if (infoSection == "nonVerbalMovement") {
            ttl = "Activity (movement)"
            msg = "0 - Lying quietly, normal position<br/>" +
                "1 - Seeking attention through movement or slow, cautious movement <br /> " +
                "2 - Restless, excessive activity and/or withdrawal reflexes<br/>"
        } else if (infoSection == "nonVerbalGuarding") {
            ttl = "Guarding"
            msg = "0 - Lying quietly, no positioning of hands over areas of the body<br/>" +
                "1 - Splinting areas of the body, tense<br/>" +
                "2 - Rigid, stiff<br/>"
        } else if (infoSection == "nonVerbalPhysiology") {
            ttl = "Physiology (vital signs)"
            msg = "0 - Baseline vital signs unchanged<br/>" +
                "1 - Change in SBP>20 mmHg or HR>20 bpm<br/>" +
                "2 - Change in SBP>30 mmHg or HR>25 bpm<br/>"
        } else if (infoSection == "nonVerbalRespiratory") {
            ttl = "Respiratory"
            msg = "0 - Baseline RR/SpO2 synchronous with ventilator<br/>" +
                "1 - RR>10 bpm over baseline, 5% decrease SpO2 or mild ventilator asynchrony<br/>" +
                "2 - RR>20 bpm over baseline, 10% decrease SpO2 or severe ventilator asynchrony<br/>"
        } else { msg = "Not found" }
        var modal = $(this)
        modal.find('.modal-title').html(ttl)
        modal.find('.modal-body p').html(msg)
    });

    $('.painScaleRadio').on('change', e => {
        $('.painScale').each((i, t) => {
            var txt = t.innerHTML.toString().toLowerCase();
            if (!t.classList.contains('d-none')) {
                t.classList.add('d-none');
            };
            if (txt.includes('input')) {
                var tag = t.getElementsByTagName('input');
                for (j = 0; j < tag.length; j++) {
                    if (tag[j].hasAttribute('required')) {
                        tag[j].removeAttribute('required');
                    };
                };
            };
        });
        $('.painScale-' + e.target.value).each((i, t) => {
            var txt = t.innerHTML.toString().toLowerCase();
            if (t.classList.contains('d-none')) {
                t.classList.remove('d-none');
            };
            if (txt.includes('input')) {
                var tag = t.getElementsByTagName('input');
                for (j = 0; j < tag.length; j++) {
                    tag[j].setAttribute('required', 'true');
                };
            };
        });

    });

    $(".extra-row").addClass("hidden-row");
    $("#hide-row-btn").on("click",
        e => {
            if (!$("#hide-row-btn").hasClass("extra-showing")) {
                $(".extra-row").removeClass("hidden-row");
                $("#hide-row-btn").addClass("extra-showing");
                $("#hide-row-btn").text("Hide Extra");
            } else {
                $(".extra-row").addClass("hidden-row");
                $("#hide-row-btn").removeClass("extra-showing");
                $("#hide-row-btn").text("Show More");
            }
        });

    //Require PCA Secondary Assessment description only when 'abnormal'
    //Not Assessed
    $(".cs-n").on("click", e => toggleRequired(e.target, false));
    //WNL
    $(".cs-t").on("click", e => toggleRequired(e.target, false));
    //Abnormal
    $(".cs-f").on("click", e => toggleRequired(e.target, true));
    $(() => {
        for (let ab = 0; ab < $(".cs-f").length; ab++) {
            if ($(".cs-f")[ab].checked)
                toggleRequired($(".cs-f")[ab], true);
        }
    });

    var pageNum = $("p:contains('Current Page')").text().substring(14, 16);
    $('a').filter(function (index) { return $(this).text() === pageNum; }).addClass("currentPage");

    function toggleRequired(target, isRequired) {
        const descriptionBoxId = target.id.substr(0, target.id.length - 1) + "d";
        const $clickedLabel = $(`[for=${target.id}]`);
        $($clickedLabel.parents(".row")[0]).find("label").removeClass("active");
        $clickedLabel.addClass("active");
        if (isRequired)
            $(`#${descriptionBoxId}`).attr("required", true).removeClass("d-none");
        else
            $(`#${descriptionBoxId}`).attr("required", false).addClass("d-none");
    }

    // function for applying a mask to inputing a US phone number
    // note that the extra characters may need stripped out by the controller depending upon
    // how the field is defined in the database
    $(".phone").inputmask({ "mask": "(999)-999-9999" });

    // function for applying a mask to inputing a US zip code or postal code
    // If other countries are truly desired, this mask may need to tie into the countryid
    $(".zipcode").inputmask({ "mask": "99999[-9999]" });

    // Run method on page load to add event listeners
    validateAddresses();

    // Address fields validation
    function validateAddresses() {
        // Checks for partial address and validates to user.
        $(".validate-address").on("input mouseup", function (e) {
            if ($(e.target).has(".validate")) {
                const $inputs = $(this).find("input.validate, select.validate")

                let anyFilled = false;
                $inputs.each(function (index, e) {
                    if (e.value.length != 0) {
                        anyFilled = true
                    }
                })

                if (anyFilled) {
                    $inputs.each(function (index, e) {
                        if (e.value.length == 0) {
                            $(e).not(".not-required").addClass("invalid");
                            $(e).not(".not-required").prop('required', true);
                            $(e).nextAll(".invalid-summary").first().show();
                        } else {
                            $(e).not(".not-required").removeClass("invalid");
                            $(e).prop('required', false);
                            $(e).nextAll(".invalid-summary").first().hide();
                        }
                    })
                } else {
                    $inputs.each(function (index, e) {
                        $(e).removeClass("invalid");
                        $(e).prop('required', false);
                        $(e).nextAll(".invalid-summary").first().hide();
                    })
                }
            }
        })
    }

    //Birth Registry Form Validation
    // custom validation for SSN
    $.validator.addMethod("ssn", function (value, element) {
        if (this.optional(element)) return true;
        // Remove any non-digits and check if it's exactly 9 digits
        const cleanSSN = value.replace(/\D/g, '');
        return cleanSSN.length === 9;
    }, "Please enter a valid 9-digit Social Security Number");

    $.validator.addMethod("postalCode", function (value, element) {
        if (this.optional(element)) return true;
        // Allow 5 digits or 5 digits + dash + 4 digits (ZIP or ZIP+4)
        return /^[0-9]{5}(-[0-9]{4})?$/.test(value);
    }, "Please enter a valid postal code (12345 or 12345-6789)");

    // weight (positive numbers only)
    $.validator.addMethod("positiveNumber", function (value, element) {
        return this.optional(element) || ($.isNumeric(value) && parseFloat(value) > 0);
    }, "Please enter a positive number");

    // APGAR scores (0-10)
    $.validator.addMethod("apgarScore", function (value, element) {
        if (this.optional(element)) return true;
        const score = parseInt(value, 10);
        return score >= 0 && score <= 10;
    }, "APGAR score must be between 0 and 10");

    // gestational age (realistic range)
    $.validator.addMethod("gestationalAge", function (value, element) {
        if (this.optional(element)) return true;
        const weeks = parseInt(value, 10);
        return weeks >= 20 && weeks <= 44;
    }, "Gestational age must be between 20 and 44 weeks");

    // phone numbers
    $.validator.addMethod("phoneUS", function (value, element) {
        if (this.optional(element)) return true;
        // Remove all non-digits
        const phone = value.replace(/\D/g, '');
        return phone.length === 10;
    }, "Please enter a valid 10-digit phone number");

    // birth weight (realistic range)
    $.validator.addMethod("birthWeight", function (value, element) {
        if (this.optional(element)) return true;
        const weight = parseFloat(value);
        // Typical range: 1-15 lbs or 300-7000 grams
        const unit = $('input[name="weightUnit"]:checked').val();
        if (unit === 'lbs') {
            return weight >= 1 && weight <= 15;
        } else if (unit === 'grams') {
            return weight >= 300 && weight <= 7000;
        }
        return true;
    }, "Please enter a realistic birth weight");

    // Birth Registry form validation setup
    $("form[name='birthRegistry']").validate({
        ignore: "", // Don't ignore hidden fields since accordions may be collapsed

        // Input validation rules
        rules: {

            //Facility Information Rules 
            "Facility.PrintedName": {
                required: true,
                alphabetsnspace: true,
                minlength: 2,
                maxlength: 50
            },

            "Facility.PlaceOfBirth": {
                required: true
            },
            "Facility.NonFacilityAddressLine1": {
                required: function () {
                    return $('select[name="Facility.PlaceOfBirth"]').val() === 'NonFacility';
                },
                maxlength: 100
            },
            "Facility.BirthPlaceTypeId": {
                required: true
            },
            "Facility.IsPlannedHomeBirth": {
                required: function () {
                    const selectedText = $('select[name="Facility.BirthPlaceTypeId"] option:selected').text().toLowerCase();
                    return selectedText.includes('home');
                }
            },

            "Facility.IsMotherTransferred": {
                required: true
            },
            "Facility.FacilityTransferredFromName": {
                required: function () {
                    return $('input[name="Facility.IsMotherTransferred"]:checked').val() === 'true';
                },
                facilityName: true
            },

            "Facility.AttendantFirst": {
                required: true,
                alphabetsnspace: true,
                minlength: 2,
                maxlength: 50
            },
            "Facility.AttendantLast": {
                required: true,
                alphabetsnspace: true,
                minlength: 2,
                maxlength: 50
            },
            "Facility.AttendantNPI": {
                required: true,
                npi: true
            },
            "Facility.AttendantTitle": {
                required: true
            },

            "Facility.IsCertifierAttendant": {
                required: function () {
                    return !$('#isCertifierNotAttendant').is(':checked');
                }
            },
            "Facility.CertifierFirst": {
                required: function () {
                    return $('#isCertifierNotAttendant').is(':checked');
                },
                alphabetsnspace: true,
                minlength: 2,
                maxlength: 50
            },
            "Facility.CertifierLast": {
                required: function () {
                    return $('#isCertifierNotAttendant').is(':checked');
                },
                alphabetsnspace: true,
                minlength: 2,
                maxlength: 50
            },
            "Facility.CertifierNPI": {
                required: function () {
                    return $('#isCertifierNotAttendant').is(':checked');
                },
                npi: true
            },
            "Facility.CertifierTitle": {
                required: function () {
                    return $('#isCertifierNotAttendant').is(':checked');
                }
            },

            "Facility.CertifierSignature": {
                required: true,
                minlength: 2,
                maxlength: 100
            },
            "Facility.DateCertified": {
                required: true,
                date: true,
                maxDate: true
            },


            // Mother Information Rules
            "Mother.FirstName": {
                required: function () {
                    return $('#motherNotInDatabase').is(':checked') || $('#isExistingPatient').val() === 'true';
                },
                alphabetsnspace: true,
                minlength: 2,
                maxlength: 50
            },
            "Mother.MiddleName": {
                alphabetsnspace: true,
                maxlength: 50
            },
            "Mother.LastName": {
                required: function () {
                    return $('#motherNotInDatabase').is(':checked') || $('#isExistingPatient').val() === 'true';
                },
                alphabetsnspace: true,
                minlength: 2,
                maxlength: 50
            },
            "Mother.DateOfBirth": {
                required: function () {
                    return $('#motherNotInDatabase').is(':checked') || $('#isExistingPatient').val() === 'true';
                },
                date: true,
                maxDate: true,
                minDate: true
            },
            "Mother.SSN": {
                ssn: true
            },
            "Mother.BirthPlace": {
                alphabetsnspace: true,
                maxlength: 100
            },

            // Address validation
            "Mother.ResidentialAddress.Address1": {
                required: function () {
                    return $('#motherNotInDatabase').is(':checked') || $('#isExistingPatient').val() === 'true';
                },
                maxlength: 100
            },
            "Mother.ResidentialAddress.City": {
                required: function () {
                    return $('#motherNotInDatabase').is(':checked') || $('#isExistingPatient').val() === 'true';
                },
                alphabetsnspace: true,
                maxlength: 50
            },
            "Mother.ResidentialAddress.PostalCode": {
                required: function () {
                    return $('#motherNotInDatabase').is(':checked') || $('#isExistingPatient').val() === 'true';
                },
                postalCode: true,
                minlength: 5,
                maxlength: 10
            },
            "Mother.ResidentialAddress.AddressStateID": {
                required: function () {
                    return $('#motherNotInDatabase').is(':checked') || $('#isExistingPatient').val() === 'true';
                }
            },

            // Father Information Rules (conditional based on paternity)
            "Father.FirstName": {
                required: function () {
                    return $("input[name='Father.HasPaternityAcknowledgement']:checked").val() === "true";
                },
                alphabetsnspace: true,
                minlength: 2,
                maxlength: 50
            },
            "Father.LastName": {
                required: function () {
                    return $("input[name='Father.HasPaternityAcknowledgement']:checked").val() === "true";
                },
                alphabetsnspace: true,
                minlength: 2,
                maxlength: 50
            },
            "Father.DateOfBirth": {
                required: function () {
                    return $("input[name='Father.HasPaternityAcknowledgement']:checked").val() === "true";
                },
                date: true,
                maxDate: true,
                minDate: true
            },
            "Father.SSN": {
                ssn: true
            },

            // Prenatal Care Rules
            "Prenatal.DateOfFirstVisit": {
                required: function () {
                    return !$('#no-prenatal-care').is(':checked');
                },
                date: true,
                maxDate: true  // No future dates allowed
            },
            "Prenatal.DateOfLastVisit": {
                date: true,
                maxDate: true
            },
            "Prenatal.TotalNumberPrenatalVisits": {
                required: function () {
                    return !$('#no-prenatal-care').is(':checked');
                },
                digits: true,
                min: 0,
                max: 50
            },
            "Prenatal.PrepregnancyWeightInLbs": {
                required: true,
                positiveNumber: true,
                max: 1000
            },
            "Prenatal.WeightAtDeliveryInLbs": {
                required: true,
                positiveNumber: true,
                max: 1100
            },
            "Prenatal.DateLastPeriod": {
                required: true,
                date: true,
                maxDate: true
            },
            "Prenatal.LiveBirthsAmountStillLiving": {
                required: true,
                digits: true,
                min: 0,
                max: 20
            },
            "Prenatal.NumberOfOtherPregnancyOutcomes": {
                required: true,
                digits: true,
                min: 0,
                max: 20
            },
            "Prenatal.NumberOfPriorCesareanBirths": {
                required: function () {
                    return $('#PreviousCesareanDelivery').is(':checked');
                },
                digits: true,
                min: 1,
                max: 10
            },
            "Prenatal.IsWicRecipient": {
                required: true
            },

            // Newborn Information Rules
            "Newborn.DateAndTimeOfBirth": {
                required: function () {
                    return !$('#nbTimeUnknown').is(':checked');
                },
                date: true
            },
            "Newborn.BirthWeightInLbs": {
                required: true,
                positiveNumber: true,
                min: 1,
                max: 15,
            },
            "Newborn.GestationalAgeEstimateInWeeks": {
                required: true,
                gestationalAge: true
            },
            "Newborn.SexId": {
                required: true
            },
            "Newborn.ApgarScoreAt5Minutes": {
                required: true,
                apgarScore: true
            },
            "Newborn.ApgarScoreAt10Minutes": {
                required: function () {
                    const apgar5 = parseInt($('#nbApgar5').val(), 10);
                    return apgar5 < 6;
                },
                apgarScore: true
            },
            "Newborn.Plurality": {
                required: function () {
                    return !$('#nbSingle').is(':checked');
                },
                digits: true,
                min: 2,
                max: 8
            },
            "Newborn.BirthOrder": {
                required: function () {
                    return !$('#nbSingle').is(':checked');
                },
                digits: true,
                min: 1
            },
            "Newborn.InfantTransferredWithin24Hours": {
                required: true
            },
            "Newborn.NameOfFacilityTransferredTo": {
                required: function () {
                    return $('#nbXferYes').is(':checked');
                },
                maxlength: 100
            },
            "Newborn.IsInfantStillLiving": {
                required: true
            },
            "Newborn.IsInfantBeingBreastfed": {
                required: true
            },
            "Newborn.SelectedAbnormalConditionIds": {
                required: function () {
                    return !$('#nbNoAbnl').is(':checked');
                }
            },
            "Newborn.SelectedCongenitalAnomalyIds": {
                required: function () {
                    return !$('#nbNoAnom').is(':checked');
                }
            },
            "Finalize.RegistrarSignature": {
                required: true,
                minlength: 2,
                maxlength: 100,
                noDigitsOnly: true
            },
            "Finalize.DateOfRegistrarSignature": {
                required: true,
                date: true,
                maxDate: true
            }
        },

        // Validation error messages
        messages: {

            //Facility Information Messages
            "Facility.PrintedName": {
                required: "Please enter your name",
                minlength: "Name must be at least 2 characters",
                maxlength: "Name cannot exceed 50 characters"
            },
            "Facility.PlaceOfBirth": "Please select the place of birth",
            "Facility.NonFacilityAddressLine1": {
                required: "Please enter the non-facility birth address",
                maxlength: "Address cannot exceed 100 characters"
            },
            "Facility.BirthPlaceTypeId": "Please select the type of place where birth occurred",
            "Facility.IsPlannedHomeBirth": "Please indicate if the home birth was planned",
            "Facility.IsMotherTransferred": "Please indicate if the mother was transferred",
            "Facility.FacilityTransferredFromName": {
                required: "Please enter the name of facility mother was transferred from",
                facilityName: "Facility name must be between 2 and 100 characters"
            },
            "Facility.AttendantFirst": {
                required: "Please enter the attendant's first name",
                minlength: "First name must be at least 2 characters",
                maxlength: "First name cannot exceed 50 characters"
            },
            "Facility.AttendantLast": {
                required: "Please enter the attendant's last name",
                minlength: "Last name must be at least 2 characters",
                maxlength: "Last name cannot exceed 50 characters"
            },
            "Facility.AttendantNPI": {
                required: "Please enter the attendant's NPI",
                npi: "Please enter a valid 10-digit NPI"
            },
            "Facility.AttendantTitle": "Please select the attendant's title",
            "Facility.IsCertifierAttendant": "Please indicate if certifier is the attendant",
            "Facility.CertifierFirst": {
                required: "Please enter the certifier's first name",
                minlength: "First name must be at least 2 characters",
                maxlength: "First name cannot exceed 50 characters"
            },
            "Facility.CertifierLast": {
                required: "Please enter the certifier's last name",
                minlength: "Last name must be at least 2 characters",
                maxlength: "Last name cannot exceed 50 characters"
            },
            "Facility.CertifierNPI": {
                required: "Please enter the certifier's NPI",
                npi: "Please enter a valid 10-digit NPI"
            },
            "Facility.CertifierTitle": "Please select the certifier's title",
            "Facility.CertifierSignature": {
                required: "Please provide the certifier's signature",
                minlength: "Signature must be at least 2 characters",
                maxlength: "Signature cannot exceed 100 characters"
            },
            "Facility.DateCertified": {
                required: "Please enter the date certified",
                date: "Please enter a valid date",
                maxDate: "Date certified cannot be in the future"
            },
            // Mother Information Messages (keep as-is)
            "Mother.FirstName": {
                required: "Please enter the mother's first name",
                minlength: "First name must be at least 2 characters",
                maxlength: "First name cannot exceed 50 characters"
            },
            "Mother.LastName": {
                required: "Please enter the mother's last name",
                minlength: "Last name must be at least 2 characters",
                maxlength: "Last name cannot exceed 50 characters"
            },
            "Mother.DateOfBirth": {
                required: "Please enter the mother's date of birth",
                date: "Please enter a valid date"
            },
            "Mother.SSN": "Please enter a valid 9-digit Social Security Number",
            "Mother.ResidentialAddress.Address1": "Please enter a street address",
            "Mother.ResidentialAddress.City": "Please enter a city name",
            "Mother.ResidentialAddress.PostalCode": {
                required: "Please enter a postal code",
                postalCode: "Please enter a valid postal code (numbers only)",
                minlength: "Postal code must be at least 5 digits",
                maxlength: "Postal code cannot exceed 10 characters"
            },
            "Mother.ResidentialAddress.AddressStateID": "Please select a state",

            // Father Information Messages
            "Father.FirstName": {
                required: "Please enter the father's first name when paternity is acknowledged",
                minlength: "First name must be at least 2 characters"
            },
            "Father.LastName": {
                required: "Please enter the father's last name when paternity is acknowledged",
                minlength: "Last name must be at least 2 characters"
            },
            "Father.DateOfBirth": {
                required: "Please enter the father's date of birth when paternity is acknowledged",
                date: "Please enter a valid date"
            },

            // Prenatal Care Messages
            "Prenatal.DateOfFirstVisit": "Please enter the date of first prenatal visit",
            "Prenatal.DateOfLastVisit": "Please enter the date of last prenatal visit",
            "Prenatal.TotalNumberPrenatalVisits": {
                required: "Please enter the total number of prenatal visits",
                digits: "Please enter a whole number",
                min: "Number cannot be negative",
                max: "Number of visits seems high (over 50)"
            },
            "Prenatal.MothersHeightInInches": {
                positiveNumber: "Please enter a valid height",
                min: "Height seems too low (under 36 inches)",
                max: "Height seems too high (over 84 inches)"
            },
            "Prenatal.PrepregnancyWeightInLbs": {
                required: "Please enter the mother's pre-pregnancy weight",
                positiveNumber: "Weight must be a positive number",
                max: "Weight seems high"
            },
            "Prenatal.WeightAtDeliveryInLbs": {
                required: "Please enter the mother's weight at delivery",
                positiveNumber: "Weight must be a positive number",
                max: "Weight seems high"
            },
            "Prenatal.DateLastPeriod": {
                required: "Please enter the date of last menstrual period",
                date: "Please enter a valid date",
                maxDate: "Date cannot be in the future"
            },
            "Prenatal.PrevLiveBirthsAmountStillLiving": {
                required: "Please enter the number of previous live births still living",
                digits: "Please enter a whole number",
                min: "Number cannot be negative",
                max: "Number seems high"
            },
            "Prenatal.PrevLiveBirthsAmountNotLiving": {
                required: "Please enter the number of previous live births not living",
                digits: "Please enter a whole number",
                min: "Number cannot be negative",
                max: "Number seems high"
            },
            "Prenatal.NumberOfOtherPregnancyOutcomes": {
                required: "Please enter the number of other pregnancy outcomes",
                digits: "Please enter a whole number",
                min: "Number cannot be negative",
                max: "Number seems high"
            },
            "Prenatal.NumberOfPriorCesareanBirths": {
                required: "Please enter the number of prior cesarean births",
                digits: "Please enter a whole number",
                min: "Must be at least 1 if cesarean delivery was selected",
                max: "Number seems high"
            },
            "Prenatal.SelectedRiskFactorIds": "Please select applicable risk factors or check 'None of the above'",
            "Prenatal.SelectedInfectionIds": "Please select applicable infections or check 'None of the above'",
            "Prenatal.SelectedObstetricProcedureIds": "Please select applicable procedures or check 'None of the above'",
            "Prenatal.IsWicRecipient": "Please indicate WIC recipient status",

            // Newborn Information Messages
            "Newborn.DateAndTimeOfBirth": {
                required: "Please enter the birth date and time (or check 'Time unknown')",
                date: "Please enter a valid date and time"
            },
            "Newborn.IsTimeUnknown": "Please check if time is unknown",
            "Newborn.BirthWeightInLbs": {
                required: "Please enter the infant's birth weight",
                positiveNumber: "Birth weight must be a positive number",
                min: "Birth weight is too low",
                max: "Birth weight is too high"
            },
            "Newborn.GestationalAgeEstimateInWeeks": {
                required: "Please enter gestational age in weeks",
                gestationalAge: "Gestational age must be between 20 and 44 weeks"
            },
            "Newborn.SexId": "Please select the infant's sex",
            "Newborn.ApgarScoreAt1Minute": {
                apgarScore: "1-minute APGAR score must be between 0 and 10"
            },
            "Newborn.ApgarScoreAt5Minutes": {
                required: "Please enter the 5-minute APGAR score",
                apgarScore: "5-minute APGAR score must be between 0 and 10"
            },
            "Newborn.ApgarScoreAt10Minutes": {
                required: "Please enter the 10-minute APGAR score (required when 5-minute score is less than 6)",
                apgarScore: "10-minute APGAR score must be between 0 and 10"
            },
            "Newborn.IsSingleBirth": "Please indicate if this is a single birth",
            "Newborn.Plurality": {
                required: "Please enter the number of babies born (when not single birth)",
                digits: "Please enter a whole number",
                min: "Plurality must be at least 2 for multiple births",
                max: "Number is too high"
            },
            "Newborn.BirthOrder": {
                required: "Please enter the birth order for this baby",
                digits: "Please enter a whole number",
                min: "Birth order must be at least 1"
            },
            "Newborn.InfantTransferredWithin24Hours": {
                required: "Please indicate if infant was transferred within 24 hours"
            },
            "Newborn.NameOfFacilityTransferredTo": {
                required: "Please enter the name of the facility infant was transferred to",
                maxlength: "Facility name is too long (maximum 100 characters)"
            },
            "Newborn.IsInfantStillLiving": {
                required: "Please indicate if infant is living at time of report"
            },
            "Newborn.IsInfantBeingBreastfed": {
                required: "Please indicate if infant is being breastfed at discharge"
            },
            "Newborn.SelectedAbnormalConditionIds": {
                required: "Please select abnormal conditions or check 'No abnormalities identified'"
            },
            "Newborn.SelectedCongenitalAnomalyIds": {
                required: "Please select congenital anomalies or check 'No congenital anomalies identified'"
            },
            "Finalize.RegistrarSignature": {
                required: "Please provide your signature to finalize this birth record",
                minlength: "Signature must be at least 2 characters",
                maxlength: "Signature cannot exceed 100 characters",
                noDigitsOnly: "Signature cannot contain only numbers"
            },
            "Finalize.DateOfRegistrarSignature": {
                required: "Please enter the date of signature",
                date: "Please enter a valid date",
                maxDate: "Date cannot be in the future"
            }
        },

        // Highlight errors by showing the accordion section
        highlight: function (element, errorClass, validClass) {
            // Show the section with the error
            var $section = $(element).closest(".collapse");
            if ($section.length) {
                $section.collapse('show');
            }

            // Add Bootstrap invalid class
            $(element).addClass('is-invalid').removeClass('is-valid');
        },

        // Remove error styling when valid
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass('is-invalid').addClass('is-valid');
        },

        // Custom error placement
        errorPlacement: function (error, element) {
            error.addClass('invalid-feedback d-block');

            // Place error after the form group
            var $formGroup = element.closest('.form-group');
            if ($formGroup.length) {
                $formGroup.append(error);
            } else {
                error.insertAfter(element);
            }
        },

        submitHandler: function (form) {
            // Let birthRegistryForm.js handle the actual submission logic
            // This just ensures input validation passes
            form.submit();
        }
    });

    // scripts used in the PatientBannerPartial for Directives button
    // 1) load the directives list for a given MRN
    function loadDirectivesList(mrn) {
        var $modal = $('#directivesModal');
        var $body = $modal.find('.modal-body');
        var $footer = $modal.find('.modal-footer');

        // hide Back button
        $footer.find('#backToListBtn').hide();

        // show spinner
        $body.html(
            '<div class="text-center py-5">' +
            '<i class="fas fa-spinner fa-pulse"></i> Loading…' +
            '</div>'
        );

        // load the list partial (no more bindDirectiveClicks)
        $body.load('/Patient/ListDirectives/' + mrn);
    }

    // 2) On modal show, stash MRN & load list
    $('#directivesModal').on('show.bs.modal', function (e) {
        var mrn = $(e.relatedTarget).data('mrn');
        $(this).data('mrn', mrn);
        loadDirectivesList(mrn);
    });

    // 3) Delegate click on any row (works for dynamically-loaded rows)
    $('#directivesModal').on('click', '.directive-row', function () {
        var docId = $(this).data('docid');
        var url = '/Patient/ViewDirective?docId=' + docId;
        var $modal = $('#directivesModal');
        var $body = $modal.find('.modal-body');
        var $footer = $modal.find('.modal-footer');

        console.log('Embedding PDF from', url);

        // inject the PDF viewer
        $body.html(`
        <object
        data="${url}"
        type="application/pdf"
        width="100%"
        height="600px">
        <p>Your browser does not support PDFs.
            <a href="${url}" target="_blank">Download</a>.
        </p>
        </object>
        `);

        // now show the Back to List button in the footer
        $footer.find('#backToListBtn').show();
    });

    // 4) Back to List
    $('#directivesModal').on('click', '#backToListBtn', function () {
        var mrn = $('#directivesModal').data('mrn');
        loadDirectivesList(mrn);
    });

    // control the Directives button enabled/disabled  
    // find every Directives button on the page
    $('.btn[data-target="#directivesModal"]').each(function () {
        var $btn = $(this);
        var mrn = $btn.data('mrn');
        if (!mrn) return;

        // query the new endpoint
        $.getJSON('/Patient/HasDirectives', { mrn: mrn })
            .done(function (has) {
                if (!has) {
                    $btn.prop('disabled', true)
                        .attr('title', 'No advanced directives available');
                }
            })
            .fail(function () {
                // in case of error, optionally leave it enabled or disable
                console.warn('Could not determine directives for MRN:', mrn);
            });
    });

    // this function calculates the available view screen for the _EncounterMenu and works with css in that file to provide the correct height and vertical scrollbar
    (function () {
        var MENU_WRAPPER_SELECTOR = '.encounter-menu-wrapper'; // update to your actual selector
        var HEADER_SELECTOR = '.navbar'; // header/nav element(s)
        var BANNER_SELECTOR = '.patient-banner'; //  banner element(s)
        var FOOTER_SELECTOR = 'footer'; // footer element
        var root = document.documentElement;

        function px(n) { return n + 'px'; }

        function computeOffsets() {
            var header = document.querySelector(HEADER_SELECTOR);
            var banner = document.querySelector(BANNER_SELECTOR);
            var footer = document.querySelector(FOOTER_SELECTOR);

            var headerH = header ? header.getBoundingClientRect().height : 0;
            var bannerH = banner ? banner.getBoundingClientRect().height : 0;
            var footerH = footer ? footer.getBoundingClientRect().height : 0;

            // Add small buffer to avoid overlap (optional)
            var buffer = 8;

            var total = Math.ceil(headerH + bannerH + footerH + buffer);
            root.style.setProperty('--encounter-menu-vertical-offset', px(total));
        }

        // Run on load and on resize
        window.addEventListener('load', computeOffsets);
        window.addEventListener('resize', computeOffsets);

        // If your page changes banner/footer height dynamically (e.g., after AJAX),
        // call computeOffsets() again after that change. Example: after global modal closes.
        // You can also observe DOM changes:
        if ('MutationObserver' in window) {
            var observerTargets = [];
            var h = document.querySelector(HEADER_SELECTOR);
            if (h) observerTargets.push(h);
            var b = document.querySelector(BANNER_SELECTOR);
            if (b) observerTargets.push(b);
            var f = document.querySelector(FOOTER_SELECTOR);
            if (f) observerTargets.push(f);

            if (observerTargets.length) {
                var mo = new MutationObserver(function () { computeOffsets(); });
                observerTargets.forEach(function (el) { mo.observe(el, { childList: true, subtree: true, attributes: true }); });
            }
        }

        // If you need to refresh immediately from code, expose a helper:
        window.refreshEncounterMenuOffsets = computeOffsets;
    })();

});

//#region Validation
//TODO there has to be a better way to do this??!?!?//

function Validation() {

    $(function () {
        // ## FACE ##

        if ($('input[id*= "Faces"]').is(':checked')) {
            $('label[for*= "Faces"]').removeClass();
            $('label[for*= "Faces"]').addClass('btn btn-outline-primary w-90 mb-0');
        } else {
            $('label[for*= "Faces"]').removeClass();
            $('label[for*= "Faces"]').addClass('btn btn-outline-danger w-90 mb-0');
        }
        // ## CRIES ##

        if ($('input[id*= "Crying"]').is(':checked')) {
            $('label[for*= "Crying"]').removeClass();
            $('label[for*= "Crying"]').addClass('btn btn-outline-primary w-90 mb-0');

        } else {
            $('label[for*= "Crying"]').removeClass();
            $('label[for*= "Crying"]').addClass('btn btn-outline-danger w-90 mb-0');
        }
        if ($('input[id*= "Requires"]').is(':checked')) {
            $('label[for*= "Requires"]').removeClass();
            $('label[for*= "Requires"]').addClass('btn btn-outline-primary w-90 mb-0');

        } else {
            $('label[for*= "Requires"]').removeClass();
            $('label[for*= "Requires"]').addClass('btn btn-outline-danger w-90 mb-0');
        }
        if ($('input[id*= "Increased"]').is(':checked')) {
            $('label[for*= "Increased"]').removeClass();
            $('label[for*= "Increased"]').addClass('btn btn-outline-primary w-90 mb-0');

        } else {
            $('label[for*= "Increased"]').removeClass();
            $('label[for*= "Increased"]').addClass('btn btn-outline-danger w-90 mb-0');
        }
        if ($('input[id*= "Expression"]').is(':checked')) {
            $('label[for*= "Expression"]').removeClass();
            $('label[for*= "Expression"]').addClass('btn btn-outline-primary w-90 mb-0');

        } else {
            $('label[for*= "Expression"]').removeClass();
            $('label[for*= "Expression"]').addClass('btn btn-outline-danger w-90 mb-0');
        }
        if ($('input[id*= "Sleepless"]').is(':checked')) {
            $('label[for*= "Sleepless"]').removeClass();
            $('label[for*= "Sleepless"]').addClass('btn btn-outline-primary w-90 mb-0');

        } else {
            $('label[for*= "Sleepless"]').removeClass();
            $('label[for*= "Sleepless"]').addClass('btn btn-outline-danger w-90 mb-0');
        }
        // ## NONVERBAL ##

        if ($('input[id*= "e1"]').is(':checked')) {
            $('label[for*= "e1"]').removeClass();
            $('label[for*= "e1"]').addClass('btn btn-outline-primary w-90 mb-0');

        } else {
            $('label[for*= "e1"]').removeClass();
            $('label[for*= "e1"]').addClass('btn btn-outline-danger w-90 mb-0');


        }
        if ($('input[id*= "Activity"]').is(':checked')) {
            $('label[for*= "Activity"]').removeClass();
            $('label[for*= "Activity"]').addClass('btn btn-outline-primary w-90 mb-0');

        } else {
            $('label[for*= "Activity"]').removeClass();
            $('label[for*= "Activity"]').addClass('btn btn-outline-danger w-90 mb-0');


        }
        if ($('input[id*= "Guarding"]').is(':checked')) {
            $('label[for*= "Guarding"]').removeClass();
            $('label[for*= "Guarding"]').addClass('btn btn-outline-primary w-90 mb-0');

        } else {
            $('label[for*= "Guarding"]').removeClass();
            $('label[for*= "Guarding"]').addClass('btn btn-outline-danger w-90 mb-0');


        }
        if ($('input[id*= "Physiology"]').is(':checked')) {
            $('label[for*= "Physiology"]').removeClass();
            $('label[for*= "Physiology"]').addClass('btn btn-outline-primary w-90 mb-0');

        } else {
            $('label[for*= "Physiology"]').removeClass();
            $('label[for*= "Physiology"]').addClass('btn btn-outline-danger w-90 mb-0');


        }
        // Respiratory refuses to follow along sooooo accessing each label is required and annoying...

        if ($('input[name$= "[15]"]').is(':checked')) {
            $('label[for= "Respiratory1534"]').removeClass();
            $('label[for= "Respiratory1534"]').addClass('btn btn-outline-primary w-90 mb-0');
            $('label[for= "Respiratory1535"]').removeClass();
            $('label[for= "Respiratory1535"]').addClass('btn btn-outline-primary w-90 mb-0');
            $('label[for= "Respiratory1536"]').removeClass();
            $('label[for= "Respiratory1536"]').addClass('btn btn-outline-primary w-90 mb-0');
        } else {
            $('label[for= "Respiratory1534"]').removeClass();
            $('label[for= "Respiratory1534"]').addClass('btn btn-outline-danger w-90 mb-0');
            $('label[for= "Respiratory1535"]').removeClass();
            $('label[for= "Respiratory1535"]').addClass('btn btn-outline-danger w-90 mb-0');
            $('label[for= "Respiratory1536"]').removeClass();
            $('label[for= "Respiratory1536"]').addClass('btn btn-outline-danger w-90 mb-0');


        }
    });
};

//function to show / hide the textarea within the PCA form and make description required for abnormal entries

function ShowHide(id, idArea) {
    var yes = document.getElementById(id);
    var area = document.getElementById(idArea);
    //    area.className = yes.checked ? 'form-control d-block mb-2' : 'd-none';

    if (yes.checked) {
        $('#' + idArea).prop('required', true).focus();

    }
}

function confirmDelete(uniqueId, isDeleteClicked) {
    var deleteSpan = 'deleteSpan_' + uniqueId;
    var confirmDelete = 'confirmDelete' + uniqueId;

    if (isDeleteClicked) {
        $('#' + deleteSpan).hide();
        $('#' + confirmDelete).show();

    }
    else {
        $('#' + deleteSpan).show();
        $('#' + confirmDelete).hide();
    }
}
