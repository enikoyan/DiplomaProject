// Получение данных из JSON-файла
fetch("../js/json/homework-list.json")
  .then((response) => response.json())
  .then((data) => {
    // Список всех ДЗ
    const homeworks = document.querySelector(".homeworks");

    for (let key in data) {
      data[key].forEach((homework) => {
        /* Блок домашнего задания */
        const homeworksItem = document.createElement("div");
        homeworksItem.classList.add("homeworks__item");
        homeworks.appendChild(homeworksItem);

        // Название ДЗ
        const homeworksTitle = document.createElement("h3");
        homeworksTitle.textContent = homework.homework_title;
        homeworksTitle.classList.add("homeworks__title");

        // Информация о ДЗ
        const homeworksInfo = document.createElement("div");
        homeworksInfo.classList.add("homeworks__info");
        const homeworksText = document.createElement("p");
        homeworksText.textContent = homework.homework_text;
        homeworksText.classList.add("homeworks__text");

        const homeworksAddDate = document.createElement("span");
        homeworksAddDate.textContent = `Дата добавления: ${homework.homework_addDate}`;
        homeworksAddDate.classList.add("homeworks_date", "homeworks__date_add");

        const homeworksDeadline = document.createElement("span");
        homeworksDeadline.textContent = `Срок сдачи: ${homework.homework_deadline}`;
        homeworksDeadline.classList.add(
          "homeworks_date",
          "homeworks__date_deadline"
        );

        homeworksInfo.appendChild(homeworksText);
        homeworksInfo.appendChild(homeworksAddDate);
        homeworksInfo.appendChild(homeworksDeadline);

        // Прикрепленные файлы
        const homeworksFiles = document.createElement("div");
        homeworksFiles.classList.add("homeworks__files");
        const homeworksFilesTitle = document.createElement("h3");
        homeworksFilesTitle.textContent = "Прикрепленные файлы:";
        homeworksFiles.appendChild(homeworksFilesTitle);

        // Получение прикрепленных файлов
        homework.homework_files.forEach((file) => {
          const link = document.createElement("a");
          link.href = file;
          link.setAttribute("download", "download");

          // Получение названия файла
          const fileName = file.split("/").pop();

          link.textContent = fileName;
          homeworksFiles.appendChild(link);
        });

        // Создание кнопки скачивания архивом
        const homeworksDownloadAllBtn = document.createElement("button");
        homeworksDownloadAllBtn.classList.add(
          "custom-btn",
          "homeworks__download-all-btn"
        );
        homeworksDownloadAllBtn.textContent = "Скачать всё";
        homeworksFiles.appendChild(homeworksDownloadAllBtn);

        // Добавление компонентов в блок домашнего задания
        homeworksItem.appendChild(homeworksTitle);
        homeworksItem.appendChild(homeworksInfo);
        homeworksItem.appendChild(homeworksFiles);

        // Создание кнопки для просмотра прикрепленных ответов
        const homeworksShowAttachedAnswers = document.createElement("button");
        homeworksShowAttachedAnswers.textContent = "Просмотреть ответы";
        homeworksShowAttachedAnswers.classList.add(
          "custom-btn",
          "homeworks__show-answers-btn"
        );
        homeworksItem.appendChild(homeworksShowAttachedAnswers);
      });
    }
  });
