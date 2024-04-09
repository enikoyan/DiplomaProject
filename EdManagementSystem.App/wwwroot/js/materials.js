// Variables
const filterSelector = document.querySelector('[name="filterSelector"]');
const filterSelectorMaterial = document.querySelector('[name="filterSelectorMaterial"]');
const courseSelector = document.querySelector('.search-row__filter_courses');
const groupSelector = document.querySelector('.search-row__filter_student-groups');
const searchBtn = document.querySelector('.search-row__btn_search');
const materialsContainer = document.querySelector('.found-materials');
const optionsContainerCourses = document.getElementById('options-container-courses');
const optionsContainerSquads = document.getElementById('options-container-squads');
let selectedFilter = 0;

document.addEventListener('DOMContentLoaded', function () {
    filterSelector.value = localStorage.getItem('selectedFilter');
    changeFilter();
    switch (filterSelector.value) {
        case "searchBySquads": {
            groupSelector.value = localStorage.getItem('groupSelectorValue');
        }
        case "searchByCourses": {
            courseSelector.value = localStorage.getItem('courseSelectorValue');
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
    }
    else if (filterSelectorMaterial.value === "searchByCourses") {
        optionsContainerCourses.style.display = "flex";
        optionsContainerSquads.style.display = "none";
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
    materialsItem_img.src = `/icons/file-type-icons/${item.type}-icon.svg`;

    // Material info
    const materialsItem_info = document.createElement("div");
    materialsItem_info.classList.add('found-materials__info');

    const materialsItem_info_text = document.createElement("div");
    materialsItem_info_text.classList.add('found-materials__info_text');
    const materialsItem_info__title = document.createElement("span");
    materialsItem_info__title.textContent = `${item.title}`;
    const materialsItem_info__addDate = document.createElement("span");
    materialsItem_info__addDate.textContent = `Дата добавления: ${item.dateAdded}`;
    materialsItem_info_text.appendChild(materialsItem_info__title);
    materialsItem_info_text.appendChild(materialsItem_info__addDate);

    materialsItem_info.appendChild(materialsItem_img);
    materialsItem_info.appendChild(materialsItem_info_text);

    // Download material button
    const materislItem_downloadBtn = document.createElement("button");
    materislItem_downloadBtn.textContent = "Скачать";
    materislItem_downloadBtn.setAttribute("data-fileId", `${item.materialId}`);

    materialsItem.appendChild(materialsItem_info);
    materialsItem.appendChild(materislItem_downloadBtn);

    materialsContainer.appendChild(materialsItem);
}

// Calling API
searchBtn.addEventListener('click', () => {
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
});

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