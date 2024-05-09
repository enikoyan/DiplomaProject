window.addEventListener('DOMContentLoaded', async () => {
    // Filter handler
    await filterHandler();

    // Option handler
    await optionHandler();
});

// Variables and controls
const filterSelector = document.querySelector('[name="filterSelector"]');
const filterSelectorHomework = document.querySelector('[name="filterSelectorHomework"]');
const courseSelector = document.querySelector('.search-row__filter_courses');
const groupSelector = document.querySelector('.search-row__filter_student-groups');
const searchBtn = document.querySelector('.search-row__btn_search');
const homeworksContainer = document.querySelector('.homeworks');
const optionsContainerCourses = document.getElementById('options-container-courses');
const optionsContainerSquads = document.getElementById('options-container-squads');
const homewrkDeadlineTb = document.getElementById('homewrkDeadlineTb');
let checkedOptions = "squads";
var isDeadlineExist = false;
const downloadApiUrl = "https://localhost:44370/api/FileManagement/DownloadFileFromDB";
const getHomeworkUrl = "https://localhost:44370/api/Homeworks/GetHomeworksBy";
const downloadAllApiUrl = "https://localhost:44370/api/Homeworks/DownloadHomeworks";
const createHomeworkApiUrl = "https://localhost:44370/api/Homeworks/CreateHomework";

async function filterHandler() {
    if (localStorage.getItem('selectedFilter')) filterSelector.value = localStorage.getItem('selectedFilter');
    await changeFilter();
    switch (filterSelector.value) {
        case "searchBySquads": {
            if (localStorage.getItem('groupSelectorValue')) groupSelector.value = localStorage.getItem('groupSelectorValue');
            break;
        }
        case "searchByCourses": {
            if (localStorage.getItem('courseSelectorValue')) courseSelector.value = localStorage.getItem('courseSelectorValue');
            break;
        }
    };
}

async function optionHandler() {
    switch (localStorage.getItem('selectedFilter')) {
        case "searchByCourses": {
            if (localStorage.getItem('selectedCourseOption')) {
                courseSelector.value = localStorage.getItem('selectedCourseOption');
                await createHomeworkItems(JSON.parse(localStorage.getItem(`homeworksData / Course / ${courseSelector.value}`)));
            }
            break;
        }
        case "searchBySquads": {
            if (localStorage.getItem('selectedSquadOption')) {
                groupSelector.value = localStorage.getItem('selectedSquadOption');
                await createHomeworkItems(JSON.parse(localStorage.getItem(`homeworksData / Squad / ${groupSelector.value}`)));
                break;
            }
        }
    }
}

// Handler of changing filter event
filterSelector.addEventListener('change', function () { changeFilter() });

// Search filter change handler
async function changeFilter() {
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

// Search button click handler
searchBtn.addEventListener('click', async () => searchHomeworks());

async function searchHomeworks() {
    switch (localStorage.getItem('selectedFilter')) {
        case "searchByCourses": {
            if (localStorage.getItem(`homeworksData / Course / ${courseSelector.value}`)) {
                const data = JSON.parse(localStorage.getItem(`homeworksData / Course / ${courseSelector.value}`));
                await createHomeworkItems(data);
            }
            else {
                await getHomeworksFromDB("Course", courseSelector.value);
            }
            localStorage.setItem('selectedCourseOption', courseSelector.value);
            break;
        }
        case "searchBySquads": {
            if (localStorage.getItem(`homeworksData / Squad / ${courseSelector.value}`)) {
                const data = JSON.parse(localStorage.getItem(`homeworksData / Squad / ${courseSelector.value}`));
                await createHomeworkItems(data);
            }
            else {
                await getHomeworksFromDB("Squad", groupSelector.value);
            }
            localStorage.setItem('selectedSquadOption', groupSelector.value);
            break;
        }
    }
}

// Create homeworks in the HTML
async function createHomeworkItems(homeworks) {
    homeworksContainer.innerHTML = '';

    homeworks.forEach((item) => {
        // Homework item container
        const homeworkItem = document.createElement('div');
        homeworkItem.className = "homeworks__item";
        homeworkItem.setAttribute('data-homeworkId', item.homework.homeworkId);

        // Homework item components
        const homeworkTitle = document.createElement('h3');
        homeworkTitle.textContent = item.homework.title;
        homeworkTitle.className = "homeworks_title";

        /* Info block */
        const homeworkInfo = document.createElement('div');
        homeworkInfo.className = "homeworks__info";

        // Description
        const homeworkDescription = document.createElement('p');
        homeworkDescription.textContent = `описание: ${item.homework.description}`;
        homeworkDescription.className = "homeworks__description";
        homeworkInfo.appendChild(homeworkDescription);

        // Dates
        homeworkInfo.innerHTML = `
            <span><date class="homeworks__date">Дата добавления: ${item.homework.dateAdded}</date></span>
            <span><date  class="homeworks__date">Крайний срок сдачи : ${item.homework.deadline ? item.homework.deadline : "отсутствует"}</date></span>
            `;

        // Note
        if (item.homework.note != "null") homeworkInfo.innerHTML += `<span class="homeworks__note">Примечание: ${item.homework.note}</span>`;

        /* Attached files block */
        const homeworkFilesContainer = document.createElement('div');
        homeworkFilesContainer.className = "homeworks__files";
        homeworkFilesContainer.innerHTML = "<h3>Прикрепленные файлы: </h3>";

        if (item.attachedFiles.length > 0) {
            for (let i = 0; i < item.attachedFiles.length; i++) {
                homeworkFilesContainer.innerHTML +=
                    `<a href="${downloadApiUrl}?fileId=${item.attachedFiles[i].id}&folderName=Homeworks" download="download">
                ${item.attachedFiles[i].title}
                </a>`;
            }

            homeworkFilesContainer.innerHTML += `<button class="custom-btn homeworks__download-all-btn">
            Скачать всё
        </button>`;
        }
        else
        {
            homeworkFilesContainer.innerHTML = "<h3>Прикрепленные файлы: отсутствуют</h3>";
        }

        // Adding components into item
        homeworkItem.appendChild(homeworkTitle);
        homeworkItem.appendChild(homeworkDescription);
        homeworkItem.appendChild(homeworkInfo);
        homeworkItem.appendChild(homeworkFilesContainer);

        // Adding homeworkItem into the homeworks container
        homeworksContainer.appendChild(homeworkItem);
    });

    // Download all files as archive
    const downloadAllBtn = document.querySelectorAll(".homeworks__download-all-btn");
    downloadAllBtn.forEach(item => {
        item.addEventListener('click', () => {
            const parentElement = item.closest(".homeworks__item");
            const homeworkId = parentElement.dataset.homeworkid;
            window.location.href = `${downloadAllApiUrl}?homeworkId=${homeworkId}`;
        });
    })
}

// Get homeworks from database
async function getHomeworksFromDB(apiEnd, attribute) {
    fetch(`${getHomeworkUrl}${apiEnd}/${attribute}`)
        .then(response => {
            if (!response.ok) {
                homeworksContainer.innerHTML = '';
                localStorage.removeItem(`homeworksData / ${apiEnd} / ${attribute}`);
                throw new Error('Данных не найдено');
            }
            return response.json();
        })
        .then(async data => {
            if (data.length > 0) {
                await createHomeworkItems(data);
                localStorage.setItem(`homeworksData / ${apiEnd} / ${attribute}`, JSON.stringify(data));
            }
            else {
                alert("Домашние задания не найдены!");
            }
        })
        .catch(error => {
            console.log(error.message);
        })
}

/* ModalScreen */
filterSelectorHomework.addEventListener('change', function () { changeHomeworkItems() });
function changeHomeworkItems() {
    if (filterSelectorHomework.value === "searchBySquads") {
        optionsContainerSquads.style.display = "flex";
        optionsContainerCourses.style.display = "none";
        checkedOptions = "squads";
    }
    else if (filterSelectorHomework.value === "searchByCourses") {
        optionsContainerCourses.style.display = "flex";
        optionsContainerSquads.style.display = "none";
        checkedOptions = "courses";
    }
}
const addHomeworkModalScreen = document.querySelector(".modal-addItem");
const shadowBg = document.querySelector(".overlay");
function displayModal() {
    shadowBg.style.display = "block";
    addHomeworkModalScreen.style.display = "flex";
}
var addHomeworkBtn = document.getElementById("add-homework-btn");
addHomeworkBtn.addEventListener("click", function () {
    displayModal();
});
const btnClose = document.getElementById("btn-close");
btnClose.addEventListener("click", () => {
    shadowBg.style.display = "none";
    addHomeworkModalScreen.style.display = "none";
});

// Homework deadline off
const homeworkOffDeadlineBtn = document.getElementById("homeworkOffDeadlineBtn");
homeworkOffDeadlineBtn.addEventListener('change', () => {
    if (homeworkOffDeadlineBtn.checked) {
        homewrkDeadlineTb.disabled = true;
        isDeadlineExist = true;
    }
    else {
        homewrkDeadlineTb.disabled = false;
        isDeadlineExist = true;
    }
})

/* API calling (create homework) */
document.getElementById('addHomeworkForm').addEventListener('submit', async (event) => {
    event.preventDefault();
    // Selection items check
    const selectedItems = await getSelectedItems();

    if (selectedItems != null) {

        const formData = new FormData();

        // Adding files
        const attachedFiles = document.querySelector('.add-file-input').files;
        if (attachedFiles.length == 0) {
            formData.append('files', null);
        }
        else {
            for (let i = 0; i < attachedFiles.length; i++) {
                formData.append('files', attachedFiles[i]);
            }
        }

        // Arguments
        formData.append('groupBy', filterSelectorHomework.value);
        switch (filterSelectorHomework.value) {
            case "searchByCourses": {
                for (let i = 0; i < selectedItems.length; i++) {
                    formData.append('foreignKeys', selectedItems[i]);
                    localStorage.removeItem(`homeworksData / Course / ${courseSelector.value}`);
                }
                break;
            }
            case "searchBySquads": {
                for (let i = 0; i < selectedItems.length; i++) {
                    formData.append('foreignKeys', selectedItems[i]);
                    localStorage.removeItem(`homeworksData / Squad / ${groupSelector.value}`);
                }
                break;
            }
        }

        formData.append('title', document.getElementById('homeworkTitleTb').value);
        formData.append('description', document.getElementById('homeworkDescTb').value);
        let noteValue = document.getElementById('homeworkNoteTb').value;
        formData.append('note', noteValue ? noteValue : null);
        let deadlineTb = document.getElementById('homewrkDeadlineTb');
        if (isDeadlineExist) {
            formData.append('deadline', deadlineTb.value);
        }

        // Query options
        const requestOptions = {
            method: 'POST',
            body: formData
        };

        // Calling API method
        fetch(createHomeworkApiUrl, requestOptions)
            .then(response => {
                if (!response.ok) {
                    return response.text().then(text => { alert(text) })
                }
                else {
                    return response.text();
                }
            })
            .then(async data => {
                if (data) {
                    alert("Домашнее задание успешно выдано!");
                    await searchHomeworks();
                }
            })
            .catch(error => {
                console.log(error);
            });
    }
});

async function getSelectedItems() {
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