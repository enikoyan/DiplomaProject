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

// Preloader
var preloader = document.querySelector(".preloader");
var lottieLogo = document.getElementById("logoLottie");
var links = document.querySelectorAll("a");

const animation = lottie.loadAnimation({
    container: document.getElementById("logoLottie"),
    renderer: "svg",
    loop: true,
    autoplay: true,
    path: "../js/outerJS/lottie_logo.json",
});

async function confirmPopup(messageText) {
    return new Promise((resolve, reject) => {
        try {
            document.querySelector('.custom-alert').remove();
        }
        catch {

        }

        const popupElement = document.createElement('div');
        const popupHTML =
            `<div class="custom-alert custom-alert_confirm">
            <div>
                <img class="custom-alert__icon" src="../icons/message-icons/info-message.svg" data-status:"info"/>
                <p class="custom-alert__message">${messageText}</p>
            </div>
            <div>
                <button type="button" class="custom-btn_secondary" data-returnValue="yes">Да</button>
                <button type="button" class="custom-btn_secondary" data-returnValue="no">Нет</button>
            </div>
        </div>`;

        popupElement.innerHTML = popupHTML;
        document.body.appendChild(popupElement);

        popupElement.querySelector('[data-returnValue="yes"]').addEventListener('click', function () {
            popupElement.remove();
            resolve(true);
        });

        popupElement.querySelector('[data-returnValue="no"]').addEventListener('click', function () {
            popupElement.remove();
            resolve(false);
        });
    });
}

window.addEventListener("DOMContentLoaded", async () => {

    // Preloader handler
    links.forEach(function (link) {
        link.addEventListener("click", function () {
            setTimeout(function () {
                preloader.classList.remove("preloader_hidden");
                lottieLogo.classList.remove("logoLottie_hidden");
                animation.play();
            }, 800);
        });
    });

    window.addEventListener("load", function () {
        setTimeout(function () {
            preloader.classList.add("preloader_hidden");
            lottieLogo.classList.add("logoLottie_hidden");
            animation.pause();
        }, 1000);
    });

    // Logout handler
    logOutBtn.addEventListener("click", () => {
        localStorage.clear();
    });

    logoutItem.addEventListener("click", async (event) => {
        event.preventDefault();
        const confirmLogout = await confirmPopup("Вы уверены, что хотите выйти?");
        if (confirmLogout == true) {
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