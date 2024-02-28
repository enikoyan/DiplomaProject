document.addEventListener("DOMContentLoaded", () => {
    document.body.addEventListener("click", (event) => {
        if (event.target.classList.contains("questions-item__toggle-btn")) {
            const btn = event.target;
            const parent = btn.closest(".questions-item");
            const answer = parent.querySelector(".questions-item__answer");

            if (btn.textContent === "+") {
                btn.textContent = "-";
                answer.style.maxHeight = answer.scrollHeight + "px";
            } else {
                btn.textContent = "+";
                answer.style.maxHeight = "0";
            }
        }
    });
});