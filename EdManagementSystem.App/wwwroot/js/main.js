// Active button
const menuItems = document.querySelectorAll('.aside-menu__item');

let path = window.location.pathname.split('/')[2];

menuItems.forEach(item => {
    if (item.parentElement.getAttribute('data-path') === path) {
        item.classList.add('aside-menu__item_active');
    }
});

// Change content
document.addEventListener('DOMContentLoaded', () => {

    /* Hide and Show aside menu */
    var key = "menuState";
    const aside = document.querySelector(".aside");
    const asideMenuToggle = document.querySelector('.aside-menu-toggle');

    // Добавляем обработчик клика для кнопки меню
    asideMenuToggle.addEventListener('click', () => {
        if (aside.classList.contains('aside_closed')) {
            aside.classList.remove('aside_closed');
            localStorage.setItem(key, 'TRUE');
        } else {
            aside.classList.add('aside_closed');
            localStorage.setItem(key, 'FALSE')
        }
    });

    try {
        var menuOpen = localStorage.getItem(key);
        if (menuOpen === null || menuOpen === 'FALSE') {
            closeNav();
        }
        else {
            openNav();
        }
    }
    catch (ex) {
        console.log("Ошибка: " + ex.message);
    }

    function openNav() {
        const targetElement = document.querySelector(".aside");
        targetElement.style.display = "flex";
        targetElement.classList.remove("aside_closed");

        localStorage.setItem(key, 'TRUE');
    }

    function closeNav() {
        const targetElement = document.querySelector(".aside");
        targetElement.style.display = "flex";
        targetElement.classList.add("aside_closed");

        localStorage.setItem(key, 'FALSE');
    }

    /* Ajax content loading */
    const contentContainer = document.querySelector('.dashboard-content-container');
    const navLinks = document.querySelectorAll('.aside-menu__item');
    const loadingOverlay = document.querySelector('.dashboard-content-container__loader');

    const showLoadingOverlay = () => {
        loadingOverlay.style.opacity = '1';
    };

    const hideLoadingOverlay = () => {
        loadingOverlay.style.opacity = '0';
    };
});