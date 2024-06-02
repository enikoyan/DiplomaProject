// Variables and controls
const weekInput = document.getElementById("weekInput");
const stepUpBtn = document.getElementById("nextWeekBtn");
const stepDownBtn = document.getElementById("previousWeekBtn");
const attendanceSelectVars = document.querySelectorAll(".attendance-popup__text");
const popup = document.querySelector(".attendance-popup");
const groupSelector = document.getElementById("group-select");
const attendanceBody = document.querySelector(".attendance-body");
const attendanceDate = document.getElementById("attendance-table-date");
const attendanceTable = document.querySelector(".custom-table-attendance");
const saveAsPdfBtn = document.getElementById("downloadPdfTable");
const saveAsExcelBtn = document.getElementById("downloadExcelTable");
const saveAttendanceBtn = document.getElementById("saveAttendanceBtn");
const sendAttendanceBtn = document.getElementById("sendAttendanceBtn");
const checkAttendanceInServerBtn = document.getElementById('checkAttendanceInServerBtn');
const filterTitle = document.querySelector(".custom-select__title");
const filterOptions = document.querySelectorAll(".custom-options__item");
var isTableTouched = false;
var isAttendanceSaved = true;
var tdFioValue = "";
var attendanceData = {};
const attendanceIconMap = {
    0: "attendance-empty",
    1: "attendance-true",
    2: "attendance-false",
};
const createAttendanceItemAPI = "http://localhost:8001/api/Attendances/CreateAttendanceItem";
const checkAttendanceItemAPI = "http://localhost:8001/api/Attendances/CreateAttendanceMatrix";

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

function confirmPopup(messageText) {
    return new Promise((resolve, reject) => {
        try {
            document.querySelector('.custom-alert').remove();
        }
        catch {

        }

        const popupElement = document.createElement('div');
        const popupHTML =
            `<div class="custom-alert custom-alert_confirm">
            <div>
                <img class="custom-alert__icon" src="../icons/message-icons/info-message.svg" data-status:"info"/>
                <p class="custom-alert__message">${messageText}</p>
            </div>
            <div>
                <button type="button" class="custom-btn_secondary" data-returnValue="yes">Да</button>
                <button type="button" class="custom-btn_secondary" data-returnValue="no">Нет</button>
            </div>
        </div>`;

        popupElement.innerHTML = popupHTML;
        document.body.appendChild(popupElement);

        popupElement.querySelector('[data-returnValue="yes"]').addEventListener('click', function () {
            popupElement.remove();
            resolve(true);
        });

        popupElement.querySelector('[data-returnValue="no"]').addEventListener('click', function () {
            popupElement.remove();
            resolve(false);
        });
    });
}

async function destroyMessagePopup() {
    const popup = document.querySelector('.custom-alert');
    popup.classList.add('hide');

    setTimeout(() => {
        popup.remove();
    }, 500);
}

document.addEventListener('DOMContentLoaded', async () => {

    /* FILTER SELECTOR HANDLER */
    groupSelector.addEventListener('click', async () => {
        const children = Array.from(groupSelector.children);

        children[0].classList.toggle("custom-select__btn_active");
        children[1].classList.toggle("custom-options_disabled");
    });

    filterOptions.forEach(item => {
        item.addEventListener('click', async () => {
            const value = item.getAttribute("data-value");
            //const valueText = selectedOption.textContent;
            const customSelect = item.closest(".custom-select");
            const customSelectTitle = customSelect.querySelector(".custom-select__title");

            customSelectTitle.textContent = item.textContent;
            customSelect.setAttribute("data-target", value);

            await createTableOfStudents();
            localStorage.setItem('attendance_selectedWeek', weekInput.value);
        });
    })

    // Check selected squad
    await checkSelectedSquad();
    // Check selected date
    await checkSelectedWeek();

    /* METHODS */

    // WEEK INPUT
    async function checkSelectedSquad() {
        if (localStorage.getItem("attendance_selectedSquad")) {
            groupSelector.setAttribute('data-target', localStorage.getItem("attendance_selectedSquad"));
            const targetValue = groupSelector.getAttribute('data-target');
            const targetOption = groupSelector.querySelector(`.custom-options__item[data-value="${targetValue}"]`);
            filterTitle.textContent = targetOption.textContent;

            await createTableOfStudents();
        }
    }
    async function checkSelectedWeek() {
        if (localStorage.getItem("attendance_selectedWeek")) {
            weekInput.value = localStorage.getItem("attendance_selectedWeek");
            await changeAttendanceTableDate();

            if (localStorage.getItem(`attendanceDATA_${groupSelector.getAttribute('data-target')}_${weekInput.value}`)) {
                await fillTableOfStudents();
            }
        }
    }
    async function changeAttendanceTableDate() {
        const selectedValue = weekInput.value;
        const weekNumber = selectedValue.split("-W")[1];
        attendanceDate.textContent = `Неделя: ${weekNumber}`;
    }

    // Attendance TABLE
    async function createTableOfStudents() {
        let count = 0;
        let selectedSquad = groupSelector.getAttribute('data-target');

        // Get students list
        const studentsList = JSON.parse(localStorage.getItem("studentsList"));

        // Get students list of selected squad
        const selectedSquadStudentsList = studentsList.find(
            (item) => item.squadName === selectedSquad
        ).students;

        if (selectedSquadStudentsList !== null) {
            attendanceBody.innerHTML = "";
            localStorage.setItem("attendance_selectedSquad", selectedSquad);
            selectedSquadStudentsList.forEach(async (item) => {
                var tRow = document.createElement("tr");
                tRow.innerHTML = `<td class="attendance-td">${count}</td>
                     <td class="attendance-td"> ${item.fio}</td>
                `;

                count++;

                for (let i = 0; i < 7; i++) {
                    tRow.innerHTML += `<td>
                            <button class="attendance-btn">
                                <img class="attendance-icon" src="/icons/attendance-icons/${attendanceIconMap[0]}.svg" data-status="${attendanceIconMap[0]}">
                            </button>
                        </td>`;
                }

                attendanceBody.appendChild(tRow);
            });

            sendAttendanceBtn.disabled = false;
            saveAsPdfBtn.disabled = false;
            saveAsExcelBtn.disabled = false;
        }
        else {
            await createMessagePopup("warning", "Студенты не найдены!");
        }

        await attendanceBtnHandler();
    }
    async function fillTableOfStudents() {
        try {
            const storedData = JSON.parse(
                localStorage.getItem(
                    `attendanceDATA_${groupSelector.getAttribute('data-target')}_${weekInput.value}`
                )
            );
            if (storedData != null) {
                const rows = document.querySelectorAll(".attendance-body tr");
                rows.forEach((row) => {
                    const studentId = row.querySelector(".attendance-td").textContent;
                    const statusCells = row.querySelectorAll("td .attendance-icon");
                    if (storedData[studentId]) {
                        statusCells.forEach((cell, index) => {
                            cell.setAttribute("data-status", storedData[studentId][index]);
                            cell.setAttribute(
                                "src",
                                `../icons/attendance-icons/${storedData[studentId][index]}.svg`
                            );
                        });
                    }
                });
                localStorage.setItem("attendance_selectedWeek", weekInput.value);
            }
            else {
                await createMessagePopup("warning", "Информация не найдена!");
                await changeAttendanceTableDate();
                await createTableOfStudents();
            }
        }
        catch {
            console.log("Ошибка!", DOMException);
        }
    }
    async function changeTableTouchedStatus() {
        isTableTouched = true;
        isAttendanceSaved = false;
        saveAsPdfBtn.disabled = false;
        saveAsExcelBtn.disabled = false;
        saveAttendanceBtn.disabled = false;
    }
    async function saveAttendanceLocally() {
        try {
            const rows = document.querySelectorAll(".attendance-body tr");
            rows.forEach((row) => {
                const studentId = row.querySelector(".attendance-td").textContent;
                const statusCells = row.querySelectorAll("td .attendance-icon");
                const statusValues = Array.from(statusCells).map((cell) =>
                    cell.getAttribute("data-status")
                );
                attendanceData[studentId] = statusValues;
            });

            localStorage.setItem(
                `attendanceDATA_${groupSelector.getAttribute('data-target')}_${weekInput.value}`,
                JSON.stringify(attendanceData)
            );

            localStorage.setItem("attendance_selectedWeek", weekInput.value);

            return true;
        }
        catch {
            return false;
        }
    }
    async function createExcelAttendance(table) {
        const wb = XLSX.utils.table_to_book(table);

        wb.SheetNames.forEach((sheetName) => {
            const ws = wb.Sheets[sheetName];
            const range = XLSX.utils.decode_range(ws["!ref"]);
            range.e.r += 2;

            const columnWidths = [];
            for (let col = range.s.c; col <= range.e.c; col++) {
                let maxLength = 0;
                for (let row = range.s.r; row <= range.e.r; row++) {
                    const cellAddress = XLSX.utils.encode_cell({ r: row, c: col });
                    if (ws[cellAddress] && ws[cellAddress].v) {
                        const cellLength = String(ws[cellAddress].v).length;
                        maxLength = Math.max(maxLength, cellLength + 2);
                    }
                }
                columnWidths[col] = { wch: maxLength };
            }
            ws["!cols"] = columnWidths;

            ws["!ref"] = XLSX.utils.encode_range(range);
        });

        wb.sheetName = "PIU";

        const dateStart = (await getWeekBorders(weekInput.value)).firstDay;
        const dateEnd = (await getWeekBorders(weekInput.value)).lastDay;

        XLSX.writeFile(
            wb, `Посещаемость ${filterTitle.textContent} (${dateStart}-${dateEnd}).xlsx`
        );
    }
    async function createPdfAttendance(table) {
        var html = table.outerHTML;

        var val = htmlToPdfmake(html);
        var dd = {
            content: val,
        };

        const dateStart = (await getWeekBorders(weekInput.value)).firstDay;
        const dateEnd = (await getWeekBorders(weekInput.value)).lastDay;

        pdfMake
            .createPdf(dd)
            .download(
                `Посещаемость ${filterTitle.textContent} (${dateStart}-${dateEnd}).pdf`
            );
    }
    async function createAttendanceMatrix() {
        const originalTable = document.querySelector(".custom-table-attendance");
        const clonedTable = originalTable.cloneNode(true);

        const rows = clonedTable.querySelectorAll("tbody tr");
        rows.forEach((row) => {
            const imgCells = row.querySelectorAll(".attendance-icon");
            imgCells.forEach((imgCell) => {
                const cell = imgCell.closest("td");
                const status = imgCell.getAttribute("data-status");
                const letter = getCellLetter(status);
                cell.textContent = letter;
            });
        });

        return clonedTable;
    }
    function getCellLetter(status) {
        switch (status) {
            case "attendance-false":
                return "н";
            case "attendance-ill":
                return "б";
            case "attendance-true":
                return "+"
            default:
                return "";
        }
    }

    // Attendance BUTTON
    async function attendanceBtnHandler() {
        const attendanceBtn = document.querySelectorAll(".attendance-btn");

        // Select in the table
        attendanceBtn.forEach((item) => {
            item.addEventListener("click", async (e) => {
                const buttonRect = item.getBoundingClientRect();
                const popupWidth = popup.offsetWidth;
                const popupHeight = popup.offsetHeight;
                const offsetX = (popupWidth - buttonRect.width) / 2;
                const offsetY = 5;
                const popupLeft = buttonRect.left - offsetX;
                const popupTop = buttonRect.bottom + offsetY;

                //popup.style.position = "fixed";
                popup.style.top = `${popupTop}px`;
                popup.style.left = `${popupLeft}px`;
                popup.style.visibility = "visible";

                // Удаляем обработчики событий для attendanceSelectVars
                attendanceSelectVars.forEach((variant) => {
                    variant.removeEventListener("click", variant.clickHandler);
                });

                // Добавляем новые обработчики событий для attendanceSelectVars
                attendanceSelectVars.forEach((variant) => {
                    variant.clickHandler = async () => {
                        await changeTableTouchedStatus();
                        const itemId = variant.id;
                        item.children[0].setAttribute(
                            "src",
                            `../icons/attendance-icons/${itemId}.svg`
                        );
                        item.children[0].setAttribute("data-status", `${itemId}`);
                        popup.style.visibility = "collapse";
                    };
                    variant.addEventListener("click", variant.clickHandler);
                });
            });
        });
    }
    saveAsExcelBtn.addEventListener("click", async () => {
        var table = await createAttendanceMatrix();
        await createExcelAttendance(table);
    });
    saveAsPdfBtn.addEventListener("click", async () => {
        var table = await createAttendanceMatrix();
        await createPdfAttendance(table);
    });

    // Get weekStart and weekEnd from weekInput
    async function getWeekBorders(weekValue) {
        // Split the line into the year and the week number
        var year = parseInt(weekValue.substring(0, 4));
        var weekNumber = parseInt(weekValue.substring(6));

        // Creating a new Date object with the first day of the week
        var firstDay = new Date(year, 0, (weekNumber - 1) * 7 + 2);

        // Get the last day of the week by adding 6 days to the first day
        var lastDay = new Date(firstDay);
        lastDay.setDate(lastDay.getDate() + 6);

        return {
            firstDay: firstDay.toISOString().split("T")[0],
            lastDay: lastDay.toISOString().split("T")[0],
        };
    }
    async function confirmAttendance() {
        if (confirm("Сохранить изменения?")) {
            if (await saveAttendanceLocally()) {
                await createMessagePopup("Посещаемость успешно сохранена!");
                saveAttendanceBtn.disabled = true;
                sendAttendanceBtn.disabled = false;
            }
        }
        isAttendanceSaved = true;
        saveAttendanceBtn.disabled = true;
    }

    /* LISTENERS */
    stepUpBtn.addEventListener("click", async () => {
        if (!isAttendanceSaved) {
            await confirmAttendance();
        }
        await weekInput.stepUp();
        await changeAttendanceTableDate();
        await createTableOfStudents();
        await fillTableOfStudents();
    });
    stepDownBtn.addEventListener("click", async () => {
        if (!isAttendanceSaved) {
            await confirmAttendance();
        }
        await weekInput.stepDown();
        await changeAttendanceTableDate();
        await createTableOfStudents();
        await fillTableOfStudents();
    });
    weekInput.addEventListener("change", async () => {
        await changeAttendanceTableDate();
        await createTableOfStudents();
        await fillTableOfStudents();
    });
    saveAttendanceBtn.addEventListener('click', async () => {
        // SAVE LOCALLY
        if (await saveAttendanceLocally()) {
            await createMessagePopup("success", "Посещаемость успешно сохранена!");
            isAttendanceSaved = true;
            saveAttendanceBtn.disabled = true;
            sendAttendanceBtn.disabled = false;
        }
    });
    refreshScheduleBtn.addEventListener('click', async () => {
        localStorage.removeItem("studentsList");

        fetch(`${location.href}/removeCache`, { method: "POST" })
            .then(response => {
                if (response.ok) {
                    localStorage.removeItem('studentsList');
                    location.reload(true);
                }
            })
            .catch(error => {
                console.log(error);
            });
    })
    sendAttendanceBtn.addEventListener('click', async () => {
        var table = await createAttendanceMatrix();
        await sendExcelAttendanceToServer(table);
    });
    checkAttendanceInServerBtn.addEventListener('click', async () => await loadExcelAttendanceFromServer());

    // SERVER SIDE
    async function getExcelAttendanceForServer(table) {
        const wb = XLSX.utils.table_to_book(table);

        wb.SheetNames.forEach((sheetName) => {
            const ws = wb.Sheets[sheetName];
            const range = XLSX.utils.decode_range(ws["!ref"]);
            range.e.r += 2;

            const columnWidths = [];
            for (let col = range.s.c; col <= range.e.c; col++) {
                let maxLength = 0;
                for (let row = range.s.r; row <= range.e.r; row++) {
                    const cellAddress = XLSX.utils.encode_cell({ r: row, c: col });
                    if (ws[cellAddress] && ws[cellAddress].v) {
                        const cellLength = String(ws[cellAddress].v).length;
                        maxLength = Math.max(maxLength, cellLength + 2);
                    }
                }
                columnWidths[col] = { wch: maxLength };
            }
            ws["!cols"] = columnWidths;

            ws["!ref"] = XLSX.utils.encode_range(range);
        });

        wb.sheetName = "PIU";

        const dateStart = (await getWeekBorders(weekInput.value)).firstDay;
        const dateEnd = (await getWeekBorders(weekInput.value)).lastDay;

        const wbArrayBuffer = XLSX.write(wb, { bookType: 'xlsx', type: 'array' });

        const fileName = `Посещаемость ${filterTitle.textContent} (${dateStart}-${dateEnd}).xlsx`;

        return {
            fileName: fileName,
            fileData: new Blob([wbArrayBuffer], { type: 'application/octet-stream' })
        };
    }
    async function sendExcelAttendanceToServer(table) {
        if (isAttendanceSaved) {
            setTimeout(function () {
                preloader.classList.remove("preloader_hidden");
                lottieLogo.classList.remove("logoLottie_hidden");
                animation.play();
            }, 400);

            // Упаковка данных для отправки
            var attendanceFile = await getExcelAttendanceForServer(table);
            let dataSend = new FormData();
            dataSend.append("squadName", localStorage.getItem('attendance_selectedSquad'));
            dataSend.append("weekDate", localStorage.getItem('attendance_selectedWeek'));
            dataSend.append("attendanceFile", attendanceFile.fileData, attendanceFile.fileName);

            // Отправка на сервер
            fetch(createAttendanceItemAPI, {
                method: 'POST',
                body: dataSend
            })
                .then(async response => {
                    if (!response.ok) {
                        await createMessagePopup("error", "Не удалось отправить файл!");
                    }
                    return response.text();
                })
                .then(async data => {
                    if (data) {
                        setTimeout(function () {
                            preloader.classList.add("preloader_hidden");
                            lottieLogo.classList.add("logoLottie_hidden");
                            animation.pause();
                        }, 400);
                        await createMessagePopup("info", "Посещаемость успешно отправлена на сервер!");
                    }
                })
                .catch(async error => {
                    await createMessagePopup("error", error);
                });
        }
        else {
            await createMessagePopup("warning", "Сохраните изменения перед отправкой!");
        }
    }
    async function loadExcelAttendanceFromServer() {
        // Get current attendance matrix from server
        let dataSend = new FormData();
        dataSend.append("squadName", groupSelector.getAttribute('data-target'));
        dataSend.append("weekDate", weekInput.value);

        fetch(checkAttendanceItemAPI, {
            method: 'POST',
            body: dataSend
        })
            .then(response => {
                if (!response.ok) {
                    console.log('Не удалось получить информацию! Отправьте на сервер хотя бы раз!');
                }
                return response.json();
            })
            .then(async data => {
                if (data) {
                    let answer = await confirmPopup("Информация на сервере присутствует, хотите загрузить её?");
                    if (answer == true) {
                        localStorage.setItem(`attendanceDATA_${groupSelector.getAttribute('data-target')}_${weekInput.value}`, JSON.stringify(data));
                        await createMessagePopup("info", "Посещаемость успешно обновлена!");
                        await fillTableOfStudents();
                    }
                }
                else await createMessagePopup("warning", "Информация на сервере отсутствует!");
            })
            .catch(async error => {
                await createMessagePopup("error", error);
            });
    }
});