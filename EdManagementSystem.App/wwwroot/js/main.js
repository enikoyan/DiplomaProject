// Variables and controls
const logOutBtn = document.getElementById("logOutBtn");
const burgerCheckbox = document.querySelector(".hamburger-checkbox");
const burgerLinesList = document.querySelectorAll(".line");
const asideNav = document.querySelector(".aside-menu");
const logotype = document.querySelector(".aside-logo");
const asideMenu = document.querySelector(".aside");
const asideMenuHeader = document.querySelector(".aside__header");
const asideMenuTextList = document.querySelectorAll(".aside-menu__span");
const logoutForm = document.querySelector('form[action="/Auth/Logout"]');
const logoutItem = document.getElementById('logoutItem');
const logoutAPIUri = "https://localhost:44354/auth/Logout";

window.addEventListener("DOMContentLoaded", async () => {
    // Logout handler
    logOutBtn.addEventListener("click", () => {
        localStorage.clear();
    });

    logoutItem.addEventListener('click', async (event) => {
        event.preventDefault();
        const confirmLogout = confirm("Вы уверены, что хотите выйти?");
        if (confirmLogout) {
            logoutForm.submit();
        }
    });

    burgerCheckbox.checked = JSON.parse(localStorage.getItem('sidebarState'));
    await changeSidebarSize(JSON.parse(localStorage.getItem('sidebarState')));

    // Burger menu handler
    burgerCheckbox.addEventListener("change", async () => await setBurgerState());

    async function setBurgerState() {
        await changeSidebarSize(burgerCheckbox.checked);
    }

    async function closeBurger() {
        burgerLinesList[0].style.transform = "rotate(0)";
        burgerLinesList[1].style.transform = "scaleY(1)";
        burgerLinesList[2].style.transform = "rotate(0)";
    }

    async function openBurger() {
        burgerLinesList[0].style.transform = "rotate(45deg)";
        burgerLinesList[1].style.transform = "scaleY(0)";
        burgerLinesList[2].style.transform = "rotate(-45deg)";
    }

    async function changeSidebarSize(sidebarState) {
        if (sidebarState === true) {
            asideMenu.classList.remove("aside_closed");
            await openBurger();
            localStorage.setItem('sidebarState', true);
        }
        else {
            asideMenu.classList.add("aside_closed");
            await closeBurger();
            localStorage.setItem('sidebarState', false);
        }
    }

    async function logoutUser() {
        fetch(logoutAPIUri, {
            method: 'POST',
        })
            .then(async response => {
                if (response.ok) {
                    console.log("URA");
                }
                else console.log('PIU');
            })
            .catch(error => {
                console.log('Возникла ошибка:', error);
            });
    }
});
