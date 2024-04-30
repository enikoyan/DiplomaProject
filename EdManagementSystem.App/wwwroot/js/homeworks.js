window.addEventListener('DOMContentLoaded', async () => {
    // Filter handler
    await filterHandler();
});

// Variables and controls
const filterSelector = document.querySelector('[name="filterSelector"]');
const filterSelectorMaterial = document.querySelector('[name="filterSelectorMaterial"]');
const courseSelector = document.querySelector('.search-row__filter_courses');
const groupSelector = document.querySelector('.search-row__filter_student-groups');
const searchBtn = document.querySelector('.search-row__btn_search');

// Functions
async function filterHandler() {
    if (localStorage.getItem('selectedFilter')) filterSelector.value = localStorage.getItem('selectedFilter');
    await changeFilter();
    switch (filterSelector.value) {
        case "searchBySquads": {
            if (localStorage.getItem('groupSelectorValue')) groupSelector.value = localStorage.getItem('groupSelectorValue');
            break;
        }
        case "searchByCourses": {
            if (localStorage.getItem('courseSelectorValue')) courseSelector.value = localStorage.getItem('courseSelectorValue');
            break;
        }
    };
}

// Handler of changing filter event
filterSelector.addEventListener('change', function () { changeFilter() });

// Search filter change handler
async function changeFilter() {
    if (filterSelector.value === "searchBySquads") {
        courseSelector.disabled = true;
        courseSelector.style.display = "none";
        groupSelector.disabled = false;
        groupSelector.style.display = "block";

        selectedFilter = 0;
    }
    else if (filterSelector.value === "searchByCourses") {
        groupSelector.disabled = true;
        groupSelector.style.display = "none";
        courseSelector.disabled = false;
        courseSelector.style.display = "block";

        selectedFilter = 1;
    }
    localStorage.setItem('selectedFilter', filterSelector.value);
}