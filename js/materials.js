// Получение данных из JSON-файла
fetch("../js/json/materials-list.json")
  .then((response) => response.json())
  .then((data) => {
    // Список всех файлов
    const foundMaterials = document.querySelector(".found-materials");

    for (let key in data) {
      data[key].forEach((material) => {
        // Файловый контейнер
        const foundMaterials_item = document.createElement("div");
        foundMaterials_item.classList.add("found-materials__item");
        foundMaterials.appendChild(foundMaterials_item);

        /* Информация о файле */
        const foundMaterials_info = document.createElement("div");
        foundMaterials_info.classList.add("found-materials__info");

        // Картинка
        const foundMaterialsIcon = document.createElement("img");
        foundMaterialsIcon.setAttribute("style", "width: 40px");
        foundMaterialsIcon.setAttribute(
          "src",
          `../icons/file-type-icons/${material.type}-icon.svg`
        );
        foundMaterialsIcon.setAttribute("alt", `${material.type}-icon`);
        foundMaterialsIcon.classList.add("found-materials__icon");

        // Текст
        const foundMaterialsInfoText = document.createElement("div");
        foundMaterialsInfoText.classList.add("found-materials__info_text");

        const foundMaterials_title = document.createElement("span");
        foundMaterials_title.classList.add("found-materials__title");
        foundMaterials_title.textContent = material.name;

        const foundMaterials_weight = document.createElement("span");
        foundMaterials_weight.classList.add("found-materials__weight");
        foundMaterials_weight.textContent = material.size;

        foundMaterialsInfoText.appendChild(foundMaterials_title);
        foundMaterialsInfoText.appendChild(foundMaterials_weight);

        foundMaterials_info.appendChild(foundMaterialsIcon);
        foundMaterials_info.appendChild(foundMaterialsInfoText);

        /* Кнопки управления */
        const foundMaterials_btns = document.createElement("div");
        foundMaterials_btns.classList.add("found-materials__btns");

        // Кнопка получения информации о файле
        const foundMaterials_btn_getInfo = document.createElement("div");
        foundMaterials_btn_getInfo.classList.add(
          "found-materials__btn",
          "found-materials__btn_get-info"
        );
        const infoIcon = document.createElement("img");
        infoIcon.setAttribute("src", "../icons/info-icon.svg");
        infoIcon.setAttribute("style", "width: 30px");
        infoIcon.setAttribute("alt", "info-icon");
        foundMaterials_btn_getInfo.appendChild(infoIcon);

        // Кнопка удаления файла
        const foundMaterials_btn_delete = document.createElement("div");
        foundMaterials_btn_delete.classList.add(
          "found-materials__btn",
          "found-materials__btn_delete"
        );
        const deleteIcon = document.createElement("img");
        deleteIcon.setAttribute("src", "../icons/delete-icon.svg");
        deleteIcon.setAttribute("style", "width: 30px");
        deleteIcon.setAttribute("alt", "info-icon");
        foundMaterials_btn_delete.appendChild(deleteIcon);

        // Добавление кнопок в контейнер кнопок управления
        foundMaterials_btns.appendChild(foundMaterials_btn_getInfo);
        foundMaterials_btns.appendChild(foundMaterials_btn_delete);

        // Добавление в контейнер файла информации и кнопок
        foundMaterials_item.appendChild(foundMaterials_info);
        foundMaterials_item.appendChild(foundMaterials_btns);

        // Показать информацию о файле
        foundMaterials_btn_getInfo.addEventListener("click", () => {
          showAlert(material);
        });
        // Функция для отображения информации в alert
        function showAlert(material) {
          alert(`
            Название файла: ${material.name}
            Размер файла: ${material.size}
            Дата добавления: ${material.date_added}
            Тип файла: ${material.type}`);
        }

        // Удалить файл
        foundMaterials_btn_delete.addEventListener("click", function () {
          // Получение уникального идентификатора объекта
          const materialId = material.id;

          // Удаление объекта из JSON-файла
          delete data[key].find((material) => material.id === materialId);

          // ваш код сохранения изменений в JSON-файле

          // Удаление соответствующего контейнера файла из HTML-разметки
          foundMaterials_item.remove();
        });
      });
    }
  });
