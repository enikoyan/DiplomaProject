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
// toggleBtn.addEventListener('click', toggleAsideMenu);
// const checkScreenWidth = () => {
//   if (window.innerWidth < 768) {
//     targetElement.classList.add('aside_closed');
//   }
// }
// window.addEventListener('resize', checkScreenWidth);
// checkScreenWidth();