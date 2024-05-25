﻿// Variables and controls
const filterSelector = document.getElementById('filter-select');
const filterSelectorHomework = document.getElementById('filter-select-adding');
const groupSelector = document.getElementById('group-select');
const courseSelector = document.getElementById('course-select');
const searchBtn = document.getElementById('search-btn');
const homeworksContainer = document.querySelector(".homeworks");
let checkedOptions = "squads";
var isDeadlineExist = false;
const getHomeworkUrl = "https://localhost:44370/api/Homeworks/GetHomeworksBy";
const downloadAllApiUrl = "https://localhost:44370/api/Homeworks/DownloadHomeworks";
const downloadApiUrl = "https://localhost:44370/api/FileManagement/DownloadFileFromDB";
const createHomeworkApiUrl = "https://localhost:44370/api/Homeworks/CreateHomework";

/* FILTER SELECTOR HANDLER */
async function selectOptionHandler(selectedOption, saveBool) {
    const value = selectedOption.getAttribute("data-value");
    //const valueText = selectedOption.textContent;
    const customSelect = selectedOption.closest(".custom-select");
    const customSelectTitle = customSelect.querySelector(".custom-select__title");

    customSelectTitle.textContent = selectedOption.textContent;
    customSelect.setAttribute("data-target", value);

    await switchSelects(value, saveBool);
}

async function switchSelects(value, saveBool) {
    switch (value) {
        case "searchBySquads": {
            courseSelector.disabled = true;
            courseSelector.style.display = "none";
            groupSelector.disabled = false;
            groupSelector.style.display = "flex";

            if (saveBool) {
                localStorage.setItem("homeworks_lastSelectFilter", "searchBySquads");
            }

            break;
        }
        case "searchByCourses": {
            groupSelector.disabled = true;
            groupSelector.style.display = "none";
            courseSelector.disabled = false;
            courseSelector.style.display = "flex";

            if (saveBool) {
                localStorage.setItem("homeworks_lastSelectFilter", "searchByCourses");
            }

            break;
        }
    }
}

async function selectHandler(container) {
    const children = Array.from(container.children);

    children[0].classList.toggle("custom-select__btn_active");
    children[1].classList.toggle("custom-options_disabled");
}

// Set last selected filter
async function setLastSelectedFilter(lastFilter) {
    filterSelector.setAttribute('data-target', lastFilter);
    switch (lastFilter) {
        case "searchBySquads": {
            filterSelector.querySelector(".custom-select__title").textContent = "По группам";
            try {
                let option = localStorage.getItem("selectedSquadOption").split(',');
                await setLastSelectedOption(groupSelector, option[0], option[1]);

            }
            catch {
               // continue;
            }
            break;
        }
        case "searchByCourses": {
            filterSelector.querySelector(".custom-select__title").textContent = "По курсам";
            try {
                let option = localStorage.getItem("selectedCourseOption").split(',');
                await setLastSelectedOption(courseSelector, option[0], option[1]);
            }
            catch {
                //continue;
            }

            break;
        }
    }
    await switchSelects(lastFilter);
    await searchHomeworks();
}

async function setLastSelectedOption(selectOjbect, option, optionValue) {
    selectOjbect.setAttribute('data-target', option);
    selectOjbect.querySelector(".custom-select__title").textContent = optionValue;
}

// SEARCH HANDLER
async function searchHomeworks() {
    let selectedFilter = localStorage.getItem('homeworks_lastSelectFilter');
    switch (selectedFilter) {
        case "searchByCourses": {
            let dataTarget = courseSelector.getAttribute("data-target");
            if (localStorage.getItem(`homeworksData / Course / ${dataTarget}`)) {
                const data = JSON.parse(
                    localStorage.getItem(
                        `homeworksData / Course / ${dataTarget}`
                    )
                );
                await createHomeworkItems(data);
            }
            else {
                await getHomeworksFromDB("Course", courseSelector.getAttribute('data-target'));
            }
            break;
        }
        case "searchBySquads": {
            if (localStorage.getItem(`homeworksData / Squad / ${groupSelector.getAttribute("data-target")}`)) {
                const data = JSON.parse(localStorage.getItem(`homeworksData / Squad / ${groupSelector.getAttribute("data-target")}`));
                await createHomeworkItems(data);
            }
            else {
                await getHomeworksFromDB("Squad", groupSelector.getAttribute("data-target"));
            }
            break;
        }
    }
}

// Get homeworks from database
async function getHomeworksFromDB(apiEnd, attribute) {
    fetch(`${getHomeworkUrl}${apiEnd}/${attribute}`)
        .then((response) => {
            if (!response.ok) {
                homeworksContainer.innerHTML = "";
                localStorage.removeItem(`homeworksData / ${apiEnd} / ${attribute}`);
                throw new Error("Данных не найдено");
            }
            return response.json();
        })
        .then(async (data) => {
            if (data.length > 0) {
                await createHomeworkItems(data);
                localStorage.setItem(
                    `homeworksData / ${apiEnd} / ${attribute}`,
                    JSON.stringify(data)
                );
            } else {
                homeworksContainer.innerHTML = "";
                homeworksContainer.setAttribute('data-isFilled', "false");
                alert("Домашние задания не найдены!");
            }
        })
        .catch((error) => {
            console.log(error.message);
        });
}

// Create homeworks in the HTML
async function createHomeworkItems(homeworks) {
    homeworksContainer.innerHTML = "";

    homeworks.forEach((item) => {
        // Homework item container
        const homeworkItem = document.createElement("div");
        homeworkItem.className = "homeworks__item";
        homeworkItem.setAttribute("data-homeworkId", item.homework.homeworkId);

        // Homework item components
        const homeworkTitle = document.createElement("h3");
        homeworkTitle.textContent = item.homework.title;
        homeworkTitle.className = "homeworks_title";

        /* Info block */
        const homeworkInfo = document.createElement("div");
        homeworkInfo.className = "homeworks__info";

        // Description
        const homeworkDescription = document.createElement("p");
        homeworkDescription.textContent = `описание: ${item.homework.description}`;
        homeworkDescription.className = "homeworks__description";
        homeworkInfo.appendChild(homeworkDescription);

        // Dates
        homeworkInfo.innerHTML = `
          <span><date class="homeworks__date">Дата добавления: ${item.homework.dateAdded
            }</date></span>
          <span><date  class="homeworks__date">Крайний срок сдачи : ${item.homework.deadline ? item.homework.deadline : "отсутствует"
            }</date></span>
          `;

        // Note
        if (item.homework.note != "null")
            homeworkInfo.innerHTML += `<span class="homeworks__note">Примечание: ${item.homework.note}</span>`;

        /* Attached files block */
        const homeworkFilesContainer = document.createElement("div");
        homeworkFilesContainer.className = "homeworks__files";
        homeworkFilesContainer.innerHTML = "<h3>Прикрепленные файлы: </h3>";

        if (item.attachedFiles.length > 0) {
            for (let i = 0; i < item.attachedFiles.length; i++) {
                homeworkFilesContainer.innerHTML += `<a href="${downloadApiUrl}?fileId=${item.attachedFiles[i].id}&folderName=Homeworks" download="download">
              ${item.attachedFiles[i].title}
              </a>`;
            }

            homeworkFilesContainer.innerHTML += `<button class="custom-btn homeworks__download-all-btn">
          Скачать всё
      </button>`;
        } else {
            homeworkFilesContainer.innerHTML =
                "<h3>Прикрепленные файлы: отсутствуют</h3>";
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
    const downloadAllBtn = document.querySelectorAll(
        ".homeworks__download-all-btn"
    );
    downloadAllBtn.forEach((item) => {
        item.addEventListener("click", () => {
            const parentElement = item.closest(".homeworks__item");
            const homeworkId = parentElement.dataset.homeworkid;
            window.location.href = `${downloadAllApiUrl}?homeworkId=${homeworkId}`;
        });
    });

    let selectedFilter = localStorage.getItem('homeworks_lastSelectFilter');

    switch (selectedFilter) {
        case "searchByCourses": {
            localStorage.setItem("selectedCourseOption",
                `${courseSelector.getAttribute('data-target')},${courseSelector.querySelector(".custom-select__title").textContent}`);
            break;
        }
        case "searchBySquads": {
            localStorage.setItem("selectedSquadOption",
                `${groupSelector.getAttribute('data-target')},${groupSelector.querySelector(".custom-select__title").textContent}`);
            break;
        }
    }
}

window.addEventListener('DOMContentLoaded', async () => {

    // First load
    let lastFilter = localStorage.getItem('homeworks_lastSelectFilter');
    await setLastSelectedFilter(lastFilter);

    // Search button click handler
    searchBtn.addEventListener("click", async () => searchHomeworks());
});