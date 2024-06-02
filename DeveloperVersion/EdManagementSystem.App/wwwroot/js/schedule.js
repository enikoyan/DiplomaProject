window.addEventListener('DOMContentLoaded', async () => {

    // Variables and controls
    const weekInput = document.getElementById('weekInput');
    const stepUpBtn = document.getElementById('nextWeekBtn');
    const stepDownBtn = document.getElementById('previousWeekBtn');
    const scheduleItems = document.querySelectorAll('.schedule-item__body');
    const refreshDataBtn = document.getElementById('refreshScheduleBtn');
    var weekStart = '';
    var teacherEmail = '';
    var weekEnd = '';
    var currentMonth = '';
    var apiUrl = "https://localhost:44370/api/Schedules/GetScheduleByWeek/?";
    const month = {
        1: "янв", 2: "фев", 3: "мар", 4: "апр", 5: "май", 6: "июн", 7: "июл", 8: "авг", 9: "сен", 10: "окт", 11: "ноя", 12: "дек"
    }

    // First methods
    await setDateValue();
    await getUserId();
    await createSchedule();

    // WeekInput select handler
    weekInput.addEventListener('change', async () => await createSchedule());
    stepUpBtn.addEventListener('click', async () => { await weekInput.stepUp(); await createSchedule() });
    stepDownBtn.addEventListener('click', async () => { await weekInput.stepDown(); await createSchedule() });

    // Refresh data btn handler
    refreshDataBtn.addEventListener('click', async () => await refreshData());

    // Refresh data for current date
    async function refreshData() {
        deleteWeekScheduleData(weekInput.value);
        localStorage.removeItem('selectedWeek');
        await createSchedule();
    }

    // Create schedule
    async function createSchedule() {

        // Set numbers of week days
        await setDaysNumbers();

        /* Create schedule events */

        // Check localStorage
        if (localStorage.getItem(`weekSchedule / ${teacherEmail} / ${weekInput.value}`)) {
            const weekScheduleJSON = getWeekSchedule(weekInput.value).schedule;
            await clearScheduleEvents();
            await createScheduleEvents(weekScheduleJSON);
        }
        else await extractScheduleFromAPI();
    }

    // Create events in HTML
    async function createScheduleEvents(data) {

        // Get days of events
        let lessonsByDay = data.reduce((acc, lesson) => {
            if (!acc[lesson.weekday]) {
                acc[lesson.weekday] = [];
            }
            acc[lesson.weekday].push(lesson);
            return acc;
        }, {});

        let count = 0;

        // Events by day
        for (let day in lessonsByDay) {

            lessonsByDay[day].forEach(async lesson => {
                // Create event element
                await createEventElement(lesson, scheduleItems[count]);
            });

            count++;
        }
    }

    // Create event element in the container by days
    async function createEventElement(event, item) {
        // Event element
        const eventElement = document.createElement('div');
        eventElement.className = 'schedule-item__event';
        eventElement.setAttribute('data', `id: ${event.id}`);

        /* Attributes */
        const eventNote = document.createElement('span');
        eventNote.className = 'schedule-item__note';
        eventNote.textContent = event.note;
        const eventPlace = document.createElement('span');
        eventPlace.className = 'schedule-item__place';
        eventPlace.textContent = event.place;
        const eventSquad = document.createElement('span');
        eventSquad.className = 'schedule-item__squad';
        eventSquad.textContent = event.squadName;

        let startTime = event.timelineStart.slice(0, -3);
        let endTime = event.timelineEnd.slice(0, -3);
        const eventTimeline = document.createElement('time');
        eventTimeline.className = 'schedule-item__timeline';
        eventTimeline.textContent = `${startTime} - ${endTime}`;

        eventElement.appendChild(eventNote);
        eventElement.appendChild(eventPlace);
        eventElement.appendChild(eventSquad);
        eventElement.appendChild(eventTimeline);

        item.appendChild(eventElement);
    }

    // Call API and extract schedule for selected week
    async function extractScheduleFromAPI() {
        // Get input data for API calling
        const result = getWeekBorders(weekInput.value);
        weekStart = result.firstDay;
        weekEnd = result.lastDay;

        let isScheduleFound = false;

        const params = {
            teacherEmail,
            weekStart,
            weekEnd
        }

        await clearScheduleEvents();

        // GET SCHEDULE FOR SELECTED WEEK
        fetch(apiUrl + new URLSearchParams(params), {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(async response => {
                if (!response.ok) {
                    response.text().then(async text => {
                        await createMessagePopup("error", text);
                        isScheduleFound = false;
                    });
                }
                else {
                    await createMessagePopup("success", "Расписание успешно загружено!");
                    isScheduleFound = true;
                    return response.json();
                }
            })
            .then(async data => {
                if (isScheduleFound) {
                    // Save schedule to the localStorage
                    saveWeekSchedule(weekInput.value, data);
                    localStorage.setItem('selectedWeek', weekInput.value);
                    await createScheduleEvents(data);
                }
            })
            .catch(error => {
                console.log(error);
            });
    }

    // Clear events
    async function clearScheduleEvents() {
        const elements = document.querySelectorAll('.schedule-item__event');

        elements.forEach(element => {
            element.remove();
        });
    }

    // Save schedule into localStorage
    function saveWeekSchedule(weekNumber, scheduleData) {
        const weekSchedule = {
            weekNumber: weekNumber,
            schedule: scheduleData
        };
        localStorage.setItem(`weekSchedule / ${teacherEmail} / ${weekNumber}`, JSON.stringify(weekSchedule));
    }

    // Extract schedule from localStorage
    function getWeekSchedule(weekNumber) {
        const storedData = localStorage.getItem(`weekSchedule / ${teacherEmail} / ${weekNumber}`);
        return storedData ? JSON.parse(storedData) : null;
    }

    // Delete schedule from localStorage
    async function deleteWeekScheduleData(weekNumber) {
        localStorage.removeItem(`weekSchedule / ${teacherEmail} / ${weekNumber}`);
    }

    // Get currentUserId
    async function getUserId() {
        await fetch('/dashboard/getCurrentUserId')
            .then(response => response.text())
            .then(data => teacherEmail = data)
            .catch(error => console.error('Ошибка:', error));
    }

    // Set date value for the input
    function setDateValue() {
        if (localStorage.getItem('selectedWeek')) {
            weekInput.value = localStorage.getItem('selectedWeek');
        }
        else {
            localStorage.setItem('selectedWeek', weekInput.value);
        }
        setDaysNumbers();
    }

    // Set numbers of week days
    async function setDaysNumbers() {
        const scheduleHeaderItems = document.querySelectorAll('.schedule-item__date');

        // Get first day of scheduleInterval
        weekStart = await getWeekBorders(weekInput.value).firstDay;
        var firstDay = (new Date(weekStart)).getDate();

        // Get current month name
        currentMonth = await setCurrentMonth();

        for (let i = 0; i < scheduleHeaderItems.length; i++) {
            scheduleHeaderItems[i].textContent = `${firstDay.toString()} ${currentMonth}`;
            firstDay++;
        }
    }

    // Setting current month name
    async function setCurrentMonth() {
        return month[Math.floor(getWeekNumber() / 4)];
    }

    // Get number of week
    function getWeekNumber() {
        return weekInput.value.slice(weekInput.value.indexOf('W') + 1);
    }

    // Get weekStart and weekEnd from weekInput
    function getWeekBorders(weekValue) {
        // Split the line into the year and the week number
        var year = parseInt(weekValue.substring(0, 4));
        var weekNumber = parseInt(weekValue.substring(6));

        // Creating a new Date object with the first day of the week
        var firstDay = new Date(year, 0, (weekNumber - 1) * 7 + 2);

        // Get the last day of the week by adding 6 days to the first day
        var lastDay = new Date(firstDay);
        lastDay.setDate(lastDay.getDate() + 6);

        return {
            firstDay: firstDay.toISOString().split('T')[0],
            lastDay: lastDay.toISOString().split('T')[0]
        };
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
});