// Variables and controls
const filterSelector = document.getElementById('filter-select');
const filterSelectorHomework = document.getElementById('filter-select-adding');
const groupSelector = document.getElementById('group-select');
const courseSelector = document.getElementById('course-select');
const searchBtn = document.getElementById('search-btn');
const refreshBtn = document.getElementById('refresh-btn');
const homeworksContainer = document.querySelector(".homeworks");
const options = { year: 'numeric', month: 'long', day: 'numeric', hour: 'numeric', minute: 'numeric' };
let checkedOptions = "squads";
var isDeadlineExist = false;
const getHomeworkUrl = "http://localhost:8001/api/Homeworks/GetHomeworksBy";
const downloadAllApiUrl = "http://localhost:8001/api/Homeworks/DownloadHomeworks";
const downloadApiUrl = "http://localhost:8001/api/FileManagement/DownloadFileFromDB";
const createHomeworkApiUrl = "http://localhost:8001/api/Homeworks/CreateHomework";

// Preloader
var preloader = document.querySelector(".preloader");
var lottieLogo = document.getElementById("logoLottie");
var links = document.querySelectorAll("a");

/* FILTER SELECTOR HANDLER */
async function selectOptionHandler(selectedOption) {
    const value = selectedOption.getAttribute("data-value");
    //const valueText = selectedOption.textContent;
    const customSelect = selectedOption.closest(".custom-select");
    const customSelectTitle = customSelect.querySelector(".custom-select__title");

    customSelectTitle.textContent = selectedOption.textContent;
    customSelect.setAttribute("data-target", value);

    await switchSelects(value);
}

async function switchSelects(value, saveBool) {
    switch (value) {
        case "searchBySquads": {
            courseSelector.disabled = true;
            courseSelector.style.display = "none";
            groupSelector.disabled = false;
            groupSelector.style.display = "flex";

            break;
        }
        case "searchByCourses": {
            groupSelector.disabled = true;
            groupSelector.style.display = "none";
            courseSelector.disabled = false;
            courseSelector.style.display = "flex";

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
    let selectedFilter = filterSelector.getAttribute('data-target');

    if (selectedFilter == 'null' || selectedFilter == 'none') {
        await createMessagePopup("warning", "Выберите фильтр!");
    }
    else
    {
        switch (selectedFilter) {
            case "searchByCourses": {
                if (courseSelector.getAttribute('data-target') == 'none') {
                    await createMessagePopup("warning", "Выберите курс!");
                }
                else {
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
            }
            case "searchBySquads": {
                if (courseSelector.getAttribute('data-target') == 'none') {
                    await createMessagePopup("warning", "Выберите группу!");
                }
                else {
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
    }
}

// Get homeworks from database
async function getHomeworksFromDB(apiEnd, attribute) {
    setTimeout(function () {
        preloader.classList.remove("preloader_hidden");
        lottieLogo.classList.remove("logoLottie_hidden");
        animation.play();
    }, 400);

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
            setTimeout(function () {
                preloader.classList.add("preloader_hidden");
                lottieLogo.classList.add("logoLottie_hidden");
                animation.pause();
            }, 400);

            if (data.length > 0) {
                localStorage.setItem('homeworks_lastSelectFilter', filterSelector.getAttribute('data-target'));
                await createHomeworkItems(data);
                localStorage.setItem(
                    `homeworksData / ${apiEnd} / ${attribute}`,
                    JSON.stringify(data)
                );
                await createMessagePopup("success", "Домашние задания успешно найдены!");
            } else {
                homeworksContainer.innerHTML = "";
                homeworksContainer.setAttribute('data-isFilled', "false");
                await createMessagePopup("warning", "Домашние задания не найдены!");
            }
        })
        .catch(async error => {
            await createMessagePopup("error", error);
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
        homeworkTitle.className = "homeworks__title";

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
            <date class="homeworks__date">Дата добавления: ${new Date(item.homework.dateAdded).toLocaleDateString('ru-RU', options)}
            </date>
            <date class="homeworks__date">Крайний срок сдачи:
                ${item.homework.deadline ? new Date(item.homework.deadline).toLocaleDateString('ru-RU', options) : "отсутствует"}
            </date>`;

        // Note
        if (item.homework.note != null)
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

            homeworkFilesContainer.innerHTML += `<button class="custom-btn homeworks__download-all-btn">Скачать всё</button>`;
        }
        else
        {
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

window.addEventListener('DOMContentLoaded', async () => {

    // First load
    let lastFilter = localStorage.getItem('homeworks_lastSelectFilter');
    if (lastFilter !== null) {
        await setLastSelectedFilter(lastFilter);
    }

    // Search button click handler
    searchBtn.addEventListener("click", async () => await searchHomeworks());

    // Refresh data
    refreshBtn.addEventListener("click", async () => {
        switch (filterSelector.getAttribute('data-target')) {
            case "searchByCourses": {
                localStorage.removeItem(`homeworksData / Course / ${courseSelector.getAttribute('data-target')}`);
                break;
            }
            case "searchBySquads": {
                localStorage.removeItem(`homeworksData / Squad / ${groupSelector.getAttribute('data-target')}`);
                break;
            }
        }
        await searchHomeworks();

    });
});