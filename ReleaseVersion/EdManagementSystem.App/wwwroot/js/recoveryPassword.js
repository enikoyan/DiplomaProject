// Variables and controls
const backBtn = document.querySelector(".custom-btn_secondary");
const indicator = document.getElementById("timerSpan");
const timer = document.querySelector(".timer");
const lsKey = "timerEnd";
const recoveryForm = document.getElementById("changePasswordForm");
const passwordInput = document.getElementById("passwordInput");
const keyInput = document.getElementById("keyInput");
var changePasswordAPI = "http://localhost:8001/api/auth/changePassword";
const expireTime = 300;

window.addEventListener("DOMContentLoaded", async () => {
  backBtn.addEventListener("click", async () => {
    if (history.length > 1) {
      history.back();
    }
  });

  if (JSON.parse(localStorage.getItem("recoveryTimer_started")) == false) {
    runTimer();
    localStorage.setItem("recoveryTimer_started", true);
    localStorage.setItem("recoveryTimer_ended", false);
  }

  recoveryForm.addEventListener("submit", async (e) => {
    e.preventDefault();

    // Check inputs
    let checkFieldsResult = await checkInputFields();

    if (checkFieldsResult === true) {
      await callChangePasswordAPI();
    }
  });

  async function callChangePasswordAPI() {
    const formData = new FormData();
    formData.append("serverKey", localStorage.getItem("serverKey"));
    formData.append("clientKey", keyInput.value);
    formData.append("newPassword", passwordInput.value);

    // Query options
    const requestOptions = {
      method: "POST",
      body: formData,
    };

    // Calling API method
    fetch(changePasswordAPI, requestOptions)
      .then((response) => {
        if (response.ok) {
          return response.text();
        } else {
          return response.text();
        }
      })
      .then(async (data) => {
        await createMessagePopup("info", data);
        localStorage.removeItem(lsKey);
        localStorage.setItem("recoveryTimer_ended", true);
        localStorage.setItem("recoveryTimer_started", false);
        localStorage.removeItem("serverKey");
        setTimeout(async () => {
          location.href = "http://localhost:8080/auth/login";
        }, 1000);
      })
      .catch(async (error) => {
        await createMessagePopup("error", error);
      });
  }

  async function checkInputFields() {
    if (keyInput.value.length == 0) {
      keyInput.classList.add("custom-input_errored");
      return false;
    } else if (passwordInput.value.length == 0) {
      keyInput.classList.add("custom-input_correct");
      passwordInput.classList.add("custom-input_errored");
      return false;
    } else {
      keyInput.classList.add("custom-input_correct");
      passwordInput.classList.add("custom-input_correct");
      return true;
    }
  }

  /* TIMER */
  function runTimer() {
    timer.style.display = "block";
    let D = new Date();
    D.setTime(D.getTime() + 1000 * expireTime);
    timerStart(D);
  }
  let savedTime = parseInt(window.localStorage.getItem(lsKey));
  if (savedTime) {
    timer.style.display = "block";
    let D = new Date();
    D.setTime(savedTime);
    timerStart(D);
  }

  function timerStart(finishDate) {
    let LS = window.localStorage;
    let D = new Date();

    let seconds = Math.round((finishDate - D) / 1000);
    if (seconds <= 0) {
      LS.removeItem(lsKey);
      localStorage.setItem("recoveryTimer_ended", true);
      localStorage.removeItem("serverKey");
      return;
    }

    LS.setItem(lsKey, finishDate.getTime()); // запомнили в LS

    indicator.textContent = seconds;
    setIndicator(true);
    let timerId = setInterval(() => {
      let seconds = Math.round((finishDate - new Date()) / 1000);
      indicator.textContent = seconds;
      if (seconds <= 0) {
        LS.removeItem(lsKey);
        clearInterval(timerId);
        setIndicator(false);
      }
    }, 100);
  }
  function setIndicator(onOff) {
    if (!onOff) {
      timer.classList.add(`timer_lessLeft`);
      localStorage.setItem("recoveryTimer_ended", true);
      localStorage.removeItem("serverKey");
    }
  }

  // Message popup handler
  async function createMessagePopup(messageStatus, messageText) {
    try {
      document.querySelector(".custom-alert").remove();
    } catch {}

    let popupHTML = `<div class="custom-alert">
            <img class="custom-alert__icon" src="../../icons/message-icons/${messageStatus}-message.svg" data-status:"${messageStatus}"/>
            <p class="custom-alert__message">${messageText}</p>
            <span class="custom-alert__close-btn close-btn"></span>
        </div>`;

    const popupElement = document.createElement("div");
    popupElement.innerHTML = popupHTML;
    document.body.appendChild(popupElement);

    let closeButtonClicked = false;

    document
      .querySelector(".custom-alert__close-btn")
      .addEventListener("click", async () => {
        closeButtonClicked = true;
        popupElement.remove();
      });

    setTimeout(async () => {
      if (!closeButtonClicked) {
        await destroyMessagePopup();
      }
    }, 4000);
  }
  async function destroyMessagePopup() {
    const popup = document.querySelector(".custom-alert");
    popup.classList.add("hide");

    setTimeout(() => {
      popup.remove();
    }, 500);
  }

  if (localStorage.getItem("serverKey") == null) {
    location.href =
      "http://localhost:8080/auth/recoveryPassword/getRecoveryKey";
  }
});
