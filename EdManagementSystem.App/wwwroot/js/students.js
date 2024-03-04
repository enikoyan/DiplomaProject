const filterSelector = document.querySelector('[name="filterSelector"]');
const courseSelector = document.querySelector('.search-row__filter_courses');
const groupSelector = document.querySelector('.search-row__filter_student-groups');

// Обработчик события изменения выбора фильтра
filterSelector.addEventListener('change', function () {
    // Если выбрана опция "По группам"
    if (filterSelector.value === "searchBySquads") {
        courseSelector.disabled = true; // Делаем select курсов недоступным
        courseSelector.style.display = "none"; // Скрываем select курсов

        groupSelector.disabled = false; // Делаем select групп доступным
        groupSelector.style.display = "block"; // Показываем select групп
    }
    // Если выбрана опция "По курсам"
    else if (filterSelector.value === "searchByCourses") {
        groupSelector.disabled = true; // Делаем select групп недоступным
        groupSelector.style.display = "none"; // Скрываем select групп

        courseSelector.disabled = false; // Делаем select курсов доступным
        courseSelector.style.display = "block"; // Показываем select курсов
    }
});