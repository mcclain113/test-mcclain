// birthRegistryPage.js
// Page-level wiring only

$(function () {
    $('#createRegistryBtn').on('click', function () {

        $('#finalize-tab').tab('show');

        const $finalizeBtn =
            $('#finalize-content')
                .find('button[type="submit"], .save-tab-btn, .finalize-btn, #finalizeSubmitBtn')
                .first();

        if ($finalizeBtn.length) {
            $finalizeBtn.trigger('click');
        } else {
            $('#birthRegistryForm').trigger('submit');
        }
    });

    $(function () {

        let formChanged = false;
        let formSubmit = false;

        const mode = ($('#currentMode').val() || '').toLowerCase();
        const isViewMode = (mode === 'view');

        if (!isViewMode) {
            $('#birthRegistryForm').on('input change', ':input', function () {
                formChanged = true;
            });

            $('#birthRegistryForm').on('submit', function () {
                formSubmit = true;
            });

            window.addEventListener('beforeunload', function (e) {
                if (!formSubmit && formChanged) {
                    e.preventDefault();
                    e.returnValue = '';
                    return '';
                }
            });
        }

        $(document).on('click', '#registryCancelBtn', function (e) {
            if (!isViewMode && !formSubmit && formChanged) {
                const ok = window.confirm('You have unsaved changes. Leave without saving?');
                if (!ok) {
                    e.preventDefault();
                    e.stopPropagation();
                    return false;
                }
            }
        });

        window.birthRegistryMarkSaved = function () {
            formChanged = false;
            formSubmit = false;
        };
    });
});