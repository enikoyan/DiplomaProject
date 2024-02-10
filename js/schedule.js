// Получение данных из JSON-файла
fetch('../js/json/schedule-list.json')
  .then(response => response.json())
  .then(data => {

    // Получаем контейнер для расписания
    const schedule = document.querySelector('.schedule');

    // Перебираем дни недели
    for (let day in data) {

      // Создаем блок с днем недели
      const scheduleItem = document.createElement('div');
      scheduleItem.classList.add('schedule__item');

      /* Внутри блока scheduleItem */

      // День недели
      const scheduleItem_title = document.createElement('h3');
      scheduleItem_title.textContent = day;
      scheduleItem_title.classList.add('schedule__title');
      scheduleItem.appendChild(scheduleItem_title);

      // Создание таблицы расписания для текущего дня
      const scheduleDayTable = document.createElement('table');
      scheduleDayTable.classList.add('custom-table', 'custom-table_lessons-list');
      scheduleItem.appendChild(scheduleDayTable);

      // Шапка
      const thead = document.createElement('thead');
      const tbHeaderRow = document.createElement('tr');
      const tbHeaderColumns = ['Пара', 'Аудитория', 'Время занятия'];   
      tbHeaderRow.setAttribute('style', 'background-color: transparent');
      tbHeaderColumns.forEach(column => {
        const tableHeaderColumn = document.createElement('th');
        tableHeaderColumn.setAttribute('scope', 'col');
        tableHeaderColumn.textContent = column;
        tbHeaderRow.appendChild(tableHeaderColumn);
      });
      thead.appendChild(tbHeaderRow);
      scheduleDayTable.appendChild(thead);

      // Тело таблицы
      const tbody = document.createElement('tbody');

      data[day].forEach(schedule => {
        const tableDataRow = document.createElement('tr');
        const tableDataColumns = [schedule.lesson_number, schedule.room, schedule.lesson_time];
        tableDataColumns.forEach(column => {
          const tableDataColumn = document.createElement('th');
          tableDataColumn.textContent = column;
          tableDataRow.appendChild(tableDataColumn);
        });
        tbody.appendChild(tableDataRow);
      });
      scheduleDayTable.appendChild(tbody);

      // Создаем кнопку редактировать
      const editBtn = document.createElement('button');
      editBtn.textContent = 'Редактировать';
      editBtn.classList.add('custom-btn', 'custom-btn_edit-schedule-item');
      scheduleItem.append(editBtn);

      schedule.appendChild(scheduleItem);
    }
});