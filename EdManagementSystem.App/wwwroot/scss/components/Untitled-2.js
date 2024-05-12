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
const saveAsExcelBtn = document.getElementById("downloadExcelTable");
const saveAttendanceBtn = document.getElementById("saveAttendanceBtn");
var isTableTouched = false;
var isAttendanceSaved = false;
var tdFioValue = "";
var attendanceData = {};
const attendanceIconMap = {
  0: "attendance-empty",
  1: "attendance-true",
  2: "attendance-false",
};

/* METHODS */
async function checkSelectedSquad() {
  if (localStorage.getItem("attendance_selectedSquad")) {
    groupSelector.value = localStorage.getItem("attendance_selectedSquad");
  }
}
async function checkSelectedWeek() {
  if (localStorage.getItem("attendance_selectedWeek")) {
    weekInput.value = localStorage.getItem("attendance_selectedWeek");
    await changeAttendanceTableDate();
  }
}
async function changeAttendanceTableDate() {
  const selectedValue = weekInput.value;
  const weekNumber = selectedValue.split("-W")[1];
  attendanceDate.textContent = `Неделя: ${weekNumber}`;
}
async function changeTableTouchedStatus() {
  isTableTouched = false;
  saveAttendanceBtn.disabled = false;
  saveAsPdfBtn.disabled = false;
  saveAsExcelBtn.disabled = false;
}
async function fillAttendanceTable() {
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
async function makeListOfStudents(selectedSquad) {
  let count = 0;

  const studentsList = JSON.parse(localStorage.getItem("studentsList"));

  const selectedSquadStudentsList = studentsList.find(
    (item) => item.squadName === selectedSquad
  ).students;

  if (selectedSquadStudentsList !== null) {
    attendanceBody.innerHTML = "";
    localStorage.setItem("attendance_selectedSquad", selectedSquad);
    selectedSquadStudentsList.forEach(async (item) => {
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

    await checkAttendanceFilled();

    localStorage.setItem("selectedSquad", groupSelector.value);
  } else {
    alert("Студенты не найдены!");
  }

  await attendanceBtnHandler();
}
async function checkIsTableTouched() {
  if (isTableTouched && !isAttendanceSaved) {
    if (confirm("Сохранить изменения?")) {
      // SAVE ATTENDANCE
      await saveAttendanceLocally();
      isAttendanceSaved = true;
      return true;
    } else {
      return false;
    }
    await makeListOfStudents(groupSelector.value);
    isTableTouched = false;
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
async function checkAttendanceFilled() {
  // Check is attendanceTable filled
  if (
    localStorage.getItem(
      `attendanceDATA_${groupSelector.value}_${weekInput.value}`
    )
  ) {
    await fillAttendanceTable();
  } else {
    await makeListOfStudents(groupSelector.value);
  }
}

window.addEventListener("DOMContentLoaded", async () => {
  // Check selected squad
  await checkSelectedSquad();

  // Check selected date
  await checkSelectedWeek();

  // Load saved studentsList
  await makeListOfStudents(groupSelector.value);

  // Check saved locally attendance matrix
  await checkAttendanceFilled();

  // StepUP/Down weekInput listener
  stepUpBtn.addEventListener("click", async () => {
    await weekInput.stepUp();
    localStorage.setItem("attendance_selectedWeek", weekInput.value);
    await changeAttendanceTableDate();
    await checkIsTableTouched();
    //await checkAttendanceFilled();
  });
  stepDownBtn.addEventListener("click", async () => {
    await weekInput.stepDown();
    localStorage.setItem("attendance_selectedWeek", weekInput.value);
    await changeAttendanceTableDate();
    await checkIsTableTouched();
    //await checkAttendanceFilled();
  });
  weekInput.addEventListener("change", async () => {
    localStorage.setItem("attendance_selectedWeek", weekInput.value);
    await changeAttendanceTableDate();
    await checkIsTableTouched();
    //await checkAttendanceFilled();
  });

  // Search button listener
  searchBtn.addEventListener("click", async () => {
    await makeListOfStudents(groupSelector.value);
  });
});
