/* Hide and Show aside menu */
const button = document.querySelector('.aside-menu-toggle');
const targetElement = document.querySelector(".aside");

button.addEventListener('click', () => {
  if (targetElement.classList.contains('aside_closed')) {
    targetElement.classList.remove('aside_closed');
  } else {
    targetElement.classList.add('aside_closed');
  }
});

/* Dynamic colSpan for table */
var tableElement = document.querySelector("#student-count");
const checkScreenWidth = () => {
  if (window.innerWidth < 600) {
    tableElement.colSpan = "1";
  } else {
    tableElement.colSpan = "4";
  }
}
window.addEventListener('resize', checkScreenWidth);
checkScreenWidth();