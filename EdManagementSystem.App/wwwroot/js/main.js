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

let path = window.location.pathname.split('/')[2];

menuItems.forEach(item => {
    if (item.parentElement.getAttribute('data-path') === path) {
        item.classList.add('aside-menu__item_active');
    }
});


// Change content
//$(".aside-menu__item").click(function () {
//    var path = $(this).data("path");
//    if (path) {
//        $.ajax({
//            url: "/dashboard/GetPartialView?path=" + path,
//            type: "GET",
//            success: function (data) {
//                $(".dashboard-content-container").html(data);
//            },
//            error: function () {
//                alert("Error loading data");
//            }
//        });
//    }
//});