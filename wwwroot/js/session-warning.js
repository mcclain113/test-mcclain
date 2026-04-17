(function () {
  // Read config from DOM / global
  var totalSeconds = parseInt(document.body.dataset.sessionTimeoutSeconds, 10) || 3600;
  var warnBeforeSeconds = 120; // warn 2 minutes before expiry
  var keepAliveUrl = (window.__appConfig && window.__appConfig.keepAliveUrl) || '/Session/KeepAlive';
  var expiredUrl = (window.__appConfig && window.__appConfig.expiredUrl) || '/Session/Expired';

  var showWarningAfterMs = Math.max(0, (totalSeconds - warnBeforeSeconds) * 1000);
  var countdownInterval = null;
  var warningTimeout = null;

  function $id(id) { return document.getElementById(id); }
  var modalEl = $('#sessionExpiryModal');
  var countdownEl = $id('sessionExpiryCountdown');
  var messageEl = $id('sessionExpiryMessage');
  var stayBtn = $id('sessionStayBtn');
  var signOutBtn = $id('sessionSignOutBtn');

  function getCsrfToken() {
    var meta = document.querySelector('meta[name="csrf-token"]');
    return meta ? meta.getAttribute('content') : null;
  }

  function startWarningTimer() {
    clearTimers();
    warningTimeout = setTimeout(showWarning, showWarningAfterMs);
  }

  function clearTimers() {
    if (warningTimeout) { clearTimeout(warningTimeout); warningTimeout = null; }
    if (countdownInterval) { clearInterval(countdownInterval); countdownInterval = null; }
  }

  function showWarning() {
    var remaining = warnBeforeSeconds;
    if (messageEl) messageEl.textContent = 'Your session will expire in ' + remaining + ' seconds.';
    if (countdownEl) countdownEl.textContent = '';
    modalEl.modal({ backdrop: 'static', keyboard: false });
    modalEl.modal('show');

    countdownInterval = setInterval(function () {
      remaining--;
      if (remaining <= 0) {
        clearInterval(countdownInterval);
        modalEl.modal('hide');
        window.location.href = expiredUrl;
      } else {
        if (messageEl) messageEl.textContent = 'Your session will expire in ' + remaining + ' seconds.';
      }
    }, 1000);
  }

  function keepAlive() {
    var token = getCsrfToken();
    var headers = {
      'X-Requested-With': 'XMLHttpRequest',
      'Content-Type': 'application/json'
    };
    if (token) headers['RequestVerificationToken'] = token;

    fetch(keepAliveUrl, {
      method: 'POST',
      headers: headers,
      credentials: 'same-origin',
      body: null
    }).then(function (res) {
      if (res.ok) {
        if (modalEl && modalEl.hasClass('show')) modalEl.modal('hide');
        clearTimers();
        startWarningTimer();
      } else {
        window.location.href = expiredUrl;
      }
    }).catch(function () {
      window.location.href = expiredUrl;
    });
  }

  function resetOnActivity() {
    clearTimers();
    startWarningTimer();
  }

  document.addEventListener('DOMContentLoaded', function () {
    if (stayBtn) stayBtn.addEventListener('click', keepAlive);
    if (signOutBtn) signOutBtn.addEventListener('click', function () {
      window.location.href = expiredUrl;
    });

    startWarningTimer();

    var activityEvents = ['click', 'mousemove', 'keydown', 'touchstart'];
    activityEvents.forEach(function (evt) {
      window.addEventListener(evt, resetOnActivity, { passive: true });
    });
  });
})();
