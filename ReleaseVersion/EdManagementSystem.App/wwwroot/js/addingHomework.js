const radioBtn = document.getElementById("homeworkOffDeadlineBtn");
const optionsContainerCourses = document.getElementById(
  "options-container-courses"
);
const optionsContainerSquads = document.getElementById(
  "options-container-squads"
);
const dropContainer = document.getElementById("dropcontainer");
const fileInput = document.getElementById("attachedFiles");
const homeworkForm = document.getElementById("addHomeworkForm");
const homeworkTitleTb = document.getElementById("homeworkTitleTb");
const homeworkDescTb = document.getElementById("homeworkDescTb");
const homeworkDeadlineTb = document.getElementById("homeworkDeadlineTb");
const filterSelector = document.getElementById("filter-select");
const goBackBtn = document.getElementById("go-back-btn");
var isTouched = false;
var isDeadlineExist = true;
const createHomeworkApiUrl =
  "http://localhost:8001/api/Homeworks/CreateHomework";

// Preloader
var preloader = document.querySelector(".preloader");
var lottieLogo = document.getElementById("logoLottie");
var links = document.querySelectorAll("a");

radioBtn.addEventListener("click", function (event) {
  if (this.checked) {
    homeworkDeadlineTb.disabled = true;
    isDeadlineExist = false;
  } else {
    homeworkDeadlineTb.disabled = false;
    isDeadlineExist = true;
  }
});

async function selectOptionHandler(selectedOption) {
  const value = selectedOption.getAttribute("data-value");
  //const valueText = selectedOption.textContent;
  const customSelect = selectedOption.closest(".custom-select");
  const customSelectTitle = customSelect.querySelector(".custom-select__title");

  customSelectTitle.textContent = selectedOption.textContent;
  customSelect.setAttribute("data-target", value);

  await switchSelects(value);
}

async function switchSelects(value) {
  switch (value) {
    case "searchBySquads": {
      optionsContainerCourses.style.display = "none";
      optionsContainerSquads.style.display = "flex";

      break;
    }
    case "searchByCourses": {
      optionsContainerSquads.style.display = "none";
      optionsContainerCourses.style.display = "flex";

      break;
    }
  }
}

async function selectHandler(container) {
  const children = Array.from(container.children);

  children[0].classList.toggle("custom-select__btn_active");
  children[1].classList.toggle("custom-options_disabled");
}

// Message popup handler
async function createMessagePopup(messageStatus, messageText) {
  try {
    document.querySelector(".custom-alert").remove();
  } catch {}

  let popupHTML = `<div class="custom-alert">
            <img class="custom-alert__icon" src="../icons/message-icons/${messageStatus}-message.svg" data-status:"${messageStatus}"/>
            <p class="custom-alert__message">${messageText}</p>
            <span class="custom-alert__close-btn close-btn"></span>
        </div>`;

  const popupElement = document.createElement("div");
  popupElement.innerHTML = popupHTML;
  document.body.appendChild(popupElement);

  let closeButtonClicked = false;

  document
    .querySelector(".custom-alert__close-btn")
    .addEventListener("click", async () => {
      closeButtonClicked = true;
      popupElement.remove();
    });

  setTimeout(async () => {
    if (!closeButtonClicked) {
      await destroyMessagePopup();
    }
  }, 4000);
}

function confirmPopup(messageText) {
  return new Promise((resolve, reject) => {
    try {
      document.querySelector(".custom-alert").remove();
    } catch {}

    const popupElement = document.createElement("div");
    const popupHTML = `<div class="custom-alert custom-alert_confirm">
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

    popupElement
      .querySelector('[data-returnValue="yes"]')
      .addEventListener("click", function () {
        popupElement.remove();
        resolve(true);
      });

    popupElement
      .querySelector('[data-returnValue="no"]')
      .addEventListener("click", function () {
        popupElement.remove();
        resolve(false);
      });
  });
}

async function destroyMessagePopup() {
  const popup = document.querySelector(".custom-alert");
  popup.classList.add("hide");

  setTimeout(() => {
    popup.remove();
  }, 500);
}

document.addEventListener("DOMContentLoaded", function () {
  // File drug and drop
  dropContainer.addEventListener(
    "dragover",
    (e) => {
      e.preventDefault();
    },
    false
  );
  dropContainer.addEventListener("dragenter", () => {
    dropContainer.classList.add("drag-active");
  });
  dropContainer.addEventListener("dragleave", () => {
    dropContainer.classList.remove("drag-active");
  });
  dropContainer.addEventListener("drop", (e) => {
    e.preventDefault();
    dropContainer.classList.remove("drag-active");
    fileInput.files = e.dataTransfer.files;
  });

  // Form handler
  homeworkForm.addEventListener("submit", async (e) => {
    e.preventDefault();
    isTouched = true;

    // Verify if all the inputs are filled
    let isCheckPassed = await checkFilled();

    if (isCheckPassed) {
      homeworkTitleTb.classList.toggle("custom-input_correct");
      homeworkDescTb.classList.add("custom-input_correct");

      await createHomework();
    }
  });

  async function createHomework() {
    setTimeout(function () {
      preloader.classList.remove("preloader_hidden");
      lottieLogo.classList.remove("logoLottie_hidden");
      animation.play();
    }, 400);

    const formData = new FormData();
    const checkedValues = [];

    // Adding files
    const attachedFiles = fileInput.files;
    if (attachedFiles.length == 0) {
      formData.append("files", null);
    } else {
      for (let i = 0; i < attachedFiles.length; i++) {
        formData.append("files", attachedFiles[i]);
      }
    }

    // Arguments
    formData.append("groupBy", filterSelector.getAttribute("data-target"));
    switch (filterSelector.getAttribute("data-target")) {
      case "searchByCourses": {
        const checkedCheckboxes = optionsContainerCourses.querySelectorAll(
          'input[type="checkbox"]:checked'
        );
        checkedCheckboxes.forEach((checkbox) => {
          checkedValues.push(checkbox.value);
        });

        formData.append("foreignKeys", checkedValues);
        break;
      }
      case "searchBySquads": {
        const checkedCheckboxes = optionsContainerSquads.querySelectorAll(
          'input[type="checkbox"]:checked'
        );
        checkedCheckboxes.forEach((checkbox) => {
          checkedValues.push(checkbox.value);
        });

        formData.append("foreignKeys", checkedValues);
        break;
      }
    }

    formData.append("title", homeworkTitleTb.value);
    formData.append("description", homeworkDescTb.value);

    let noteValue = document.getElementById("homeworkNoteTb").value;

    formData.append("note", noteValue ? noteValue : "");

    if (isDeadlineExist) {
      formData.append("deadline", homeworkDeadlineTb.value);
    }

    // Query options
    const requestOptions = {
      method: "POST",
      body: formData,
    };

    // Calling API method
    fetch(createHomeworkApiUrl, requestOptions)
      .then(async (response) => {
        if (!response.ok) {
          return response.text().then(async (text) => {
            await createMessagePopup("warning", text);
          });
        } else {
          return response.text();
        }
      })
      .then(async (data) => {
        if (data) {
          switch (filterSelector.getAttribute("data-target")) {
            case "searchByCourses": {
              for (var i = 0; i < checkedValues.length; i++) {
                localStorage.removeItem(
                  `homeworksData / Course / ${checkedValues[i]}`
                );
              }
              break;
            }
            case "searchBySquads": {
              for (var i = 0; i < checkedValues.length; i++) {
                localStorage.removeItem(
                  `homeworksData / Squad / ${checkedValues[i]}`
                );
              }
              break;
            }
          }

          setTimeout(function () {
            preloader.classList.add("preloader_hidden");
            lottieLogo.classList.add("logoLottie_hidden");
            animation.pause();
          }, 400);

          var result = await confirmPopup(
            "Домашнее задание успешно выдано! Хотите вернуться назад к списку Д/З?"
          );
          if (result == true) {
            window.location.href =
              "http://localhost:8080/dashboard/homeworks";
          } else if (result == false) {
            location.reload();
          }
        }
      })
      .catch(async (error) => {
        await createMessagePopup("error", error.message);
      });
  }

  async function checkFilled() {
    const checkboxList = document.querySelectorAll(".custom-checkbox:checked");
    if (checkboxList.length == 0) {
      await createMessagePopup(
        "warning",
        "Выберите хотя бы одну группу или курс!"
      );
      return false;
    } else {
      if (homeworkTitleTb.value.trim().length === 0) {
        homeworkTitleTb.classList.add("custom-input_errored");
        await createMessagePopup(
          "warning",
          "Заполните обязательные поля (тема, описание)!"
        );
        return false;
      } else if (homeworkDescTb.value.trim().length === 0) {
        homeworkTitleTb.classList.add("custom-input_correct");
        homeworkDescTb.classList.add("custom-input_errored");
        await createMessagePopup(
          "warning",
          "Заполните обязательные поля (тема, описание)!"
        );
        return false;
      } else if (homeworkDeadlineTb.value == "" && isDeadlineExist) {
        await createMessagePopup("warning", "Укажите срок сдачи!");
      } else {
        return true;
      }
    }
  }

  goBackBtn.addEventListener("click", async () => {
    window.location.href = "http://localhost:8080/dashboard/homeworks";
  });
});
