/**
 * Attaches an event listener to the specified submit button so that when the form is submitted,
 * the form data is sent to the controller method AdjustSortOrders
 * 
 * Currently used by both CreateDisclosureFeeType.cshtml & EditDisclosureFeeType.cshtml
 *
 * @param {string} formSelector - The jQuery selector for the form (e.g. "#disclosureFeeForm").
 * @param {string} buttonSelector - The selector for the submission button (e.g. "#btnSave").
 */
function updateSortOrdersOnSubmit(formSelector, buttonSelector) {
    $(buttonSelector).on("click", function (e) {
        e.preventDefault();
        
        var $form = $(formSelector);
        // Get all form fields as an array
        var formFields = $form.serializeArray();
        var disclosureFeeTypeData = {};

        $.each(formFields, function (i, field) {
            // Process only fields that belong to the DisclosureFeeType model.
            // They are normally rendered with the name "DisclosureFeeType.PropertyName"
            if (field.name.indexOf("DisclosureFeeType.") === 0) {
                var propertyName = field.name.substring("DisclosureFeeType.".length);
                // Skip any field that starts with "__" (like __Invariant or __RequestVerificationToken)
                if (propertyName.indexOf("__") === 0) {
                    return;
                }
                // If it’s ExpirationDate and its value is empty, set it to null
                if (propertyName === "ExpirationDate" && field.value.trim() === "") {
                    disclosureFeeTypeData[propertyName] = null;
                } else if (propertyName === "SortOrder") {
                    // Ensure SortOrder is converted to an integer
                    disclosureFeeTypeData[propertyName] = parseInt(field.value, 10);
                } else {
                    disclosureFeeTypeData[propertyName] = field.value;
                }
            }
        });
        
        // Wrap the data in an object matching your RevenueStructuredDataViewModel structure.
        var payload = {
            DisclosureFeeType: disclosureFeeTypeData
        };

        console.log("Sending payload: ", payload);

        $.ajax({
            url: '/RevenueStructuredData/AdjustSortOrders',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(payload),
            success: function (response) {
                // Instead of submitting the form (which would double-submit the create case),
                // simply redirect the user to the list view (or another confirmation page).
                window.location.href = '/RevenueStructuredData/ViewDisclosureFeeTypes';
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.error("Error adjusting sort orders:", textStatus, errorThrown);
                alert("There was an error adjusting the sort orders. Please try again.");
            }
        });
    });
}

// This function intercepts the Delete form submission, deletes the record, then renumbers remaining items.
function deleteAndRenumber(formSelector, buttonSelector) {
    $(document).on("click", buttonSelector, function (e) {
        e.preventDefault();

        if (!confirm("Are you sure you want to delete this record?")) {
            return;
        }

        var $form = $(formSelector);
        var deletedSortOrder = parseInt($form.find("#SortOrder").val(), 10);

        // First, call the delete endpoint via Ajax
        $.ajax({
            url: $form.attr("action"),
            type: "POST",
            data: $form.serialize(),
            success: function (deleteResponse) {
                // Now renumber the remaining items
                $.ajax({
                    url: '/RevenueStructuredData/RenumberSortOrders',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify({ DeletedSortOrder: deletedSortOrder }),
                    success: function (renumberResponse) {
                        window.location.href = '/RevenueStructuredData/ViewDisclosureFeeTypes';
                    },
                    error: function (err) {
                        console.error("Error during renumbering:", err);
                        alert("Record deleted but encountered an error when renumbering sort orders.");
                        window.location.href = '/RevenueStructuredData/ViewDisclosureFeeTypes';
                    }
                });
            },
            error: function (error) {
                console.error("Error deleting the record:", error);
                alert("Error deleting record. Please try again.");
            }
        });
    });
}

$(document).ready(function () {
    // For example, if your form has the id "disclosureFeeForm" 
    // and the save/submit button has the id "btnSave", initialize the handler:
    if($("#disclosureFeeTypeForm").length){
        updateSortOrdersOnSubmit("#disclosureFeeTypeForm", "#btnSave");
    }
    
    // Wire up the delete-and-renumber action.
    if($("#deleteDisclosureFeeTypeForm").length){
        deleteAndRenumber("#deleteDisclosureFeeTypeForm", "#btnDelete");
    }
    
});