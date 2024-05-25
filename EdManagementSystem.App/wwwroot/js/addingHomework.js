const radioBtn = document.getElementById('homeworkOffDeadlineBtn');
const optionsContainerCourses = document.getElementById('options-container-courses');
const optionsContainerSquads = document.getElementById('options-container-squads');
const dropContainer = document.getElementById("dropcontainer");
const fileInput = document.getElementById('attachedFiles');
const homeworkForm = document.getElementById('addHomeworkForm');
const homeworkTitleTb = document.getElementById('homeworkTitleTb');
const homeworkDescTb = document.getElementById('homeworkDescTb');
const homeworkDeadlineTb = document.getElementById('homeworkDeadlineTb');
const filterSelector = document.getElementById('filter-select');
const goBackBtn = document.getElementById('go-back-btn');
var isTouched = false;
var isDeadlineExist = true;
const createHomeworkApiUrl = "https://localhost:44370/api/Homeworks/CreateHomework";

radioBtn.addEventListener('click', function (event) {
    if (this.checked) {
        homeworkDeadlineTb.disabled = true;
        isDeadlineExist = false;
    } else {
        homeworkDeadlineTb.disabled = false;
        isDeadlineExist = true;
    }
});

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

document.addEventListener('DOMContentLoaded', function () {

    // File drug and drop
    dropContainer.addEventListener("dragover", (e) => {
        e.preventDefault()
    }, false)
    dropContainer.addEventListener("dragenter", () => {
        dropContainer.classList.add("drag-active");
    })
    dropContainer.addEventListener("dragleave", () => {
        dropContainer.classList.remove("drag-active");
    })
    dropContainer.addEventListener("drop", (e) => {
        e.preventDefault();
        dropContainer.classList.remove("drag-active");
        fileInput.files = e.dataTransfer.files;
    })

    // Form handler
    homeworkForm.addEventListener('submit', async (e) => {
        e.preventDefault();
        isTouched = true;

        // Verify if all the inputs are filled
        let isCheckPassed = await checkFilled();

        if (isCheckPassed) {
            homeworkTitleTb.classList.toggle('custom-input_correct');
            homeworkDescTb.classList.add('custom-input_correct');

            await createHomework();
        }
    });

    async function createHomework() {
        const formData = new FormData();

        // Adding files 
        const attachedFiles = fileInput.files;
        if (attachedFiles.length == 0) {
            formData.append("files", null);
        } else {
            for (let i = 0; i < attachedFiles.length; i++) {
                formData.append("files", attachedFiles[i]);
            }
        }

        // Arguments
        formData.append("groupBy", filterSelector.getAttribute('data-target'));
        switch (filterSelector.getAttribute('data-target')) {
            case "searchByCourses": {
                const checkedCheckboxes = optionsContainerCourses.querySelectorAll('input[type="checkbox"]:checked');
                const checkedValues = [];
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

        formData.append("title", homeworkTitleTb.value);
        formData.append("description", homeworkDescTb.value);

        let noteValue = document.getElementById("homeworkNoteTb").value;

        formData.append("note", noteValue ? noteValue : "");

        if (isDeadlineExist) {
            formData.append("deadline", homeworkDeadlineTb.value);
        }

        // Query options
        const requestOptions = {
            method: "POST",
            body: formData,
        };

        // Calling API method
        fetch(createHomeworkApiUrl, requestOptions)
            .then((response) => {
                if (!response.ok) {
                    return response.text().then((text) => {
                        alert(text);
                    });
                } else {
                    return response.text();
                }
            })
            .then(async (data) => {
                if (data) {
                    if (confirm("Домашнее задание успешно выдано! Хотите вернуться назад к списку Д/З?")) {
                        window.location.href = 'https://localhost:44354/dashboard/homeworks';
                    } else {
                        location.reload();
                    }
                }
            })
            .catch((error) => {
                console.log(error);
            });
    }

    async function checkFilled() {
        const checkboxList = document.querySelectorAll('.custom-checkbox:checked');
        if (checkboxList.length == 0) {
            alert("Выберите хотя бы одну группу или курс!");
            return false;
        }
        else {
            if (homeworkTitleTb.value.trim().length === 0) {
                homeworkTitleTb.classList.add('custom-input_errored');
                alert("Заполните обязательные поля (тема, описание)!");
                return false;
            }
            else if (homeworkDescTb.value.trim().length === 0) {
                homeworkTitleTb.classList.add('custom-input_correct');
                homeworkDescTb.classList.add('custom-input_errored');
                alert("Заполните обязательные поля (тема, описание)!");
                return false;
            }
            else if (homeworkDeadlineTb.value == "" && isDeadlineExist) {
                alert("Укажите срок сдачи!");
            }
            else {
                return true;
            }
        }
    }

    goBackBtn.addEventListener('click', async () => {
        window.location.href = 'https://localhost:44354/dashboard/homeworks';
    });
});

window.onbeforeunload = function () {
    if (!isTouched) {
        return "Данные не сохранены. Точно перейти?";
    }
}
