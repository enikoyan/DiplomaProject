$(document).ready(function () {
    $("body").on("click", ".questions-item__toggle-btn", function () {
        const btn = $(this);
        const parent = btn.closest(".questions-item");
        const answer = parent.find(".questions-item__answer");

        if (btn.text() === "+") {
            btn.text("-");
            answer.css("max-height", answer[0].scrollHeight + "px");
        } else {
            btn.text("+");
            answer.css("max-height", "0");
        }
    });

    $("#submit-btn").on("click", function (event) {
        event.preventDefault();

        var userEmail = '';

        $.ajax({
            url: '/dashboard/getCurrentUserId',
            type: 'GET',
            success: function (response) {
                userEmail = response;
                // Send question to the server
                let requestDescription = document.getElementById('question-form__text').value;

                if (!$.trim(requestDescription)) {
                    $('#warningMessage').text('Поле не может быть пустымм!');
                }
                else if (requestDescription.length < 250) {
                    $('#warningMessage').text('Обращение должно быть минимум на 250 символов');
                }
                else {
                    let data = {
                        UserEmail: userEmail,
                        RequestDescription: requestDescription
                    };

                    fetch('https://localhost:44370/api/TechSupports/CreateRequest', {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json'
                        },
                        body: JSON.stringify(data)
                    })
                        .then(response => {
                            if (response.ok) {
                                $('#warningMessage').css('color', 'green');
                                $('#warningMessage').text('Обращение успешно отправлено!');
                                //console.log('Запрос успешно отправлен');
                                document.getElementById('question-form__text').value = '';
                            } else {
                                console.error('Ошибка при отправке запроса');
                            }
                        })
                        .catch(error => {
                            console.error('Ошибка: ', error);
                        });
                }
            },
            error: function (error) {
                console.log('Error: ' + error);
            }
        });
    });
});