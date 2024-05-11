// Variables and controls
const weekInput = document.getElementById('weekInput');
const stepUpBtn = document.getElementById('nextWeekBtn');
const stepDownBtn = document.getElementById('previousWeekBtn');
const attendanceSelectVars = document.querySelectorAll('.attendance-popup__text');
const popup = document.querySelector('.attendance-popup');
const groupSelector = document.querySelector('.search-row__filter_student-groups');
const searchBtn = document.querySelector('.search-row__btn_search');
const attendanceBody = document.querySelector('.attendance-body');
const attendanceDate = document.getElementById('attendance-table-date');
var tdFioValue = "";
const attendanceIconMap = {
    0: "attendance-empty",
    1: "attendance-true",
    2: "attendance-false"
}

// WeekInput select handler
weekInput.addEventListener('change', async () => { await changeAttendanceTableDate(); });
stepUpBtn.addEventListener('click', async () => {
    await weekInput.stepUp();
    localStorage.setItem('weekInputSelectedValue', weekInput.value);
    await changeAttendanceTableDate();
});
stepDownBtn.addEventListener('click', async () => {
    await weekInput.stepDown();
    localStorage.setItem('weekInputSelectedValue', weekInput.value);
    await changeAttendanceTableDate();
});

async function changeAttendanceTableDate() {
    const selectedValue = weekInput.value;
    const weekNumber = selectedValue.split('-W')[1];
    attendanceDate.textContent = `Неделя: ${weekNumber}`;
}

window.addEventListener('DOMContentLoaded', async () => {
    // Week input select
    if (localStorage.getItem('weekInputSelectedValue')) {
        weekInput.value = localStorage.getItem('weekInputSelectedValue');
    }
    if (localStorage.getItem('selectedSquad')) {
        groupSelector.value = localStorage.getItem('selectedSquad');
    }

    await getAttendanceBeforePageLoad();

    // Search btn handler
    searchBtn.addEventListener('click', async () => {
        // Make a list of students
        await makeListOfStudents(groupSelector.value);
        await attendanceBtnHandler();
    });

    async function getAttendanceBeforePageLoad() {
        try {
            await makeListOfStudents(groupSelector.value);
            await attendanceBtnHandler();
        }
        catch {
            console.log("Информации нет!");
        }
    }

    async function attendanceBtnHandler() {
        const attendanceBtn = document.querySelectorAll('.attendance-btn');

        // Select in the table
        attendanceBtn.forEach(item => {
            item.addEventListener('click', e => {
                const buttonRect = item.getBoundingClientRect();
                const popupWidth = popup.offsetWidth;
                const popupHeight = popup.offsetHeight;
                const offsetX = (popupWidth - buttonRect.width) / 2;
                const offsetY = 5;
                const popupLeft = buttonRect.left - offsetX;
                const popupTop = buttonRect.bottom + offsetY;

                popup.style.position = "fixed";
                popup.style.top = `${popupTop}px`;
                popup.style.left = `${popupLeft}px`;
                popup.style.visibility = 'visible';

                // Удаляем обработчики событий для attendanceSelectVars
                attendanceSelectVars.forEach(variant => {
                    variant.removeEventListener('click', variant.clickHandler);
                });

                // Добавляем новые обработчики событий для attendanceSelectVars
                attendanceSelectVars.forEach(variant => {
                    variant.clickHandler = () => {
                        const itemId = variant.id;
                        item.children[0].setAttribute('src', `../icons/attendance-icons/${itemId}.svg`);
                        item.children[0].setAttribute('data-status', `${itemId}`);
                        popup.style.visibility = 'collapse';
                    };
                    variant.addEventListener('click', variant.clickHandler);
                });
            });
        });
    }

    async function makeListOfStudents(selectedSquad) {
        let count = 0;

        const studentsList = JSON.parse(localStorage.getItem('studentsList'));

        const selectedSquadStudentsList = studentsList.find(item => item.squadName === selectedSquad).students;

        if (selectedSquadStudentsList !== null) {

            selectedSquadStudentsList.forEach((item) => {
                count++;
                var tRow = document.createElement('tr');
                tRow.innerHTML =
                    `<td class="attendance-td">${count}</td>
                     <td class="attendance-td"> ${item.fio}</td>
                `;

                for (let i = 0; i < 7; i++) {
                    tRow.innerHTML +=
                        `<td>
                            <button class="attendance-btn">
                                <img class="attendance-icon" src="/icons/attendance-icons/${attendanceIconMap[0]}.svg" data-status="${attendanceIconMap[0]}">
                            </button>
                        </td>`;
                }

                attendanceBody.appendChild(tRow);
            });

            localStorage.setItem('selectedSquad', groupSelector.value);
        }
        else alert("Студенты не найдены!");
    }
});