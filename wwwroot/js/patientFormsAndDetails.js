$(onReady);

function onReady(){
    // use ajax to access a controller getter method for displaying the _AliasPartial.cshtml upon a button click by the user
    var indexPa = $('#Aliases').children('.patientAliases').length; 
                console.log("Initial indexPa:", indexPa);
        $("#addAliasButton").on("click", function (e) {
            e.preventDefault();
            var viewPatientDetails = $('#ViewPatientDetails').val();
                console.log("viewPatientDetails:", viewPatientDetails);
            $.get("/Patient/GetAliasPartial", 
                { 
                    indexPa: indexPa, 
                    viewPatientDetails: viewPatientDetails 
                }, 
                function (data) {
                    console.log("Received data:", data);
                $("#Aliases").append(data);
                indexPa++;
                    console.log("Updated indexPa: ", indexPa);
            });
        });

    // use ajax to access a controller getter method for displaying the _EmergencyContactPartial.cshtml upon a button click by the user
    var index = $('#emergencyContactsContainer').children('.emergencyContacts').length;

    $('#addEmergencyContactBtn').click(function () {
        var viewPatientDetails = $('#ViewPatientDetails').val();
        $.get("/Patient/AddEmergencyContact",
            {
                index: index, 
                viewPatientDetails: viewPatientDetails
            }, 
            function(data){
            $('#emergencyContactsContainer').append(data);
            index++;

            // Apply inputmask functions to the newly added elements
            // These need to be here because they reside in the site.js which does not get refreshed when this callback function renders the _EmergencyContactPartial.cshtml
            // They cannot be in the _EmergencyContactPartial.cshtml because that page is displayed with the initial loading of the _PatientAccordianPartial.cshtml and would conflict with the jquery in site.js;  it must be in site.js because other cshtml also apply them
            $(".phone").inputmask({"mask": "(999)-999-9999"});
            $(".zipcode").inputmask({"mask": "99999[-9999]"});
        });
    });
    
    // Handle the ResidenceSameAsMailing checkbox change
    $(document).on("click", "#ResidenceSameAsMailing", function (e) {
        let parent = $(e.target).closest(".checkboxcontainer");
        let targetSection = $(parent).siblings("#MailingAddress");
        let ResidenceSameAsMailing = $(e.target).prop("checked");
        let MailingAddress = $(targetSection);
        MailingAddress.prop("disabled", ResidenceSameAsMailing);
        MailingAddress.prop("hidden", ResidenceSameAsMailing);
    });

    // Initialize the DeathDate field based on the checkbox state
    $('#DeathDate').prop('disabled', !$('#deceasedCheck').prop('checked'));

    // Add event listener for checkbox change
    $('#deceasedCheck').on('change', function () {
        $('#DeathDate').prop('disabled', !$(this).prop('checked'));
    });

    function toggleButtonStatus() {
        emergencyContactCount = $("#emergencyContactsContainer").children().length;
        legalGuardian = $("#LegalGuardianEmergencyContactId").val();
        if ((emergencyContactCount > 2 && legalGuardian) || (emergencyContactCount > 3 && !legalGuardian)) {
            $("#emergencyContactButton").hide();
        } else {
            $("#emergencyContactButton").show();
        }
    }

    $(document).on("click", ".remove-item-btn", function (e){
        e.preventDefault();
        let targetSection = $(e.target).closest("fieldset");
        targetSection.remove();
        toggleButtonStatus();
    });

    $(".info").popover();

    $(".info").on("click", function(e){
        e.preventDefault();
    });
    
    // these next three items set the Placeholder text in the dropdown boxes which use the select2 javascript plugin.  The first two are in the _ContactInfoPartial.cshtml and the third and fourth are in the _DemographicsPartial.cshtml
    $(".contact-time").select2({
        width: '100%',
        placeholder: "Select Contact Time(s)",
    });

    $(".contact-method").select2({
        width: '100%',
        placeholder: "Select Contact Method(s)"
    });

    $(".races").select2({
        width: '100%',
        placeholder: "Select Race(s)"
    });

    $(".languages").select2({
        width: '100%',
        placeholder: "Select Language(s)"
    });


    //This will keep select2 from making the placeholder not appear at the beginning
    $(".select2-search__field").css("width", "100%");

    // this uses the inputMask.js plugin to provide the format to the user;  code is included in a Helper class to strip the added characters prior to posting as the property expects only numbers
    $(".ssn").inputmask({"mask": "999-99-9999"});

    // run functions on page load to load listeners
    validateEmployment();
    validateInsurance1();
    validateInsurance2();
    validateInsurance3();
    validateEmergencyContact();

    // Employment fields validation
    function validateEmployment() {
        // Checks for partial address and validates to user.
        $(".validate-employment").on("input mouseup", function (e) {
            if ($(e.target).has(".validate")) {
                const $inputs = $(this).find("input.validate, select.validate")
                
                let anyFilled = false;
                $inputs.each(function (index, e) {
                    if(e.value.length != 0) {
                        anyFilled = true
                    }
                })

                if (anyFilled) {
                    $inputs.each(function (index, e) {
                        if(e.value.length == 0) {
                            $(e).not(".not-required").addClass("invalid");
                            $(e).not(".not-required").prop('required',true);
                            $(e).nextAll(".invalid-summary").first().show();
                        } else {
                            $(e).not(".not-required").removeClass("invalid");
                            $(e).prop('required',false);
                            $(e).nextAll(".invalid-summary").first().hide();
                        }
                    })
                } else {
                    $inputs.each(function (index, e) {
                        $(e).removeClass("invalid");
                        $(e).prop('required',false);
                        $(e).nextAll(".invalid-summary").first().hide();
                    })
                }
            }
        })   
    };

    // Insurance fields validation - Primary Insurance
    function validateInsurance1() {
        // Checks for partial address and validates to user.
        $(".validate-insurance1").on("input mouseup", function (e) {
            if ($(e.target).has(".validate")) {
                const $inputs = $(this).find("input.validate, select.validate")
                
                let anyFilled = false;
                $inputs.each(function (index, e) {
                    if(e.value.length != 0) {
                        anyFilled = true
                    }
                })

                if (anyFilled) {
                    $inputs.each(function (index, e) {
                        if(e.value.length == 0) {
                            $(e).not(".not-required").addClass("invalid");
                            $(e).not(".not-required").prop('required',true);
                            $(e).nextAll(".invalid-summary").first().show();
                        } else {
                            $(e).not(".not-required").removeClass("invalid");
                            $(e).prop('required',false);
                            $(e).nextAll(".invalid-summary").first().hide();
                        }
                    })
                } else {
                    $inputs.each(function (index, e) {
                        $(e).removeClass("invalid");
                        $(e).prop('required',false);
                        $(e).nextAll(".invalid-summary").first().hide();
                    })
                }
            }
        })   
    };

    // Insurance fields validation - Secondary Insurance
    function validateInsurance2() {
        // Checks for partial address and validates to user.
        $(".validate-insurance2").on("input mouseup", function (e) {
            if ($(e.target).has(".validate2")) {
                const $inputs = $(this).find("input.validate2, select.validate2")
                
                let anyFilled = false;
                $inputs.each(function (index, e) {
                    if(e.value.length != 0) {
                        anyFilled = true
                    }
                })

                if (anyFilled) {
                    $inputs.each(function (index, e) {
                        if(e.value.length == 0) {
                            $(e).not(".not-required").addClass("invalid");
                            $(e).not(".not-required").prop('required',true);
                            $(e).nextAll(".invalid-summary").first().show();
                        } else {
                            $(e).not(".not-required").removeClass("invalid");
                            $(e).prop('required',false);
                            $(e).nextAll(".invalid-summary").first().hide();
                        }
                    })
                } else {
                    $inputs.each(function (index, e) {
                        $(e).removeClass("invalid");
                        $(e).prop('required',false);
                        $(e).nextAll(".invalid-summary").first().hide();
                    })
                }
            }
        })   
    };

    // Insurance fields validation - Secondary Insurance
    function validateInsurance3() {
        // Checks for partial address and validates to user.
        $(".validate-insurance3").on("input mouseup", function (e) {
            if ($(e.target).has(".validate3")) {
                const $inputs = $(this).find("input.validate3, select.validate3")
                
                let anyFilled = false;
                $inputs.each(function (index, e) {
                    if(e.value.length != 0) {
                        anyFilled = true
                    }
                })

                if (anyFilled) {
                    $inputs.each(function (index, e) {
                        if(e.value.length == 0) {
                            $(e).not(".not-required").addClass("invalid");
                            $(e).not(".not-required").prop('required',true);
                            $(e).nextAll(".invalid-summary").first().show();
                        } else {
                            $(e).not(".not-required").removeClass("invalid");
                            $(e).prop('required',false);
                            $(e).nextAll(".invalid-summary").first().hide();
                        }
                    })
                } else {
                    $inputs.each(function (index, e) {
                        $(e).removeClass("invalid");
                        $(e).prop('required',false);
                        $(e).nextAll(".invalid-summary").first().hide();
                    })
                }
            }
        })   
    };

    // Emergency Contact fields validation
    function validateEmergencyContact() {
        // Checks for partial address and validates to user.
        $(".validate-emergencyContact").on("input mouseup", function (e) {
            if ($(e.target).has(".validate")) {
                const $inputs = $(this).find("input.validate, select.validate")
                
                let anyFilled = false;
                $inputs.each(function (index, e) {
                    if(e.value.length != 0) {
                        anyFilled = true
                    }
                })

                if (anyFilled) {
                    $inputs.each(function (index, e) {
                        if(e.value.length == 0) {
                            $(e).not(".not-required").addClass("invalid");
                            $(e).not(".not-required").prop('required',true);
                            $(e).nextAll(".invalid-summary").first().show();
                        } else {
                            $(e).not(".not-required").removeClass("invalid");
                            $(e).prop('required',false);
                            $(e).nextAll(".invalid-summary").first().hide();
                        }
                    })
                } else {
                    $inputs.each(function (index, e) {
                        $(e).removeClass("invalid");
                        $(e).prop('required',false);
                        $(e).nextAll(".invalid-summary").first().hide();
                    })
                }
            }
        })   
    }

}