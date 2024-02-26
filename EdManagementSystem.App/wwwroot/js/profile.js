// APIs
const getTeacherByEmailAPI = "https://localhost:44370/api/Teachers/GetTeacherByEmail/";
const getSquadsCount = "https://localhost:44370/api/Teachers/GetSquadsCount/";
const getStudentsCount = "https://localhost:44370/api/Teachers/GetStudentsCount/";
const getSquads = "https://localhost:44370/api/Teachers/GetSquadsOfTeacher/";
const getCourses = "https://localhost:44370/api/Teachers/GetCoursesOfTeacher/";

$(document).ready(function () {
    var userId = $('#userId').val();

    // Получение данных из localStorage
    var teacherInfo = JSON.parse(localStorage.getItem('teacherInfo'));
    var squadsCount = localStorage.getItem('squadsCount');
    var studentsCount = localStorage.getItem('studentsCount');
    var courses = JSON.parse(localStorage.getItem('courses'));
    var squads = JSON.parse(localStorage.getItem('squads'));

    if (teacherInfo && squadsCount && studentsCount && courses && squads) {
        // Если данные уже есть в localStorage, использовать их
        displayTeacherInfo(teacherInfo);
        displaySquadsCount(squadsCount);
        displayStudentsCount(studentsCount);
        displayCoursesList(courses);
        displaySquadsList(squads);
    } else {
        // Если данных нет в localStorage, отправить запрос на сервер
        getDataFromServer(userId);
    }
});

function displayTeacherInfo(teacherInfo) {
    $("#personal-account-info__name").text(teacherInfo.fio);
    $("#personal-account-info__post").text(teacherInfo.post);
    $("#personal_account_info__rate").text(teacherInfo.rate);
    $("#personal-account-info__place").text(teacherInfo.address);
    $("#personal-account-info__phone-number").text(teacherInfo.phoneNumber);
    $("#experience").text(teacherInfo.experience);

    let registrationDate = teacherInfo.regDate;
    let formattedDate = new Date(registrationDate).toLocaleDateString("en-US");
    $("#regDate").text(formattedDate);
}

function displaySquadsCount(squadsCount) {
    $("#squadsCount").text(squadsCount);
}

function displayStudentsCount(studentsCount) {
    $("#studentsCount").text(studentsCount);
}

function displayCoursesList(courses) {
    var ul = $("#coursesList");
    courses.forEach(function (item) {
        var li = document.createElement("li");
        li.textContent = item;
        li.classList.add("courses-list__item");
        ul.append(li);
    });
}

function displaySquadsList(squads) {
    var ul = $("#squadsList");
    squads.forEach(function (item) {
        var li = document.createElement("li");
        li.textContent = item;
        li.classList.add("courses-list__item");
        ul.append(li);
    });
};

function getDataFromServer(userId) {
    // Main Info about teacher
    $.get(getTeacherByEmailAPI + userId, function (data) {
        localStorage.setItem('teacherInfo', JSON.stringify(data));

        //if (data.avatar) {
        //    $("#avatar_icon").attr("src", data.avatar);
        //}

        $("#personal-account-info__name").text(data.fio);
        $("#personal-account-info__post").text(data.post);
        $("#personal_account_info__rate").text(data.rate);
        $("#personal-account-info__place").text(data.address);
        $("#personal-account-info__phone-number").text(data.phoneNumber);
        $("#experience").text(data.experience);

        let registrationDate = data.regDate;
        let formattedDate = new Date(registrationDate).toLocaleDateString("en-US");
        $("#regDate").text(formattedDate);

    }).fail(function (error) {
        // Error handling
        console.log(error);
    });

    // Additional statistics info
    $.get(getSquadsCount + userId, function (data) {
        localStorage.setItem('squadsCount', data);
        $("#squadsCount").text(data);
    }).fail(function (error) {
        // Error handling
        console.log(error);
    });

    $.get(getStudentsCount + userId, function (data) {
        localStorage.setItem('studentsCount', data);
        $("#studentsCount").text(data);
    }).fail(function (error) {
        // Error handling
        console.log(error);
    });

    // Courses and squads
    $.get(getCourses + userId, function (data) {
        localStorage.setItem('courses', JSON.stringify(data));
        var ul = $("#coursesList");
        data.forEach(function (item) {
            var li = document.createElement("li");
            li.textContent = item;
            li.classList.add("courses-list__item");
            ul.append(li);
        });
    }).fail(function (error) {
        // Error handling
        console.log(error);
    });

    $.get(getSquads + userId, function (data) {
        localStorage.setItem('squads', JSON.stringify(data));
        var ul = $("#squadsList");
        data.forEach(function (item) {
            var li = document.createElement("li");
            li.textContent = item;
            li.classList.add("courses-list__item");
            ul.append(li);
        });
    }).fail(function (error) {
        // Error handling
        console.log(error);
    });
}