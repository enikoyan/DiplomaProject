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
const logoutItem = document.getElementById("logoutItem");
const logoutAPIUri = "https://localhost:44354/auth/Logout";

window.addEventListener("DOMContentLoaded", async () => {
    // Logout handler
    logOutBtn.addEventListener("click", () => {
        localStorage.clear();
    });

    logoutItem.addEventListener("click", async (event) => {
        event.preventDefault();
        const confirmLogout = confirm("Вы уверены, что хотите выйти?");
        if (confirmLogout) {
            logoutForm.submit();
        }
    });

    burgerCheckbox.checked = JSON.parse(localStorage.getItem("sidebarState"));
    await changeSidebarSize(JSON.parse(localStorage.getItem("sidebarState")));

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
            localStorage.setItem("sidebarState", true);
        } else {
            asideMenu.classList.add("aside_closed");
            await closeBurger();
            localStorage.setItem("sidebarState", false);
        }
    }
});

// PRELOADER
const animation = lottie.loadAnimation({
    container: document.getElementById("logoLottie"),
    renderer: "svg",
    loop: true,
    autoplay: true,
    path: "../js/outerJS/lottie_logo.json",
});

document.addEventListener("DOMContentLoaded", function () {
    var preloader = document.querySelector(".preloader");
    var lottieLogo = document.getElementById("logoLottie");
    var links = document.querySelectorAll("a");

    links.forEach(function (link) {
        link.addEventListener("click", function () {
            setTimeout(function () {
                preloader.classList.remove("preloader_hidden");
                lottieLogo.classList.remove("logoLottie_hidden");
            }, 200);
        });
    });
    window.addEventListener("load", function () {
        setTimeout(function () {
            preloader.classList.add("preloader_hidden");
            lottieLogo.classList.add("logoLottie_hidden");
        }, 600);
    });
});
