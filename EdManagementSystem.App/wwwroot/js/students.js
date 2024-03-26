const filterSelector = document.querySelector('[name="filterSelector"]');
const courseSelector = document.querySelector('.search-row__filter_courses');
const groupSelector = document.querySelector('.search-row__filter_student-groups');
const searchBtn = document.querySelector('.search-row__btn');
let selectedAPI = "GetAllStudents";
let selectedFilter = 0;

// Handler of changing filter event
filterSelector.addEventListener('change', function () {

    if (filterSelector.value === "searchAllStudents") {
        courseSelector.disabled = true;
        courseSelector.style.display = "none";
        groupSelector.disabled = true;
        groupSelector.style.display = "none";

        selectedFilter = 0;
    }

    else if (filterSelector.value === "searchBySquads") {
        courseSelector.disabled = true;
        courseSelector.style.display = "none";
        groupSelector.disabled = false;
        groupSelector.style.display = "block";

        selectedFilter = 1;
    }
    else if (filterSelector.value === "searchByCourses") {
        groupSelector.disabled = true;
        groupSelector.style.display = "none";
        courseSelector.disabled = false;
        courseSelector.style.display = "block";

        selectedFilter = 2;
    }
});

// API calling
searchBtn.addEventListener('click', () => {
    let selectedAPI = "";
    let apiUrl = "";

    switch (selectedFilter) {
        case 0: {
            selectedAPI = "GetAllStudents";
            break;
        }
        case 1: {
            selectedAPI = `GetStudentsBySquad/${groupSelector.value}`;
            break;
        }
        case 2: {
            selectedAPI = `GetStudentsByCourse/${courseSelector.value}`;
            break;
        }
    }

    apiUrl = `https://localhost:44370/api/Students/${selectedAPI}`;

    fetch(apiUrl)
        .then(response => {
            if (!response.ok) {
                throw new Error('Данных не найдено');
            }
            return response.json();
        })
        .then(data => {
            // Get table from Html
            const studentsTable = document.querySelector('.custom-table_student-list');

            // Clear table before adding new elements
            studentsTable.innerHTML = '';

            // Create body of table
            const tBody = document.createElement("tbody");
            data.forEach(item => {
                const tRow = document.createElement("tr");
                for (let key in item) {
                    const tHeader = document.createElement("th");
                    tHeader.textContent = item[key];
                    tRow.appendChild(tHeader);
                }

                // Create moreBtn
                const button = document.createElement('button');
                button.classList.add('custom-btn');
                button.textContent = 'Узнать';
                const th = document.createElement("th");
                th.appendChild(button);
                tRow.appendChild(th);

                tBody.appendChild(tRow);
            });

            // Create footer of table
            const studentCount = data.length;
            const tFoot = document.createElement('tfoot');
            const tr = document.createElement('tr');
            const th = document.createElement('th');
            th.setAttribute('id', 'student-count');
            th.setAttribute('scope', 'row');
            th.setAttribute('colspan', '4');
            th.textContent = 'Количество студентов';
            const countTh = document.createElement('th');
            countTh.textContent = studentCount;

            tr.appendChild(th);
            tr.appendChild(countTh);
            tFoot.appendChild(tr);

            studentsTable.appendChild(tBody);
            studentsTable.appendChild(tFoot);
        })
        .catch(error => {
            alert(error.message);
        });
});