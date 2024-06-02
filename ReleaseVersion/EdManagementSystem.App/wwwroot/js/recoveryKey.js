// Variables and controls
const sendCodeForm = document.getElementById("sendCodeForm");
const backBtn = document.querySelector(".custom-btn_secondary");
const emailInput = document.querySelector(".reset-form__input_email");
var getServerKeyAPI =
  "http://localhost:8001/api/auth/getRecoveryKey?userEmail=";

window.addEventListener("DOMContentLoaded", async () => {
  if (localStorage.getItem("serverKey") != null) {
    location.href = "http://localhost:8080/auth/recoveryPassword";
  }

  backBtn.addEventListener("click", async () => {
    if (history.length > 1) {
      history.back();
    }
  });

  sendCodeForm.addEventListener("submit", async (e) => {
    e.preventDefault();

    // Check input field
    let result = await checkEmailField();

    // Call API and get the server key
    if (result == true && localStorage.getItem("serverKey") == null) {
      let apiResult = await getServerKey();
    }
  });

  // Get server key by API
  async function getServerKey() {
    fetch((getServerKeyAPI += emailInput.value))
      .then(async (response) => {
        if (!response.ok) {
          emailInput.classList.remove("custom-input_correct");
          emailInput.classList.add("custom-input_errored");
          const errorMessage = await response.text();
          await createMessagePopup("error", errorMessage);
          return;
        } else return response.text();
      })
      .then(async (response) => {
        if (response) {
          emailInput.classList.remove("custom-input_errored");
          emailInput.classList.add("custom-input_correct");
          await createMessagePopup(
            "success",
            "Письмо для восстановления пароля успешно отправлено на Вашу почту!"
          );
          localStorage.setItem("serverKey", response);
          setTimeout(async () => {
            await goToSecondPart();
          }, 1000);
        }
      })
      .catch((error) => {
        console.log("Возникла ошибка:", error);
      });
  }

  // Check email field
  async function checkEmailField() {
    if (emailInput.value.length == 0) {
      emailInput.classList.add("custom-input_errored");
      await createMessagePopup("warning", "Заполните поле электронной почты!");
      return false;
    } else {
      emailInput.classList.add("custom-input_correct");
      return true;
    }
  }

  // Go to second part of recovery
  async function goToSecondPart() {
    let lastIndex = window.location.href.lastIndexOf("/");
    let newUrl = window.location.href.substring(0, lastIndex);

    location.href = newUrl;

    if (
      localStorage.getItem("recoveryTimer_started") === null ||
      localStorage.getItem("recoveryTimer_ended") == "true"
    ) {
      localStorage.setItem("recoveryTimer_started", false);
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
});
