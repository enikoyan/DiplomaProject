// Variables and controls
const weekInput = document.getElementById("weekInput");
const stepUpBtn = document.getElementById("nextWeekBtn");
const stepDownBtn = document.getElementById("previousWeekBtn");
const attendanceSelectVars = document.querySelectorAll(
  ".attendance-popup__text"
);
const popup = document.querySelector(".attendance-popup");
const groupSelector = document.querySelector(
  ".search-row__filter_student-groups"
);
const searchBtn = document.querySelector(".search-row__btn_search");
const attendanceBody = document.querySelector(".attendance-body");
const attendanceDate = document.getElementById("attendance-table-date");
const attendanceTable = document.querySelector(".custom-table-attendance");
const saveAsPdfBtn = document.getElementById("downloadPdfTable");
const saveAttendanceBtn = document.getElementById("saveAttendanceBtn");
var isTableTouched = false;
var isAttendanceSaved = false;
var tdFioValue = "";
const attendanceIconMap = {
  0: "attendance-empty",
  1: "attendance-true",
  2: "attendance-false",
};

window.addEventListener("DOMContentLoaded", async () => {
  // Week input select
  if (localStorage.getItem("weekInputSelectedValue")) {
    weekInput.value = localStorage.getItem("weekInputSelectedValue");
    await changeAttendanceTableDate();
  }
  if (localStorage.getItem("selectedSquad")) {
    groupSelector.value = localStorage.getItem("selectedSquad");
  }

  await getAttendanceBeforePageLoad();

  // Search btn handler
  searchBtn.addEventListener("click", async () => {
    // Make a list of students
    await makeListOfStudents(groupSelector.value);
    await attendanceBtnHandler();
  });

  async function getAttendanceBeforePageLoad() {
    try {
      await makeListOfStudents(groupSelector.value);
      await attendanceBtnHandler();
    } catch {
      console.log("Информации нет!");
    }
  }

  async function attendanceBtnHandler() {
    const attendanceBtn = document.querySelectorAll(".attendance-btn");

    // Select in the table
    attendanceBtn.forEach((item) => {
      item.addEventListener("click", (e) => {
        isTableTouched = true;
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
          variant.clickHandler = () => {
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

  async function makeListOfStudents(selectedSquad) {
    attendanceBody.innerHTML = "";
    let count = 0;

    const studentsList = JSON.parse(localStorage.getItem("studentsList"));

    const selectedSquadStudentsList = studentsList.find(
      (item) => item.squadName === selectedSquad
    ).students;

    if (selectedSquadStudentsList !== null) {
      selectedSquadStudentsList.forEach((item) => {
        count++;
        var tRow = document.createElement("tr");
        tRow.innerHTML = `<td class="attendance-td">${count}</td>
                     <td class="attendance-td"> ${item.fio}</td>
                `;

        for (let i = 0; i < 7; i++) {
          tRow.innerHTML += `<td>
                            <button class="attendance-btn">
                                <img class="attendance-icon" src="/icons/attendance-icons/${attendanceIconMap[0]}.svg" data-status="${attendanceIconMap[0]}">
                            </button>
                        </td>`;
        }

        attendanceBody.appendChild(tRow);
      });

      localStorage.setItem("selectedSquad", groupSelector.value);
    } else alert("Студенты не найдены!");
  }

  // Download as Excel
  document
    .getElementById("downloadExcelTable")
    .addEventListener("click", async () => {
      var table = await createAttendanceMatrix();
      await createExcelAttendance(table);
    });

  // Download as Pdf
  saveAsPdfBtn.addEventListener("click", async () => {
    var table = await createAttendanceMatrix();
    await createPdfAttendance(table);
  });

  function getCellLetter(status) {
    switch (status) {
      case "attendance-false":
        return "н";
      case "attendance-ill":
        return "б";
      default:
        return "";
    }
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
      wb,
      `Посещаемость ${
        groupSelector.options[groupSelector.selectedIndex].text
      } (${dateStart}-${dateEnd}).xlsx`
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
        `Посещаемость ${
          groupSelector.options[groupSelector.selectedIndex].text
        } (${dateStart}-${dateEnd}).pdf`
      );
  }

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

  // WeekInput select handler
  weekInput.addEventListener("change", async () => {
    await checkIsTableToucher();
    await changeAttendanceTableDate();
    await getAttendanceLocally();
  });

  stepUpBtn.addEventListener("click", async () => {
    await weekInput.stepUp();
    localStorage.setItem("weekInputSelectedValue", weekInput.value);
    await changeAttendanceTableDate();
    await checkIsTableToucher();
    await getAttendanceLocally();
  });

  stepDownBtn.addEventListener("click", async () => {
    await weekInput.stepDown();
    localStorage.setItem("weekInputSelectedValue", weekInput.value);
    await changeAttendanceTableDate();
    await checkIsTableToucher();
    await getAttendanceLocally();
  });

  async function changeAttendanceTableDate() {
    const selectedValue = weekInput.value;
    const weekNumber = selectedValue.split("-W")[1];
    attendanceDate.textContent = `Неделя: ${weekNumber}`;
  }

  async function checkIsTableToucher() {
    if (isTableTouched && !isAttendanceSaved) {
      if (confirm("Сохранить изменения?")) {
        // SAVE ATTENDANCE
        await saveAttendanceLocally();
        isTableTouched = false;
        return true;
      } else {
        await getAttendanceBeforePageLoad();
        isTableTouched = false;
        return false;
      }
    }
  }

  // Объект для хранения данных посещаемости
  var attendanceData = {};

  // Save attendance locally
  async function saveAttendanceLocally() {
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
      `attendanceDATA_${groupSelector.value}_${weekInput.value}`,
      JSON.stringify(attendanceData)
    );
  }

  async function getAttendanceLocally() {
    try {
      const storedData = JSON.parse(
        localStorage.getItem(
          `attendanceDATA_${groupSelector.value}_${weekInput.value}`
        )
      );
      if (storedData == null || storedData.count == 0) {
        //console.log("Информация не найдена!");
        await getAttendanceBeforePageLoad();
      } else {
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
      }
    } catch {
      console.log("Информация не найдена!");
    }
  }

  // Save attendance in the server
  saveAttendanceBtn.addEventListener("click", async () => {
    console.log("SAVE ATTENDANCE");
  });
});
