/* Hide and Show aside menu */
const button = document.querySelector(".aside-menu-toggle");
const targetElement = document.querySelector(".aside");

// Check localStorage
const isAsideMenuOpen = localStorage.getItem("asideMenuOpen") === "true";

if (isAsideMenuOpen) {
    targetElement.classList.remove("aside_closed");
} else {
    targetElement.classList.add("aside_closed");
}

button.addEventListener("click", () => {
    if (targetElement.classList.contains("aside_closed")) {
        targetElement.classList.remove("aside_closed");
        localStorage.setItem("asideMenuOpen", "true");
    } else {
        targetElement.classList.add("aside_closed");
        localStorage.setItem("asideMenuOpen", "false");
    }
});