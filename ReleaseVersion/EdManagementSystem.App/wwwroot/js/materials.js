// Variables and cotntrols
const filterSelector = document.getElementById('filter-select');
const filterSelectorHomework = document.getElementById('filter-select-adding');
const groupSelector = document.getElementById('group-select');
const courseSelector = document.getElementById('course-select');
const searchBtn = document.getElementById('search-btn');
const materialsContainer = document.querySelector('.found-materials');
const downloadMaterialAPI = "http://localhost:8001/api/Materials/DownloadMaterial";
const deleteMaterialAPI = "http://localhost:8001/api/Materials";

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

async function switchSelects(value) {
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
                let option = localStorage.getItem("materials_selectedSquadOption").split(',');
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
                let option = localStorage.getItem("materials_selectedCourseOption").split(',');
                await setLastSelectedOption(courseSelector, option[0], option[1]);
            }
            catch {
                //continue;
            }

            break;
        }
    }
    await switchSelects(lastFilter);
    await searchMaterials();
}

async function setLastSelectedOption(selectOjbect, option, optionValue) {
    selectOjbect.setAttribute('data-target', option);
    selectOjbect.querySelector(".custom-select__title").textContent = optionValue;
}

// SEARCH HANDLER
async function searchMaterials() {
    let selectedFilter = filterSelector.getAttribute('data-target');

    if (selectedFilter == "none") {
        await createMessagePopup("warning", "Выберите фильтр!");
    }

    else {
        switch (selectedFilter) {
            case "searchByCourses": {
                if (courseSelector.getAttribute('data-target') == "none") {
                    await createMessagePopup("warning", "Выберите курс!");
                }
                else {
                    let dataTarget = courseSelector.getAttribute("data-target");
                    if (localStorage.getItem(`materialsData / Course / ${dataTarget}`)) {
                        const data = JSON.parse(
                            localStorage.getItem(
                                `materialsData / Course / ${dataTarget}`));
                        await createMaterial(data);
                    }
                    else {
                        await getMaterialsFromDB();
                    }
                }
                break;
            }
            case "searchBySquads": {
                if (groupSelector.getAttribute('data-target') == "none") {
                    await createMessagePopup("warning", "Выберите группу!");
                }
                else {
                    if (localStorage.getItem(`materialsData / Squad / ${groupSelector.getAttribute("data-target")}`)) {
                        const data = JSON.parse(
                            localStorage.getItem(
                                `materialsData / Squad / ${groupSelector.getAttribute("data-target")}`));
                        await createMaterial(data);
                    }
                    else {
                        await getMaterialsFromDB();
                    }

                }
                break;
            }
        }
    }
}

// Creating elements in the container
function createMaterial(data) {
    data.forEach(item => {
        const elementsWithFileId = materialsContainer.querySelectorAll(`[data-fileid="${item.file.id}"]`);
        if (elementsWithFileId.length === 0) {
            /* Create materialsItem for the container */
            const materialsItem = document.createElement("div");
            materialsItem.classList.add('found-materials__item');

            // Icon for the file type
            const materialsItem_img = document.createElement("img");
            materialsItem_img.alt = `${item.file.type}-icon`;
            materialsItem_img.classList.add('found-materials__icon');
            materialsItem_img.style = "width: 40px";
            materialsItem_img.setAttribute('asp-append-version', 'true');
            materialsItem_img.src = `/icons/file-type-icons/${item.file.type.slice(1)}-icon.svg`;

            // Material info
            const materialsItem_info = document.createElement("div");
            materialsItem_info.classList.add('found-materials__info');

            const materialsItem_info_text = document.createElement("div");
            materialsItem_info_text.classList.add('found-materials__info_text');
            const materialsItem_info__title = document.createElement("span");
            materialsItem_info__title.textContent = `${item.file.title}`;
            const materialsItem_info__addDate = document.createElement("span");
            const date = new Date(item.file.dateAdded);
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
            materislItem_downloadBtn.classList.add('custom-btn_secondary');
            materislItem_downloadBtn.setAttribute("data-fileId", `${item.file.id}`);
            materislItem_downloadBtn.addEventListener('click', () => {
                const materialId = materislItem_downloadBtn.getAttribute("data-fileId");
                downloadMaterial(materialId);
            });

            // Delete material button
            const materialsItem_deleteBtn = document.createElement("button");
            materialsItem_deleteBtn.textContent = "Удалить";
            materialsItem_deleteBtn.classList.add('custom-btn_secondary');
            materialsItem_deleteBtn.setAttribute("data-fileId", `${item.file.id}`);
            materialsItem_deleteBtn.addEventListener('click', () => {
                var answer = window.confirm("Вы уверены, что хотите удалить данный материал?");
                if (answer) {
                    const materialId = materialsItem_deleteBtn.getAttribute("data-fileId");
                    deleteMaterial(materialId);
                }
            });

            materialsItem_btnContainer.appendChild(materislItem_downloadBtn);
            materialsItem_btnContainer.appendChild(materialsItem_deleteBtn);

            materialsItem.appendChild(materialsItem_info);
            materialsItem.appendChild(materialsItem_btnContainer);

            materialsContainer.appendChild(materialsItem);

            let selectedFilter = filterSelector.getAttribute('data-target');

            switch (selectedFilter) {
                case "searchByCourses": {
                    localStorage.setItem("materials_selectedCourseOption",
                        `${courseSelector.getAttribute('data-target')},${courseSelector.querySelector(".custom-select__title").textContent}`);
                    localStorage.setItem("materials_lastSelectFilter", "searchByCourses");
                    break;
                }
                case "searchBySquads": {
                    localStorage.setItem("materials_selectedSquadOption",
                        `${groupSelector.getAttribute('data-target')},${groupSelector.querySelector(".custom-select__title").textContent}`);
                    localStorage.setItem("materials_lastSelectFilter", "searchBySquads");
                    break;
                }
            }
        }
    });
}

function getMaterialsFromDB() {
    let selectedAPI = "";
    let apiUrl = "";
    var selectedFilter = filterSelector.getAttribute('data-target');

    switch (selectedFilter) {
        case "searchBySquads": {
            selectedAPI = `GetMaterialsBySquad/${groupSelector.getAttribute('data-target')}`;
            break;
        }
        case "searchByCourses": {
            selectedAPI = `GetMaterialsByCourse/${courseSelector.getAttribute('data-target')}`;
            break;
        }
    }

    apiUrl = `http://localhost:8001/api/Materials/${selectedAPI}`;

    fetch(apiUrl)
        .then(response => {
            if (!response.ok) {
                materialsContainer.innerHTML = '';
                throw new Error('Данных не найдено');
            }
            return response.json();
        })
        .then(async data => {
            // Clean container
            materialsContainer.innerHTML = '';

            await createMaterial(data);

            // Saving found data to local storage
            switch (selectedFilter) {
                case "searchByCourses": {
                    localStorage.setItem(`materialsData / Course / ${courseSelector.getAttribute("data-target")}`, JSON.stringify(data));
                    break;
                }
                case "searchBySquads": {
                    localStorage.setItem(`materialsData / Squad / ${groupSelector.getAttribute("data-target")}`, JSON.stringify(data));
                    break;
                }
            }

            await createMessagePopup("success", "Данные успешно найдены!");
        })
        .catch(async error => {
            await createMessagePopup("error", error);
        })
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

async function deleteMaterial(materialId) {
    try {
        let selectedFilter = localStorage.getItem('materials_lastSelectFilter');
        var apiUrl = "";

        switch (selectedFilter) {
            case "searchBySquads": {
                apiUrl = `${deleteMaterialAPI}/DeleteSquadMaterial/${materialId}/${groupSelector.getAttribute('data-target')}`;
                break;
            }
            case "searchByCourses": {
                apiUrl += `${deleteMaterialAPI}/DeleteCourseMaterial/${materialId}/${courseSelector.getAttribute('data-target')}`;
                break;
            }
        }

        const response = await fetch(`${apiUrl}`, {
            method: 'DELETE',
        })

        if (response.ok) {
            await createMessagePopup("info", "Материал успешно удален!");
            switch (selectedFilter) {
                case "searchByCourses": {
                    localStorage.removeItem(`materialsData / Course / ${courseSelector.getAttribute("data-target")}`);
                    break;
                }
                case "searchBySquads": {
                    localStorage.removeItem(`materialsData / Squad / ${groupSelector.getAttribute("data-target")}`);
                    break;
                }
            }

            location.reload();
        }
    } catch (error) {
        console.error(error);
    }
}

async function downloadMaterial(materialId) {
    try {
        const response = await fetch(`${downloadMaterialAPI}/${materialId}`, {
            method: 'GET',
        })

        if (response.ok) {
            // Find name of file
            const contentDisposition = response.headers.get('Content-Disposition');
            const encodedFilename = contentDisposition.match(/filename\*?=UTF-8'['']?([^;"]+)['']?/i);
            const encodedFilenameText = encodedFilename[1];
            // Decoding name
            const filename = decodeURIComponent(encodedFilenameText);
            if (filename) {
                const blob = await response.blob();
                const url = window.URL.createObjectURL(blob);
                const a = document.createElement('a');
                a.href = url;
                a.download = filename || "Файл";
                document.body.appendChild(a);
                a.click();
                window.URL.revokeObjectURL(url);
            } else {
                throw new Error('Название файла не найдено!');
            }
        } else {
            await createMessagePopup("warning", "Не удалось скачать файл!");
        }
    } catch (error) {
        console.error(error);
    }
}

window.addEventListener('DOMContentLoaded', async () => {

    // First load
    let lastFilter = localStorage.getItem('materials_lastSelectFilter');
    if (lastFilter != null) {
        await setLastSelectedFilter(lastFilter);
    }

    // Search button click handler
    searchBtn.addEventListener("click", async () => searchMaterials());

});