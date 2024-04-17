window.addEventListener('DOMContentLoaded', () => {
    // Controls and variables
    const weekInput = document.getElementById('weekInput');
    const stepUpBtn = document.getElementById('nextWeekBtn');
    const stepDownBtn = document.getElementById('previousWeekBtn');

    // Functions to step the week
    stepUpBtn.addEventListener('click', () => { weekInput.stepUp(1); localStorage.setItem("selectedWeek", weekInput.value); });
    stepDownBtn.addEventListener('click', () => { weekInput.stepDown(1); localStorage.setItem("selectedWeek", weekInput.value); });

    // Week changing handler
    weekInput.addEventListener('change', function () {
        localStorage.setItem("selectedWeek", this.value);
    })

    if (localStorage.getItem("selectedWeek")) {
        weekInput.value = localStorage.getItem("selectedWeek");
    };
});