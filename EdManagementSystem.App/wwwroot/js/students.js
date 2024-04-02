const filterSelector = document.querySelector('[name="filterSelector"]');
const courseSelector = document.querySelector('.search-row__filter_courses');
const groupSelector = document.querySelector('.search-row__filter_student-groups');
const searchBtn = document.querySelector('.search-row__btn');
const saveAsPdfBtn = document.getElementById('downloadPdfTable');
const saveAsExcelBtn = document.getElementById('downloadExcelTable');
const tableElement = document.getElementById('studentsTable');
let selectedAPI = "GetAllStudents";
let selectedFilter = 0;

/* Dynamic colSpan for table */
var colSpanElement = document.getElementById("students-col-span");
const checkScreenWidth = () => {
    if (window.innerWidth < 600) {
        colSpanElement.colSpan = "1";
    } else {
        colSpanElement.colSpan = "4";
    }
};
window.addEventListener("resize", checkScreenWidth);
checkScreenWidth();

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
    saveAsExcelBtn.disabled = false;
    saveAsPdfBtn.disabled = false;
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

            // Create body of table
            const tBody = document.getElementById("tableContentContainer");

            // Clear table before adding new elements
            tBody.innerHTML = '';

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

            // Set count of students
            document.getElementById('students-count').textContent = data.length;

            // Insert tbody before tfoot
            const tFoot = studentsTable.querySelector('tfoot');
            studentsTable.insertBefore(tBody, tFoot);
        })
        .catch(error => {
            alert(error.message);
        })
});

// Download as Excel
document.getElementById('downloadExcelTable').addEventListener('click', () => {
    const wb = XLSX.utils.table_to_book(document.getElementById('studentsTable'));

    wb.SheetNames.forEach(sheetName => {
        const ws = wb.Sheets[sheetName];
        const range = XLSX.utils.decode_range(ws['!ref']);
        range.e.r += 2;
        ws['!ref'] = XLSX.utils.encode_range(range);
    });

    wb.sheetName = "PIU";

    XLSX.writeFile(wb, 'Список студентов.xlsx');
});

// Download as Pdf
saveAsPdfBtn.addEventListener('click', () => {
    var table = document.getElementById('studentsTable');
    var html = table.outerHTML;

    var val = htmlToPdfmake(html);
    var dd = {
        content: val,
    };
    pdfMake.createPdf(dd).download('Список студентов');
});