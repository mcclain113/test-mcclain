// this script handles Delete confirmation whether in regular view or a modal view
//  to implement:  add near the bottom of the cshtml:  <script src="~/js/confirm.js" defer></script>
// - 'defer' ensures the script executes after parsing
// - the script uses cature listeners so it will still intercept submits
// - The file is cached by the browser and safe to include in multiple partials because it guards against double-binding
(function () {
  // guard against double-loading
  if (window.__confirmHandlerInstalled) return;
  window.__confirmHandlerInstalled = true;

  var lastClickedSubmit = null;

  // capture-phase click listener to remember which submit button was clicked
  document.addEventListener('click', function (ev) {
    var t = ev.target;
    while (t && t !== document) {
      if ((t.tagName === 'BUTTON' || (t.tagName === 'INPUT' && (t.type === 'submit' || t.type === 'image'))) && t.hasAttribute('data-confirm')) {
        lastClickedSubmit = t;
        return;
      }
      t = t.parentElement;
    }
    lastClickedSubmit = null;
  }, true);

  // delegated submit handler (capture so it runs before other handlers)
  document.addEventListener('submit', function (ev) {
    var form = ev.target;
    if (!form || form.tagName !== 'FORM') return;

    // prefer form-level data-confirm
    var msg = form.getAttribute('data-confirm') || null;

    // fallback to clicked submit button
    if (!msg && lastClickedSubmit && form.contains(lastClickedSubmit)) {
      msg = lastClickedSubmit.getAttribute('data-confirm') || null;
    }

    // fallback to active element (keyboard submit)
    if (!msg) {
      var active = document.activeElement;
      if (active && (active.tagName === 'BUTTON' || active.tagName === 'INPUT') && active.hasAttribute('data-confirm') && form.contains(active)) {
        msg = active.getAttribute('data-confirm');
      }
    }

    if (msg) {
      // synchronous confirm BEFORE any other submit handling
      if (!window.confirm(msg)) {
        ev.preventDefault();
        ev.stopImmediatePropagation();
        lastClickedSubmit = null;
        return false;
      }
    }

    // clear remembered submit button
    lastClickedSubmit = null;
    // allow submit to continue (AJAX handlers or normal submit will run next)
  }, true);

  // helper: if other code calls form.submit(), it bypasses the submit event.
  // Provide a small helper to call instead of form.submit()
  window.confirmedSubmit = function (form) {
    if (!form || form.tagName !== 'FORM') return;
    var evt = new Event('submit', { cancelable: true });
    if (!form.dispatchEvent(evt)) {
      // canceled by handler
      return false;
    }
    // if not canceled, call native submit
    form.submit();
    return true;
  };
})();