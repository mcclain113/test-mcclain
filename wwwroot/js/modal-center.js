// // this file is used to center modals when the page content is dynamic or larger than the viewport
// // to implement:  add: data-center-viewport="true" or class="center-viewport" to any modal to which this behavior should be applied
// // currently applied to the sessionExpiry modal contained with the _Layout.cshtml
// (function () {
//   if (window.__modalCenterInstalled) return;
//   window.__modalCenterInstalled = true;

//   function applyFixedCentering($modal) {
//     var $dialog = $modal.find('.modal-dialog');

//     // ensure modal is a child of body so fixed positioning is viewport-based
//     $modal.appendTo(document.body);

//     // make dialog fixed and centered via transform
//     $dialog.css({
//       position: 'fixed',
//       left: '50%',
//       top: '50%',
//       transform: 'translate(-50%, -50%)',
//       margin: 0 // remove bootstrap margins that interfere
//     });

//     // make body scrollable so very tall content doesn't overflow viewport
//     $modal.find('.modal-body').css({
//       'max-height': 'calc(100vh - 160px)',
//       'overflow-y': 'auto'
//     });
//   }

//   function centerModalIfNeeded($modal) {
//     // fallback: compute top margin if you prefer margin-based centering
//     var $dialog = $modal.find('.modal-dialog');
//     $dialog.css('margin-top', '');
//     var vh = Math.max(document.documentElement.clientHeight, window.innerHeight || 0);
//     var dialogH = $dialog.outerHeight(true);
//     var top = Math.max(20, (vh - dialogH) / 2);
//     $dialog.css('margin-top', top + 'px');
//   }

//   // When a modal with the marker is about to be shown, apply fixed centering
//   $(document).on('show.bs.modal', '.modal.center-viewport, .modal[data-center-viewport="true"]', function () {
//     applyFixedCentering($(this));
//   });

//   // After shown, recalc in case content changed height
//   $(document).on('shown.bs.modal', '.modal.center-viewport, .modal[data-center-viewport="true"]', function () {
//     var $m = $(this);
//     centerModalIfNeeded($m);
//     // small delayed recalc for images/fonts/plugins that load after show
//     setTimeout(function () { centerModalIfNeeded($m); }, 200);
//   });

//   // set the z-index so the modal appears on top of any other apps
//   $(document).on('shown.bs.modal', '.modal.center-viewport, .modal[data-center-viewport="true"]', function () {
//     // choose a value higher than any other site z-index
//     var topZ = 99999;
//     // set the last backdrop (the one for this modal) just below the modal
//     $('.modal-backdrop').last().css('z-index', topZ - 1);
//     // set the modal itself on top
//     $(this).css('z-index', topZ);

//     // move focus into the modal (first focusable or the Stay button)
//     var $focusTarget = $(this).find('[autofocus], button, a, input').first();
//     if ($focusTarget.length) $focusTarget.focus();
//     else $(this).attr('tabindex', '-1').focus();
//   });

//   // Recenter on window resize (debounced)
//   var resizeTimer;
//   $(window).on('resize', function () {
//     clearTimeout(resizeTimer);
//     resizeTimer = setTimeout(function () {
//       $('.modal.show.center-viewport, .modal.show[data-center-viewport="true"]').each(function () {
//         centerModalIfNeeded($(this));
//       });
//     }, 120);
//   });
// })();

// this file is used to center modals when the page content is dynamic or larger than the viewport
// to implement:  add: data-center-viewport="true" or class="center-viewport" to any modal to which this behavior should be applied
// currently applied to the sessionExpiry modal contained with the _Layout.cshtml
(function () {
  if (window.__modalCenterInstalled) return;
  window.__modalCenterInstalled = true;

  // page root to mark inert / aria-hidden while modal is open
  var pageRoot = document.querySelector('main') || document.body;
  var TOP_Z = 99999; // choose a value higher than any site z-index
  var lastFocusedElement = null;

  function applyFixedCentering($modal) {
    var $dialog = $modal.find('.modal-dialog');

    // ensure modal is a child of body so fixed positioning is viewport-based
    $modal.appendTo(document.body);

    // make dialog fixed and centered via transform
    $dialog.css({
      position: 'fixed',
      left: '50%',
      top: '50%',
      transform: 'translate(-50%, -50%)',
      margin: 0 // remove bootstrap margins that interfere
    });

    // make body scrollable so very tall content doesn't overflow viewport
    $modal.find('.modal-body').css({
      'max-height': 'calc(100vh - 160px)',
      'overflow-y': 'auto'
    });
  }

  function centerModalIfNeeded($modal) {
    // fallback: compute top margin if you prefer margin-based centering
    var $dialog = $modal.find('.modal-dialog');
    $dialog.css('margin-top', '');
    var vh = Math.max(document.documentElement.clientHeight, window.innerHeight || 0);
    var dialogH = $dialog.outerHeight(true);
    var top = Math.max(20, (vh - dialogH) / 2);
    $dialog.css('margin-top', top + 'px');
  }

  // helper: set z-index for backdrop and modal (call after backdrop exists)
  function bringModalToFront($modal) {
    // set the last backdrop (the one for this modal) just below the modal
    $('.modal-backdrop').last().css('z-index', TOP_Z - 1);
    // set the modal itself on top
    $modal.css('z-index', TOP_Z);
  }

  // helper: move focus into modal
  function focusFirstInModal($modal) {
    var $focusTarget = $modal.find('[autofocus], button, a, input, [tabindex]').filter(':visible').first();
    if ($focusTarget && $focusTarget.length) {
      $focusTarget.focus();
    } else {
      // give the modal a focusable fallback
      $modal.attr('tabindex', '-1').focus();
    }
  }

  // When a modal with the marker is about to be shown, apply fixed centering
  $(document).on('show.bs.modal', '.modal.center-viewport, .modal[data-center-viewport="true"]', function () {
    var $modal = $(this);

    // store the currently focused element so we can restore focus when modal closes
    try { lastFocusedElement = document.activeElement; } catch (e) { lastFocusedElement = null; }

    // blur any focused element so aria-hidden won't be blocked
    try { if (document.activeElement && document.activeElement !== document.body) document.activeElement.blur(); } catch (e) {}

    // apply centering and ensure modal is appended to body
    applyFixedCentering($modal);

    // mark background inert if supported, otherwise set aria-hidden on page root
    try {
      if ('inert' in HTMLElement.prototype) {
        pageRoot.inert = true;
      } else {
        pageRoot.setAttribute('aria-hidden', 'true');
      }
    } catch (e) {
      // ignore errors from inert toggling
    }

    // small timeout so the backdrop element exists before we set z-index
    setTimeout(function () {
      bringModalToFront($modal);
    }, 0);
  });

  // After shown, recalc in case content changed height and move focus into modal
  $(document).on('shown.bs.modal', '.modal.center-viewport, .modal[data-center-viewport="true"]', function () {
    var $m = $(this);
    centerModalIfNeeded($m);

    // small delayed recalc for images/fonts/plugins that load after show
    setTimeout(function () { centerModalIfNeeded($m); }, 200);

    // ensure z-index is set (defensive)
    bringModalToFront($m);

    // move focus into the modal (first focusable or fallback)
    try { focusFirstInModal($m); } catch (e) {}
  });

  // before hiding, blur focused element inside modal so aria-hidden can be applied without being blocked
  $(document).on('hide.bs.modal', '.modal.center-viewport, .modal[data-center-viewport="true"]', function () {
    try { $(this).find(':focus').blur(); } catch (e) {}
  });

  // After hidden, remove inert/aria-hidden and restore focus
  $(document).on('hidden.bs.modal', '.modal.center-viewport, .modal[data-center-viewport="true"]', function () {
    try {
      if ('inert' in HTMLElement.prototype) {
        pageRoot.inert = false;
      } else {
        pageRoot.removeAttribute('aria-hidden');
      }
    } catch (e) {
      // ignore
    }

    // restore focus to previously focused element if still in document
    try {
      if (lastFocusedElement && typeof lastFocusedElement.focus === 'function') {
        lastFocusedElement.focus();
      } else {
        // fallback: focus body
        document.body.focus();
      }
    } catch (e) {}
  });

  // Recenter on window resize (debounced)
  var resizeTimer;
  $(window).on('resize', function () {
    clearTimeout(resizeTimer);
    resizeTimer = setTimeout(function () {
      $('.modal.show.center-viewport, .modal.show[data-center-viewport="true"]').each(function () {
        centerModalIfNeeded($(this));
      });
    }, 120);
  });
})();

