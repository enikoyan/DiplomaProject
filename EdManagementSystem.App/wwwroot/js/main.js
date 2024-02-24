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


// Active button
const menuItems = document.querySelectorAll('.aside-menu__item');

let activeIndex = localStorage.getItem('activeIndex');
if (!activeIndex) {
    activeIndex = '0';
}

menuItems.forEach((item, index) => {
    if (index.toString() === activeIndex) {
        item.classList.add('aside-menu__item_active');
    }
});

menuItems.forEach((item, index) => {
    item.addEventListener('click', () => {
        menuItems.forEach(item => item.classList.remove('aside-menu__item_active'));
        item.classList.add('aside-menu__item_active');
        localStorage.setItem('activeIndex', index.toString());
    });
});