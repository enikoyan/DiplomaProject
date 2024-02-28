// Aside menu buttons
const menuItems = document.querySelectorAll('.aside-menu__item');

function refreshActiveBtn() {
    let path = window.location.pathname.split('/')[2];

    menuItems.forEach(item => {
        item.classList.remove('aside-menu__item_active');
    });

    menuItems.forEach(item => {
        if (item.parentElement.getAttribute('data-path') === path) {
            item.classList.add('aside-menu__item_active');
        }
    });
}

/* Hide and Show aside menu */
const aside = document.querySelector('.aside');
const asideToggle = document.querySelector('.aside-menu-toggle');

var key = 'asideMenuOpened';

if (localStorage.getItem(key) === 'true') {
    aside.classList.remove('aside_closed');
} else {
    aside.classList.add('aside_closed');
}

asideToggle.addEventListener('click', () => {
    if (aside.classList.contains('aside_closed')) {
        aside.classList.remove('aside_closed');
        localStorage.setItem(key, 'true');
    } else {
        aside.classList.add('aside_closed');
        localStorage.setItem(key, 'false');
    }
});

$(document).ready(function () {

    //// Menu buttons click handler
    //$('.aside-menu__item').click(function (e) {
    //    e.preventDefault();
    //    var url = $(this).attr('href');

    //    // Load content without refreshing the page
    //    $('.dashboard-content-container').load(url + " .main > *", function () {
    //        history.pushState(null, null, url);
    //        refreshActiveBtn();
    //    });
    //});

    // URL changing handler
    window.onpopstate = function () {
        //var url = location.pathname;
        //$('.dashboard-content-container').load(url + " .main > *");
        refreshActiveBtn();
    };
});

// Set active btn
refreshActiveBtn();