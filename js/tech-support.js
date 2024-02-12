// Создание часто задаваемых вопросов
fetch("../js/json/answers-list.json")
  .then((response) => response.json())
  .then((data) => {
    // Контейнер вопросов
    const questionsContainer = document.querySelector(".questions");
    data.forEach((question) => {
      // Блок вопроса
      const questionsItem = document.createElement("div");
      questionsItem.classList.add("questions-item");
      questionsContainer.appendChild(questionsItem);

      // Заголовок вопроса
      const questionsItemHeader = document.createElement("div");
      questionsItemHeader.classList.add("questions-item__header");
      const questionsItemTitle = document.createElement("h4");
      questionsItemTitle.classList.add("questions-item__title");
      questionsItemTitle.textContent = question.question;
      const questionsItemToggleBtn = document.createElement("button");
      questionsItemToggleBtn.classList.add("questions-item__toggle-btn");
      questionsItemToggleBtn.textContent = "+";

      questionsItemHeader.appendChild(questionsItemTitle);
      questionsItemHeader.appendChild(questionsItemToggleBtn);

      // Вопрос
      const questionsItemAnswer = document.createElement("p");
      questionsItemAnswer.classList.add("questions-item__answer");
      questionsItemAnswer.textContent = question.answer;

      // Добавление компонентов в блок
      questionsItem.appendChild(questionsItemHeader);
      questionsItem.appendChild(questionsItemAnswer);

      // Скрытие и демонстрация ответа на вопрос
      questionsItemToggleBtn.addEventListener("click", () => {
        if (questionsItemToggleBtn.textContent === "+") {
          questionsItemToggleBtn.textContent = "-";
          questionsItemAnswer.style.maxHeight =
            questionsItemAnswer.scrollHeight + "px";
        } else {
          questionsItemToggleBtn.textContent = "+";
          questionsItemAnswer.style.maxHeight = "0";
        }
      });
    });
  });
