/* Dynamic colSpan for table */
var tableElement = document.querySelector("#student-count");
const checkScreenWidth = () => {
  if (window.innerWidth < 600) {
    tableElement.colSpan = "1";
  } else {
    tableElement.colSpan = "4";
  }
};
window.addEventListener("resize", checkScreenWidth);
checkScreenWidth();
