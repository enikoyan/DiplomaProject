// Variables and controls
const loginForm = document.querySelector(".login-form");
const signInBtn = document.querySelector(".login-form__signIn-btn");
const emailInput = document.querySelector(".login-form__input_email");
const passwordInput = document.querySelector(".login-form__input_password");
const title = document.querySelector(".login-form__title");
const loaderContainer = document.getElementById("loader-container");
const loginApiUrl = "http://localhost:8080/auth/loginRequest";
const loaderHTML = `
        <svg class="spinner" width="65px" height="65px" viewBox="0 0 66 66" xmlns="http://www.w3.org/2000/svg">
            <circle class="path" fill="none" stroke-width="6" stroke-linecap="round" cx="33" cy="33" r="30"></circle>
        </svg>
`;

// Message status list
const messageStatusList = ["error", "success", "warning", "info"];

document.addEventListener("DOMContentLoaded", async () => {
  // Login proccess
  loginForm.addEventListener("submit", async function (e) {
    e.preventDefault();

    let result = await validationForm();

    if (result) {
      await createLoader();

      // Get email and password
      let sendData = new FormData();
      sendData.append("email", emailInput.value);
      sendData.append("password", passwordInput.value);

      fetch(loginApiUrl, {
        method: "POST",
        body: sendData,
      })
        .then(async (response) => {
          if (!response.ok) {
            const errorMessage = await response.text();
            await createMessagePopup("error", errorMessage);
            emailInput.classList.add("custom-input_errored");
            passwordInput.classList.add("custom-input_errored");
          }
          return response;
        })
        .then(async (response) => {
          if (response.redirected) {
            emailInput.classList.add("custom-input_correct");
            passwordInput.classList.add("custom-input_correct");
            await createMessagePopup("success", "Вы успешно вошли в аккаунт!");
            setTimeout(() => {
              loginForm.setAttribute("action", "/dashboard");
              loginForm.submit();
              document.location = response.url;
            }, 2000);
          } else await destroyLoader();
        })
        .catch((error) => {
          console.log("Возникла ошибка:", error);
        });
    }
  });

  // Validation
  async function validationForm() {
    if (
      emailInput.value.trim().length == 0 ||
      passwordInput.value.trim().length == 0
    ) {
      createMessagePopup("warning", "Заполните все поля!");
      return false;
    }
    return true;
  }

  // Message popup handler
  async function createMessagePopup(messageStatus, messageText) {
    try {
      document.querySelector(".custom-alert").remove();
    } catch {}

    let popupHTML = `<div class="custom-alert">
            <img class="custom-alert__icon" src="../icons/message-icons/${messageStatus}-message.svg" data-status:"${messageStatus}"/>
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

  // Loader handler
  async function createLoader() {
    loaderContainer.innerHTML = loaderHTML;
  }
  async function destroyLoader() {
    let loader = document.querySelector(".spinner");
    setTimeout(() => loader.remove(), 500);
  }
});
