const filterSelector = document.getElementById('filter-select');
const courseSelector = document.getElementById('course-select');
const groupSelector = document.getElementById('group-select');
const searchBtn = document.querySelector('.search-row__btn');
const saveAsPdfBtn = document.getElementById('downloadPdfTable');
const saveAsExcelBtn = document.getElementById('downloadExcelTable');
const tableElement = document.getElementById('studentsTable');
var selectedAPI = "GetAllStudents";
var selectedFilter = 0;
var lastSelectControl = "searchAllStudents";

async function selectHandler(container) {
    const children = Array.from(container.children);

    children[0].classList.toggle('custom-select__btn_active');
    children[1].classList.toggle('custom-options_disabled');
}

// Handler of changing filter event
async function selectOptionHandler(selectedOption) {
    const value = selectedOption.getAttribute("data-value");
    const valueText = selectedOption.textContent;
    const customSelect = selectedOption.closest('.custom-select');
    const customSelectTitle = customSelect.querySelector('.custom-select__title');

    customSelectTitle.textContent = selectedOption.textContent;
    customSelect.setAttribute("data-target", value);

    await switchSelects(value);
}

async function switchSelects(value) {
    switch (value) {
        case "searchAllStudents": {
            courseSelector.disabled = true;
            courseSelector.style.display = "none";
            groupSelector.disabled = true;
            groupSelector.style.display = "none";

            selectedFilter = 0;
            lastSelectControl = value;
            break;
        };
        case "searchByCourses": {
            courseSelector.disabled = true;
            courseSelector.style.display = "none";
            groupSelector.disabled = false;
            groupSelector.style.display = "flex";

            selectedFilter = 1;
            lastSelectControl = value;
            break;
        };
        case "searchBySquads": {
            groupSelector.disabled = true;
            groupSelector.style.display = "none";
            courseSelector.disabled = false;
            courseSelector.style.display = "flex";

            selectedFilter = 2;
            lastSelectControl = value;
            break;
        };
    }
}

document.addEventListener('DOMContentLoaded', async () => {
    // API calling
    searchBtn.addEventListener('click', () => {
        if (filterSelector.getAttribute('data-target') === 'none') {
            alert('Вы не выбрали фильтр для поиска!');
        }
        else {
            let selectedAPI = "";
            let apiUrl = "";

            switch (selectedFilter) {
                case 0: {
                    selectedAPI = "GetAllStudents";
                    break;
                }
                case 1: {
                    selectedAPI = `GetStudentsBySquad/${groupSelector.getAttribute('data-target')}`;
                    break;
                }
                case 2: {
                    selectedAPI = `GetStudentsByCourse/${courseSelector.getAttribute('data-target')}`;
                    break;
                }
            }

            apiUrl = `https://localhost:44370/api/Students/${selectedAPI}`;

            fetch(apiUrl)
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Данных не найдено');
                    }
                    saveAsExcelBtn.disabled = false;
                    saveAsPdfBtn.disabled = false;
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
        }
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
});