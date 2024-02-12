// Последняя выбранная дата
document.addEventListener("DOMContentLoaded", function () {
  const weekInput = document.getElementById("weekInput");
  const selectedWeek = localStorage.getItem("selectedWeek");

  if (selectedWeek) {
    weekInput.value = selectedWeek;
  }
});

// Загрузка расписания по выбранной неделе
document
  .getElementById("showScheduleBtn")
  .addEventListener("click", function () {
    // Выбранная неделя
    const weekInput = document.getElementById("weekInput");
    const selectedWeek = weekInput.value;

    fetch("../js/json/schedule-list.json")
      .then((response) => response.json())
      .then((data) => {
        // Найти выбранную неделю
        const week = data.schedule.find((w) => w.week === selectedWeek);
        // Сохранение выбранной недели в localStorage
        localStorage.setItem("selectedWeek", selectedWeek);

        // Если нашли
        if (week) {
          const scheduleContainer = document.querySelector(".schedule");
          scheduleContainer.innerHTML = "";

          week.days.forEach((day) => {
            // Создаем блок с днем недели
            const scheduleItem = document.createElement("div");
            scheduleItem.classList.add("schedule__item");

            /* Внутри блока scheduleItem */

            // День недели
            const scheduleItem_title = document.createElement("h3");
            scheduleItem_title.textContent = day.day;
            scheduleItem_title.classList.add("schedule__title");
            scheduleItem.appendChild(scheduleItem_title);

            // Создание таблицы расписания для текущего дня
            const scheduleDayTable = document.createElement("table");
            scheduleDayTable.classList.add(
              "custom-table",
              "custom-table_lessons-list"
            );
            scheduleItem.appendChild(scheduleDayTable);

            // Шапка
            const thead = document.createElement("thead");
            const tbHeaderRow = document.createElement("tr");
            const tbHeaderColumns = [
              "Пара",
              "Аудитория",
              "Группа",
              "Время занятия",
            ];
            tbHeaderRow.setAttribute("style", "background-color: transparent");
            tbHeaderColumns.forEach((column) => {
              const tableHeaderColumn = document.createElement("th");
              tableHeaderColumn.setAttribute("scope", "col");
              tableHeaderColumn.textContent = column;
              tbHeaderRow.appendChild(tableHeaderColumn);
            });
            thead.appendChild(tbHeaderRow);
            scheduleDayTable.appendChild(thead);

            day.lessons.forEach((lesson) => {
              // Тело таблицы
              const tbody = document.createElement("tbody");
              const tableDataRow = document.createElement("tr");
              const tableDataColumns = [
                lesson.lesson_number,
                lesson.room,
                lesson.group,
                lesson.lesson_time,
              ];
              tableDataColumns.forEach((column) => {
                const tableDataColumn = document.createElement("th");
                tableDataColumn.textContent = column;
                tableDataRow.appendChild(tableDataColumn);
              });
              tbody.appendChild(tableDataRow);
              scheduleDayTable.appendChild(tbody);
            });

            // Создаем кнопку редактировать
            const editBtn = document.createElement("button");
            editBtn.textContent = "Редактировать";
            editBtn.classList.add(
              "custom-btn",
              "custom-btn_edit-schedule-item"
            );
            scheduleItem.append(editBtn);

            scheduleContainer.appendChild(scheduleItem);
          });
        } else {
          alert("Расписание для выбранной недели не найдено");
        }
      });
  });
