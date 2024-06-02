const submitForm = document.getElementById('submit-form');

// Preloader
var preloader = document.querySelector(".preloader");
var lottieLogo = document.getElementById("logoLottie");
var links = document.querySelectorAll("a");

// Message popup handler
async function createMessagePopup(messageStatus, messageText) {
    try {
        document.querySelector('.custom-alert').remove();
    }
    catch {

    }

    let popupHTML =
        `<div class="custom-alert">
            <img class="custom-alert__icon" src="../icons/message-icons/${messageStatus}-message.svg" data-status:"${messageStatus}"/>
            <p class="custom-alert__message">${messageText}</p>
            <span class="custom-alert__close-btn close-btn"></span>
        </div>`;

    const popupElement = document.createElement('div');
    popupElement.innerHTML = popupHTML;
    document.body.appendChild(popupElement);

    let closeButtonClicked = false;

    document.querySelector('.custom-alert__close-btn').addEventListener('click', async () => {
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
    const popup = document.querySelector('.custom-alert');
    popup.classList.add('hide');

    setTimeout(() => {
        popup.remove();
    }, 500);
}

document.addEventListener('DOMContentLoaded', async () => {

    submitForm.addEventListener('submit', async (event) => {
        event.preventDefault();

        // Получение текущего пользователя
        let userEmail;
        try {
            let response = await fetch('/dashboard/getCurrentUserId');
            userEmail = await response.text();
        } catch (error) {
            await createMessagePopup("error", error);
            return;
        }

        // Отправка вопроса на сервер
        let requestDescription = document.getElementById('question-form__text').value;

        if (!requestDescription.trim()) {
            await createMessagePopup("warning", "Поле не может быть пустым!");
        } else if (requestDescription.length < 250) {
            await createMessagePopup("warning", "Обращение должно быть минимум на 250 символов");
        } else {
            preloader.classList.remove("preloader_hidden");
            lottieLogo.classList.remove("logoLottie_hidden");
            animation.play();

            try {
                let response = await fetch('https://localhost:44370/api/TechSupports/CreateRequest', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ UserEmail: userEmail, RequestDescription: requestDescription })
                });

                preloader.classList.add("preloader_hidden");
                lottieLogo.classList.add("logoLottie_hidden");
                animation.pause();

                if (response.ok) {
                    createMessagePopup("success", "Обращение успешно отправлено!");
                    document.getElementById('question-form__text').value = '';
                } else {
                    createMessagePopup("error", "Не удалось отправить обращение!");
                }
            } catch (error) {
                createMessagePopup("error", error);
            }
        }
    });
})