// Variabls and controls
const optionsContainerCourses = document.getElementById('options-container-courses');
const optionsContainerSquads = document.getElementById('options-container-squads');
const dropContainer = document.getElementById("dropcontainer");
const fileInput = document.getElementById('attachedFiles');
const materialForm = document.getElementById('addMaterialForm');
const filterSelector = document.getElementById('filter-select');
const goBackBtn = document.getElementById('go-back-btn');
const createMaterialAPI = "https://localhost:44370/api/Materials/CreateMaterial/";

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

async function selectOptionHandler(selectedOption) {
    const value = selectedOption.getAttribute("data-value");
    //const valueText = selectedOption.textContent;
    const customSelect = selectedOption.closest(".custom-select");
    const customSelectTitle = customSelect.querySelector(".custom-select__title");

    customSelectTitle.textContent = selectedOption.textContent;
    customSelect.setAttribute("data-target", value);

    await switchSelects(value);
}

async function switchSelects(value) {
    switch (value) {
        case "searchBySquads": {
            optionsContainerCourses.style.display = "none";
            optionsContainerSquads.style.display = "flex";

            break;
        }
        case "searchByCourses": {
            optionsContainerSquads.style.display = "none";
            optionsContainerCourses.style.display = "flex";

            break;
        }
    }
}

async function selectHandler(container) {
    const children = Array.from(container.children);

    children[0].classList.toggle("custom-select__btn_active");
    children[1].classList.toggle("custom-options_disabled");
}

window.addEventListener('DOMContentLoaded', async () => {

    // Form handler
    materialForm.addEventListener('submit', async (e) => {
        e.preventDefault();
        isTouched = true;

        // Verify if all the inputs are filled
        let isCheckPassed = await checkFilled();

        if (isCheckPassed) {
            await createMaterial();
        }
    });

    async function checkFilled() {
        const checkboxList = document.querySelectorAll('.custom-checkbox:checked');
        if (checkboxList.length == 0) {
            await createMessagePopup("warning", "Выберите хотя бы одну группу или курс!");
            return false;
        }
        else {
            if (fileInput.files.length == 0) {
                await createMessagePopup("warning", "Загрузите хотя бы один файл!");
                return false;
            }
            else {
                return true;
            }
        }
    }

    async function createMaterial() {
        setTimeout(function () {
            preloader.classList.remove("preloader_hidden");
            lottieLogo.classList.remove("logoLottie_hidden");
            animation.play();
        }, 400);

        const formData = new FormData();
        const checkedValues = [];

        // Adding files
        const files = fileInput.files;
        for (let i = 0; i < files.length; i++) {
            formData.append('files', files[i]);
        }

        // Arguments
        formData.append('groupBy', filterSelector.getAttribute('data-target'));
        switch (filterSelector.getAttribute('data-target')) {
            case "searchByCourses": {
                const checkedCheckboxes = optionsContainerCourses.querySelectorAll('input[type="checkbox"]:checked');

                checkedCheckboxes.forEach((checkbox) => {
                    checkedValues.push(checkbox.value);
                });
                formData.append("foreignKeys", checkedValues);
                break;
            }
            case "searchBySquads": {
                const checkedCheckboxes = optionsContainerSquads.querySelectorAll('input[type="checkbox"]:checked');
                const checkedValues = [];
                checkedCheckboxes.forEach((checkbox) => {
                    checkedValues.push(checkbox.value);
                });

                formData.append("foreignKeys", checkedValues);
                break;
            }
        }

        // Query options
        const requestOptions = {
            method: 'POST',
            body: formData
        };

        // Calling API method
        fetch(createMaterialAPI, requestOptions)
            .then(response => {
                if (response.ok) {
                    return response.text();
                } else {
                    return response.text();
                    //throw new Error('Ошибка при вызове метода');
                }
            })
            .then(async data => {

                setTimeout(function () {
                    preloader.classList.add("preloader_hidden");
                    lottieLogo.classList.add("logoLottie_hidden");
                    animation.pause();
                }, 400);

                switch (filterSelector.getAttribute('data-target')) {
                    case "searchByCourses": {
                        for (var i = 0; i < checkedValues.length; i++) {
                            localStorage.removeItem(`materialsData / Course / ${checkedValues[i]}`);
                        }
                        break;
                    }
                    case "searchBySquads": {
                        for (var i = 0; i < checkedValues.length; i++) {
                            localStorage.removeItem(`materialsData / Squad / ${checkedValues[i]}`);
                        }
                        break;
                    }
                }

                await createMessagePopup("info", data);
            })
            .catch(async error => {
                await createMessagePopup("error", error);
            });
    }

    goBackBtn.addEventListener('click', async () => {
        window.location.href = 'https://localhost:44354/dashboard/materials';
    });
});