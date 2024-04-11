// Variables
const filterSelector = document.querySelector('[name="filterSelector"]');
const filterSelectorMaterial = document.querySelector('[name="filterSelectorMaterial"]');
const courseSelector = document.querySelector('.search-row__filter_courses');
const groupSelector = document.querySelector('.search-row__filter_student-groups');
const searchBtn = document.querySelector('.search-row__btn_search');
const materialsContainer = document.querySelector('.found-materials');
const optionsContainerCourses = document.getElementById('options-container-courses');
const optionsContainerSquads = document.getElementById('options-container-squads');
const sendMaterialsBtn = document.getElementById('send-materials-btn');
let selectedFilter = 0;
let checkedOptions = "squads";
const createMaterialAPI = "https://localhost:44370/api/Materials/CreateMaterial/";
const downloadMaterialAPI = "https://localhost:44370/api/FileLoad/DownloadFile/";

document.addEventListener('DOMContentLoaded', function () {
    filterSelector.value = localStorage.getItem('selectedFilter');
    changeFilter();
    switch (filterSelector.value) {
        case "searchBySquads": {
            groupSelector.value = localStorage.getItem('groupSelectorValue');
            break;
        }
        case "searchByCourses": {
            courseSelector.value = localStorage.getItem('courseSelectorValue');
            break;
        }
    };

    const materialsData = JSON.parse(localStorage.getItem('materialsData'));

    if (materialsData) {
        // Clean container
        materialsContainer.innerHTML = '';

        materialsData.forEach(item => {
            createElements(item);
        });
    }
});

// Handler of changing filter event
filterSelector.addEventListener('change', function () { changeFilter() });

// Handler of changing filter event in materials modal screen
filterSelectorMaterial.addEventListener('change', function () { changeMaterialItems() });

function changeFilter() {
    if (filterSelector.value === "searchBySquads") {
        courseSelector.disabled = true;
        courseSelector.style.display = "none";
        groupSelector.disabled = false;
        groupSelector.style.display = "block";

        selectedFilter = 0;
    }
    else if (filterSelector.value === "searchByCourses") {
        groupSelector.disabled = true;
        groupSelector.style.display = "none";
        courseSelector.disabled = false;
        courseSelector.style.display = "block";

        selectedFilter = 1;
    }
    localStorage.setItem('selectedFilter', filterSelector.value);
}

function changeMaterialItems() {
    if (filterSelectorMaterial.value === "searchBySquads") {
        optionsContainerSquads.style.display = "flex";
        optionsContainerCourses.style.display = "none";
        checkedOptions = "squads";
    }
    else if (filterSelectorMaterial.value === "searchByCourses") {
        optionsContainerCourses.style.display = "flex";
        optionsContainerSquads.style.display = "none";
        checkedOptions = "courses";
    }
}

// Creating elements in the container
function createElements(item) {
    /* Create materialsItem for the container */
    const materialsItem = document.createElement("div");
    materialsItem.classList.add('found-materials__item');

    // Icon for the file type
    const materialsItem_img = document.createElement("img");
    materialsItem_img.alt = `${item.type}-icon`;
    materialsItem_img.classList.add('found-materials__icon');
    materialsItem_img.style = "width: 40px";
    materialsItem_img.setAttribute('asp-append-version', 'true');
    materialsItem_img.src = `/icons/file-type-icons/${item.type.slice(1)}-icon.svg`;

    // Material info
    const materialsItem_info = document.createElement("div");
    materialsItem_info.classList.add('found-materials__info');

    const materialsItem_info_text = document.createElement("div");
    materialsItem_info_text.classList.add('found-materials__info_text');
    const materialsItem_info__title = document.createElement("span");
    materialsItem_info__title.textContent = `${item.title}`;
    const materialsItem_info__addDate = document.createElement("span");
    const date = new Date(item.dateAdded);
    const options = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric', hour: 'numeric', minute: 'numeric', second: 'numeric' };
    materialsItem_info__addDate.textContent = 'Дата добавления: ' + date.toLocaleString('ru-RU', options);
    materialsItem_info_text.appendChild(materialsItem_info__title);
    materialsItem_info_text.appendChild(materialsItem_info__addDate);

    materialsItem_info.appendChild(materialsItem_img);
    materialsItem_info.appendChild(materialsItem_info_text);

    /* BUTTONS */
    const materialsItem_btnContainer = document.createElement("div");
    materialsItem_btnContainer.style = "display: flex; gap: 10px";

    // Download material button
    const materislItem_downloadBtn = document.createElement("button");
    materislItem_downloadBtn.textContent = "Скачать";
    materislItem_downloadBtn.setAttribute("data-fileId", `${item.materialId}`);
    materislItem_downloadBtn.addEventListener('click', () => {
        console.log("Download");
    });

    // Delete material button
    const materialsItem_deleteBtn = document.createElement("button");
    materialsItem_deleteBtn.textContent = "Удалить";
    materialsItem_deleteBtn.setAttribute("data-fileId", `${item.materialId}`);
    materialsItem_deleteBtn.addEventListener('click', () => {
        console.log("Delete");
    });

    materialsItem_btnContainer.appendChild(materislItem_downloadBtn);
    materialsItem_btnContainer.appendChild(materialsItem_deleteBtn);

    materialsItem.appendChild(materialsItem_info);
    materialsItem.appendChild(materialsItem_btnContainer);

    materialsContainer.appendChild(materialsItem);
}

// Calling API
searchBtn.addEventListener('click', () => {
    getMaterials();
});

function getMaterials() {
    let selectedAPI = "";
    let apiUrl = "";

    switch (selectedFilter) {
        case 0: {
            selectedAPI = `GetMaterialsBySquad/${groupSelector.value}`;
            break;
        }
        case 1: {
            selectedAPI = `GetMaterialsByCourse/${courseSelector.value}`;
            break;
        }
    }

    apiUrl = `https://localhost:44370/api/Materials/${selectedAPI}`;

    fetch(apiUrl)
        .then(response => {
            if (!response.ok) {
                throw new Error('Данных не найдено');
            }
            return response.json();
        })
        .then(data => {
            // Clean container
            materialsContainer.innerHTML = '';

            data.forEach(item => {
                createElements(item);
            });

            // Сохранение данных в локальное хранилище
            localStorage.setItem('materialsData', JSON.stringify(data));
            localStorage.setItem('groupSelectorValue', groupSelector.value);
            localStorage.setItem('courseSelectorValue', courseSelector.value);
        })
        .catch(error => {
            alert(error.message);
        })
}

/* ModalScreen */
const addMaterialModalScreen = document.getElementById("modal-addMaterial");
const shadowBg = document.querySelector(".overlay");

function displayModal() {
    shadowBg.style.display = "block";
    addMaterialModalScreen.style.display = "flex";
}

var addMaterialBtn = document.getElementById("add-material-btn");

addMaterialBtn.addEventListener("click", function () {
    displayModal();
});

const btnClose = document.getElementById("btn-close");

btnClose.addEventListener("click", () => {
    shadowBg.style.display = "none";
    addMaterialModalScreen.style.display = "none";
});

/* API calling */
document.getElementById('addMaterialForm').addEventListener('submit', (event) => {
    event.preventDefault();

    // Selection items check
    const selectedItems = getSelectedItems();

    if (selectedItems != null && document.querySelector('.add-file-input').files.length > 0) {

        const formData = new FormData();

        // Adding files
        const files = document.querySelector('.add-file-input').files;
        for (let i = 0; i < files.length; i++) {
            formData.append('files', files[i]);
        }

        // Arguments
        formData.append('groupBy', filterSelectorMaterial.value);
        for (let i = 0; i < selectedItems.length; i++) {
            formData.append('foreignKeys', selectedItems[i])
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
                    throw new Error('Ошибка при вызове метода');
                }
            })
            .then(data => {
                alert(data);
                location.reload();
            })
            .catch(error => {
                alert(error);
            });
    }
});

function getSelectedItems() {
    // Get all checkboxes inside the container
    const checkboxes = document.querySelectorAll(`#options-container-${checkedOptions} input[type="checkbox"]`);
    const selectedValues = [];

    // Add selected items inside the array
    checkboxes.forEach((checkbox) => {
        if (checkbox.checked) {
            selectedValues.push(checkbox.value);
        }
    });

    // Check if any of items was selected
    if (selectedValues.length > 0) {
        return selectedValues;
    } else {
        alert('Выберите хотя бы одно значение');
    }
}