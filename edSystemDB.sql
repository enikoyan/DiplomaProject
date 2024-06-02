-- phpMyAdmin SQL Dump
-- version 5.0.4deb2+deb11u1
-- https://www.phpmyadmin.net/
--
-- Хост: localhost:3306
-- Время создания: Июн 02 2024 г., 17:05
-- Версия сервера: 10.0.28-MariaDB-2+b1
-- Версия PHP: 7.4.33

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- База данных: `edSystemDB`
--

-- --------------------------------------------------------

--
-- Структура таблицы `Attendance`
--

CREATE TABLE `Attendance` (
  `id` char(36) NOT NULL,
  `added_date` datetime NOT NULL,
  `squad_id` int(11) NOT NULL,
  `week_date` char(8) NOT NULL,
  `file_id` char(36) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Структура таблицы `Course`
--

CREATE TABLE `Course` (
  `course_id` int(11) NOT NULL,
  `course_name` varchar(255) NOT NULL,
  `option_value` varchar(255) NOT NULL,
  `course_addDate` datetime NOT NULL,
  `course_tutor` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Дамп данных таблицы `Course`
--

INSERT INTO `Course` (`course_id`, `course_name`, `option_value`, `course_addDate`, `course_tutor`) VALUES
(2, 'C++ программирование', 'programming-c-plus-plus', '2024-02-25 17:39:13', 3),
(3, 'Веб-дизайн', 'web-design', '2024-02-25 17:39:13', 3),
(4, 'Английский язык в ПП', 'english-for-pp', '2024-02-25 17:39:13', 4),
(5, 'Лингвистика для ПП', 'lingvistik-for-pp', '2024-02-25 17:39:13', 4),
(6, 'Робототехника', 'robotics', '2024-02-25 17:45:33', 3),
(7, 'Курсы подготовки к ЕГЭ', 'ege', '2024-02-25 19:13:14', 4);

-- --------------------------------------------------------

--
-- Структура таблицы `File`
--

CREATE TABLE `File` (
  `id` char(36) NOT NULL,
  `title` varchar(255) NOT NULL,
  `type` varchar(10) NOT NULL,
  `date_added` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Структура таблицы `Homework`
--

CREATE TABLE `Homework` (
  `id` int(11) NOT NULL,
  `homework_id` char(36) NOT NULL,
  `course_id` int(11) NOT NULL,
  `squad_id` int(11) DEFAULT NULL,
  `date_added` datetime NOT NULL,
  `deadline` datetime DEFAULT NULL,
  `title` varchar(255) NOT NULL,
  `description` varchar(1024) DEFAULT NULL,
  `note` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Структура таблицы `Homework_File`
--

CREATE TABLE `Homework_File` (
  `id` int(11) NOT NULL,
  `homework_id` char(36) NOT NULL,
  `file_id` char(36) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Структура таблицы `Material`
--

CREATE TABLE `Material` (
  `id` int(11) NOT NULL,
  `id_file` char(36) NOT NULL,
  `id_course` int(11) NOT NULL,
  `id_squad` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Структура таблицы `Recovery`
--

CREATE TABLE `Recovery` (
  `id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `date` datetime NOT NULL,
  `confirmed` tinyint(1) NOT NULL DEFAULT '0',
  `user_key` char(36) NOT NULL,
  `server_key` char(36) NOT NULL,
  `expire_time` time NOT NULL DEFAULT '00:05:00'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Дамп данных таблицы `Recovery`
--

INSERT INTO `Recovery` (`id`, `user_id`, `date`, `confirmed`, `user_key`, `server_key`, `expire_time`) VALUES
(8, 3, '2024-06-02 10:04:20', 0, 'f14ece2c-eaaa-4408-9e6c-1c22f2bf8043', 'f3346b40-b1f1-4133-9e6d-83736f222cb1', '00:05:00'),
(9, 3, '2024-06-02 10:04:32', 0, '217f6b0e-86c5-4285-a641-e0a46538c31c', '3c6d34ec-1b13-49a2-b4af-b280c755093b', '00:05:00'),
(10, 3, '2024-06-02 10:06:10', 0, 'cd4af50e-3ab3-4c78-b7a9-78325b150ec0', 'eb443806-daac-495d-adb5-d7923cac6c96', '00:05:00'),
(11, 3, '2024-06-02 10:09:34', 0, '5a7fc394-a1ad-49e4-8211-85c66fcbb30f', '739843ab-ddfc-4447-b2e5-828076fc530d', '00:05:00'),
(12, 3, '2024-06-02 10:13:08', 0, '3de9ed93-5956-44db-966a-8b6b0419d8d8', 'c3d64e38-066b-419d-83af-acc7b20f886c', '00:05:00'),
(13, 3, '2024-06-02 10:15:29', 0, '10aaec5f-f0c4-4e83-b0e8-7729dd5bc46a', '8bcc2629-0106-45c4-8048-f04f6f48c892', '00:05:00'),
(14, 3, '2024-06-02 11:02:46', 0, 'c4ab54ec-6320-44af-b1f9-193868aa1d8b', '9a58f24d-fe4f-455b-811f-c850fb7cc802', '00:05:00'),
(15, 3, '2024-06-02 11:09:55', 0, 'c368207d-96e1-4e79-bdd1-49f3d05f79b1', '159d9154-c249-4767-bc08-f7dd95365bec', '00:05:00'),
(16, 3, '2024-06-02 11:18:55', 1, '55707596-6300-4e0e-8b2a-4bb74c5d1aa3', 'c8d02921-5048-4e49-85fc-a17f5e61a000', '00:05:00'),
(17, 3, '2024-06-02 11:21:35', 1, 'd6fce619-722c-4734-8035-887ebee8c70e', '741a7b78-1c62-4d1f-a3bc-5e320022f45e', '00:05:00'),
(18, 3, '2024-06-02 11:23:28', 1, '78e8981a-48d2-4af4-92bd-ad21e14f8e9c', '71d76383-d871-4126-976f-4cc5eac96a97', '00:05:00'),
(19, 3, '2024-06-02 11:39:47', 1, '6731dbf5-cce0-4c0d-9e41-89b7b7d54dc1', 'fa85dd62-bccb-4a7f-8e6a-195e62f46715', '00:05:00');

-- --------------------------------------------------------

--
-- Структура таблицы `Schedule`
--

CREATE TABLE `Schedule` (
  `id` int(11) NOT NULL,
  `teacher_id` int(11) NOT NULL,
  `squad_id` int(11) NOT NULL,
  `weekday` enum('Понедельник','Вторник','Среда','Четверг','Пятница','Суббота','Воскресенье') NOT NULL,
  `date` date NOT NULL,
  `timeline_start` time NOT NULL,
  `timeline_end` time NOT NULL,
  `place` varchar(255) NOT NULL,
  `note` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Дамп данных таблицы `Schedule`
--

INSERT INTO `Schedule` (`id`, `teacher_id`, `squad_id`, `weekday`, `date`, `timeline_start`, `timeline_end`, `place`, `note`) VALUES
(1, 3, 3, 'Понедельник', '2024-05-20', '08:00:00', '10:00:00', 'Аудитория 115', 'Занятие программирования №12'),
(2, 3, 4, 'Понедельник', '2024-05-23', '10:30:00', '12:30:00', 'Аудитория 102', 'Занятие программирования №24'),
(3, 3, 5, 'Вторник', '2024-05-21', '08:30:00', '11:30:00', 'Аудитория 201', 'Дизайн интерфейсов'),
(4, 3, 6, 'Среда', '2024-05-21', '09:00:00', '11:00:00', 'Аудитория 301', 'Тестирование продукта'),
(5, 3, 7, 'Среда', '2024-05-25', '12:00:00', '14:00:00', 'Аудитория 302', 'CI / CD контейнеры'),
(6, 3, 3, 'Четверг', '2024-05-26', '13:00:00', '15:00:00', 'Аудитория 401', 'SOLID принципы'),
(7, 3, 5, 'Пятница', '2024-05-20', '08:30:00', '10:30:00', 'Аудитория 501', 'SOLID принципы (занятие 2)'),
(8, 3, 6, 'Пятница', '2024-05-24', '11:00:00', '13:00:00', 'Аудитория 502', 'Архитектура проектов'),
(9, 3, 7, 'Суббота', '2024-05-22', '09:30:00', '11:30:00', 'Аудитория 601', 'Разработка на QT'),
(10, 3, 3, 'Воскресенье', '2024-05-23', '10:30:00', '12:30:00', 'Аудитория 701', 'Разработка на Qt Quick'),
(11, 3, 5, 'Воскресенье', '2024-05-26', '13:30:00', '15:30:00', 'Аудитория 702', 'Знакомство с QML');

-- --------------------------------------------------------

--
-- Структура таблицы `SocialMedia`
--

CREATE TABLE `SocialMedia` (
  `id` int(11) NOT NULL,
  `id_teacher` int(11) NOT NULL,
  `socialMedia_name` enum('Telegram','Vk','Discord','Facebook') NOT NULL,
  `socialMedia_url` varchar(2083) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Дамп данных таблицы `SocialMedia`
--

INSERT INTO `SocialMedia` (`id`, `id_teacher`, `socialMedia_name`, `socialMedia_url`) VALUES
(2, 3, 'Telegram', 'https://web.telegram.org/k/'),
(3, 3, 'Vk', 'https://vk.com/nikoyan4'),
(4, 3, 'Discord', 'https://discord.com/'),
(6, 4, 'Vk', 'https://vk.com');

-- --------------------------------------------------------

--
-- Структура таблицы `Squad`
--

CREATE TABLE `Squad` (
  `squad_id` int(11) NOT NULL,
  `squadName` varchar(255) NOT NULL,
  `option_value` varchar(255) NOT NULL,
  `addDate` datetime NOT NULL,
  `id_course` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Дамп данных таблицы `Squad`
--

INSERT INTO `Squad` (`squad_id`, `squadName`, `option_value`, `addDate`, `id_course`) VALUES
(1, 'ЩПКО-01-20', 'shchpko-01-20', '2024-02-25 18:19:09', 3),
(2, 'ЩПКО-02-20', 'shchpko-02-20', '2024-02-25 18:19:09', 3),
(3, 'ЩПКО-03-20', 'shchpko-03-20', '2024-02-25 18:19:09', 3),
(4, 'ЩПКО-01-21', 'shchpko-01-21', '2024-02-25 18:19:09', 4),
(5, 'ЩПКО-01-22', 'shchpko-01-22', '2024-02-25 18:19:09', 4),
(6, 'ЩПКО-01-23', 'shchpko-01-23', '2024-02-25 18:19:09', 5),
(7, 'ЩПКО-01-23', 'shchpko-01-23', '2024-02-25 18:19:09', 6),
(8, 'ЩПКО-04-23', 'shchpko-04-23', '2024-02-25 19:15:09', 7);

-- --------------------------------------------------------

--
-- Структура таблицы `SquadStudent`
--

CREATE TABLE `SquadStudent` (
  `id` int(11) NOT NULL,
  `id_student` int(11) NOT NULL,
  `id_squad` int(11) NOT NULL,
  `attachedDate` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Дамп данных таблицы `SquadStudent`
--

INSERT INTO `SquadStudent` (`id`, `id_student`, `id_squad`, `attachedDate`) VALUES
(3, 4, 3, '2024-02-25 22:12:01'),
(4, 4, 5, '2024-02-25 22:12:11'),
(5, 14, 3, '2024-02-28 14:09:45'),
(6, 15, 3, '2024-02-28 14:10:08'),
(7, 12, 3, '2024-02-28 14:13:01'),
(8, 10, 3, '2024-02-28 14:14:38'),
(9, 7, 7, '2024-03-04 15:00:09'),
(10, 11, 7, '2024-03-04 15:00:24'),
(14, 8, 3, '2024-05-12 20:38:43');

-- --------------------------------------------------------

--
-- Структура таблицы `Student`
--

CREATE TABLE `Student` (
  `student_id` int(11) NOT NULL,
  `fio` varchar(255) NOT NULL,
  `rate` double NOT NULL DEFAULT '4',
  `birthDate` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Дамп данных таблицы `Student`
--

INSERT INTO `Student` (`student_id`, `fio`, `rate`, `birthDate`) VALUES
(4, 'Козлов Дмитрий Александрович', 3.24, '1999-03-30'),
(5, 'Новикова Екатерина Сергеевна', 4.74, '2002-09-25'),
(6, 'Васильева Ольга Павловна', 5, '2003-01-05'),
(7, 'Морозов Артем Викторович', 4.25, '1997-12-12'),
(8, 'Кузнецов Алексей Игоревич', 3.1, '2004-06-20'),
(9, 'Смирнова Мария Андреевна', 5, '2000-08-18'),
(10, 'Павлов Денис Васильевич', 4.2, '1996-04-09'),
(11, 'Петров Петр Петрович', 5, '1998-11-20'),
(12, 'Сидорова Анна Владимировна', 5, '2003-02-03'),
(13, 'Козлов Дмитрий Александрович', 3.7, '1999-03-30'),
(14, 'Новикова Екатерина Сергеевна', 4.1, '2002-09-25'),
(15, 'Володин Евгений Витальевич', 3.2, '2024-02-28');

-- --------------------------------------------------------

--
-- Структура таблицы `Teacher`
--

CREATE TABLE `Teacher` (
  `teacher_id` int(11) NOT NULL,
  `fio` varchar(255) NOT NULL,
  `avatar` text,
  `address` varchar(255) DEFAULT NULL,
  `post` varchar(255) NOT NULL,
  `rate` double NOT NULL DEFAULT '4',
  `phone_number` char(11) DEFAULT NULL,
  `experience` int(11) DEFAULT NULL,
  `regDate` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Дамп данных таблицы `Teacher`
--

INSERT INTO `Teacher` (`teacher_id`, `fio`, `avatar`, `address`, `post`, `rate`, `phone_number`, `experience`, `regDate`) VALUES
(3, 'Никоян Эрик Ашотович', 'https://drive.usercontent.google.com/download?id=1Lgn1PvQfPJS9RfnJnMQCVmWb4qK58dS0&export=view&authuser=0', 'Бульвар Эйнштейна д.4', 'Преподаватель программирования', 5, '89015306299', 3, '2024-02-25 00:00:00'),
(4, 'Мария Петровна Кракина', 'https://drive.google.com/file/d/14BCPHFRNuBheNm6xKVQZ7tK7FGrUnmt_/view?usp=sharing', 'Улица Мичурина д.2', 'Преподаватель английского', 4, '89163414106', 1, '2024-02-25 16:42:13'),
(6, 'Евгений Витальевич Машков', '', 'ул.Ленина д.4', 'Системный администратор', 4, '89426584214', 6, '2024-02-25 19:17:00');

-- --------------------------------------------------------

--
-- Структура таблицы `TechSupport`
--

CREATE TABLE `TechSupport` (
  `id` int(11) NOT NULL,
  `id_user` int(11) NOT NULL,
  `description` text NOT NULL,
  `date_creation` datetime NOT NULL,
  `status` enum('в обработке','обработано') NOT NULL DEFAULT 'в обработке'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Дамп данных таблицы `TechSupport`
--

INSERT INTO `TechSupport` (`id`, `id_user`, `description`, `date_creation`, `status`) VALUES
(1, 3, 'Тестовый запрос, всё работает!', '2024-02-28 16:30:38', 'обработано'),
(2, 3, 'Пример2', '2024-02-28 17:02:57', 'обработано'),
(13, 3, '55', '2024-02-28 18:44:06', 'в обработке'),
(14, 3, 'gg', '2024-02-28 18:47:25', 'в обработке'),
(15, 3, '55', '2024-02-28 18:51:30', 'в обработке'),
(16, 3, 'test', '2024-02-28 19:01:53', 'в обработке'),
(19, 4, 'test', '2024-02-29 09:50:57', 'в обработке'),
(24, 3, '   gfgfg', '2024-02-29 10:01:44', 'в обработке'),
(26, 3, 'gggfgfggggfgfggggfgfggggfgfggggfgfggggfgfggggfgfggggfgfggggfgfggggfgfggggfgfggggfgfggggfgfggggfgfggggfgfggggfgfggggfgfggggfgfggggfgfggggfgfggggfgfggggfgfggggfgfggggfgfggggfgfggggfgfggggfgfggggfgfggggfgfggggfgfggggfgfggggfgfggggfgfggggfgfggggfgfggggfgfggggfgfggggfgfggggfgfg', '2024-02-29 10:07:43', 'в обработке'),
(27, 3, 'gggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggg', '2024-04-18 15:00:32', 'в обработке'),
(28, 3, 'gggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggg', '2024-04-18 15:01:18', 'в обработке'),
(29, 3, 'gfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkfgfgfkgfkgfkgfkgkfgkf', '2024-04-19 12:49:56', 'в обработке'),
(30, 3, 'bla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla blabla bla bla', '2024-05-12 16:34:02', 'в обработке'),
(31, 3, 'ПЮИЮИЮПЮПЮИЮЮПЮИЮИЮПЮПЮИЮЮПЮИЮИЮПЮПЮИЮЮПЮИЮИЮПЮПЮИЮЮПЮИЮИЮПЮПЮИЮЮПЮИЮИЮПЮПЮИЮЮПЮИЮИЮПЮПЮИЮЮПЮИЮИЮПЮПЮИЮЮПЮИЮИЮПЮПЮИЮЮПЮИЮИЮПЮПЮИЮЮПЮИЮИЮПЮПЮИЮЮПЮИЮИЮПЮПЮИЮЮПЮИЮИЮПЮПЮИЮЮПЮИЮИЮПЮПЮИЮЮПЮИЮИЮПЮПЮИЮЮПЮИЮИЮПЮПЮИЮЮПЮИЮИЮПЮПЮИЮЮПЮИЮИЮПЮПЮИЮЮПЮИЮИЮПЮПЮИЮЮПЮИЮИЮПЮПЮИЮЮПЮИЮИЮПЮПЮИЮЮПЮИЮИЮПЮПЮИЮЮПЮИЮИЮПЮПЮИЮЮПЮИЮИЮПЮПЮИЮЮПЮИЮИЮПЮПЮИЮЮПЮИЮИЮПЮПЮИЮЮПЮИЮИЮПЮПЮИЮЮПЮИЮИЮПЮПЮИЮЮПЮИЮИЮПЮПЮИЮЮПЮИЮИЮПЮПЮИЮЮПЮИЮИЮПЮПЮИЮЮПЮИЮИЮПЮПЮИЮЮПЮИЮИЮПЮПЮИЮЮПЮИЮИЮПЮПЮИЮЮПЮИЮИЮПЮПЮИЮЮПЮИЮИЮПЮПЮИЮЮПЮИЮИЮПЮПЮИЮЮПЮИЮИЮПЮПЮИЮЮПЮИЮИЮПЮПЮИЮЮПЮИЮИЮПЮПЮИЮЮПЮИЮИЮПЮПЮИЮЮПЮИЮИЮПЮПЮИЮЮПЮИЮИЮПЮПЮИЮЮПЮИЮИЮПЮПЮИЮЮ', '2024-05-26 16:30:35', 'в обработке'),
(32, 3, 'gggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggg', '2024-05-26 16:32:06', 'в обработке'),
(33, 3, 'grltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtkgrltrktlrktlrtk', '2024-05-26 16:33:14', 'в обработке'),
(34, 3, 'gggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggg', '2024-06-02 10:48:58', 'в обработке'),
(35, 3, 'test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1', '2024-06-02 10:49:54', 'в обработке'),
(36, 3, 'uuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuu', '2024-06-02 13:03:32', 'в обработке');

-- --------------------------------------------------------

--
-- Структура таблицы `User`
--

CREATE TABLE `User` (
  `user_id` int(11) NOT NULL,
  `user_email` varchar(255) NOT NULL,
  `user_password` char(60) NOT NULL,
  `user_role` enum('teacher','admin') NOT NULL DEFAULT 'teacher'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Дамп данных таблицы `User`
--

INSERT INTO `User` (`user_id`, `user_email`, `user_password`, `user_role`) VALUES
(3, 'blabla@yandex.ru', '$2a$11$DJkQpGCrVjDl1A3lzE9tW.susF62dfe2JYw1VB5eVriqO8A.LTk22', 'teacher'),
(4, 'example@gmail.com', '$2a$11$MHXaAwMb13Ez08mLaxTudORgsNKkMAwbqYu0x40d/qSajbZ3kQ/US', 'teacher'),
(5, 'example333@mail.ru', '$2a$11$/PWKqytf3RqJuJNyORIBiuhr8ThULifcZgGiavRXb2S/v1.4IMAGa', 'teacher'),
(6, 'example444@mail.ru', '$2a$11$7EJkFzF4fObl4eLO4XzM.uzh0dez4oTsFmS0Lpo7N9cmDTN522EES', 'teacher'),
(7, 'example444@mail.ru', '$2a$11$o742TDH8YWXTjRfWS0zcyeLcWgr8SJvnSTKrm2kg0M5lpW5UtME4q', 'teacher'),
(8, 'test@gmail.com', '$2a$11$UhfzOitOpM8ShM3j.S7us.8DhgXz4K1mtUMPTjFQTScYILytsGfaK', 'teacher');

--
-- Индексы сохранённых таблиц
--

--
-- Индексы таблицы `Attendance`
--
ALTER TABLE `Attendance`
  ADD PRIMARY KEY (`id`),
  ADD KEY `file_id` (`file_id`),
  ADD KEY `squad_id` (`squad_id`);

--
-- Индексы таблицы `Course`
--
ALTER TABLE `Course`
  ADD PRIMARY KEY (`course_id`),
  ADD KEY `course_tutor` (`course_tutor`);

--
-- Индексы таблицы `File`
--
ALTER TABLE `File`
  ADD PRIMARY KEY (`id`);

--
-- Индексы таблицы `Homework`
--
ALTER TABLE `Homework`
  ADD PRIMARY KEY (`id`),
  ADD KEY `course_id` (`course_id`),
  ADD KEY `squad_id` (`squad_id`),
  ADD KEY `homework_id` (`homework_id`);

--
-- Индексы таблицы `Homework_File`
--
ALTER TABLE `Homework_File`
  ADD PRIMARY KEY (`id`),
  ADD KEY `file_id` (`file_id`);

--
-- Индексы таблицы `Material`
--
ALTER TABLE `Material`
  ADD PRIMARY KEY (`id`),
  ADD KEY `id_course` (`id_course`),
  ADD KEY `id_squad` (`id_squad`),
  ADD KEY `id_file` (`id_file`);

--
-- Индексы таблицы `Recovery`
--
ALTER TABLE `Recovery`
  ADD PRIMARY KEY (`id`),
  ADD KEY `user_id` (`user_id`);

--
-- Индексы таблицы `Schedule`
--
ALTER TABLE `Schedule`
  ADD PRIMARY KEY (`id`),
  ADD KEY `squad_id` (`squad_id`),
  ADD KEY `teacher_id` (`teacher_id`);

--
-- Индексы таблицы `SocialMedia`
--
ALTER TABLE `SocialMedia`
  ADD PRIMARY KEY (`id`),
  ADD KEY `id_teacher` (`id_teacher`);

--
-- Индексы таблицы `Squad`
--
ALTER TABLE `Squad`
  ADD PRIMARY KEY (`squad_id`),
  ADD KEY `id_course` (`id_course`);

--
-- Индексы таблицы `SquadStudent`
--
ALTER TABLE `SquadStudent`
  ADD PRIMARY KEY (`id`),
  ADD KEY `id_squad` (`id_squad`),
  ADD KEY `id_student` (`id_student`);

--
-- Индексы таблицы `Student`
--
ALTER TABLE `Student`
  ADD PRIMARY KEY (`student_id`);

--
-- Индексы таблицы `Teacher`
--
ALTER TABLE `Teacher`
  ADD PRIMARY KEY (`teacher_id`);

--
-- Индексы таблицы `TechSupport`
--
ALTER TABLE `TechSupport`
  ADD PRIMARY KEY (`id`),
  ADD KEY `id_user` (`id_user`);

--
-- Индексы таблицы `User`
--
ALTER TABLE `User`
  ADD PRIMARY KEY (`user_id`);

--
-- AUTO_INCREMENT для сохранённых таблиц
--

--
-- AUTO_INCREMENT для таблицы `Course`
--
ALTER TABLE `Course`
  MODIFY `course_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT для таблицы `Homework`
--
ALTER TABLE `Homework`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=18;

--
-- AUTO_INCREMENT для таблицы `Homework_File`
--
ALTER TABLE `Homework_File`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=48;

--
-- AUTO_INCREMENT для таблицы `Material`
--
ALTER TABLE `Material`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=64;

--
-- AUTO_INCREMENT для таблицы `Recovery`
--
ALTER TABLE `Recovery`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=20;

--
-- AUTO_INCREMENT для таблицы `Schedule`
--
ALTER TABLE `Schedule`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT для таблицы `SocialMedia`
--
ALTER TABLE `SocialMedia`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT для таблицы `Squad`
--
ALTER TABLE `Squad`
  MODIFY `squad_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT для таблицы `SquadStudent`
--
ALTER TABLE `SquadStudent`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;

--
-- AUTO_INCREMENT для таблицы `Student`
--
ALTER TABLE `Student`
  MODIFY `student_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT для таблицы `TechSupport`
--
ALTER TABLE `TechSupport`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=37;

--
-- AUTO_INCREMENT для таблицы `User`
--
ALTER TABLE `User`
  MODIFY `user_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- Ограничения внешнего ключа сохраненных таблиц
--

--
-- Ограничения внешнего ключа таблицы `Attendance`
--
ALTER TABLE `Attendance`
  ADD CONSTRAINT `Attendance_ibfk_1` FOREIGN KEY (`file_id`) REFERENCES `File` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `Attendance_ibfk_2` FOREIGN KEY (`squad_id`) REFERENCES `Squad` (`squad_id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Ограничения внешнего ключа таблицы `Course`
--
ALTER TABLE `Course`
  ADD CONSTRAINT `Course_ibfk_1` FOREIGN KEY (`course_tutor`) REFERENCES `Teacher` (`teacher_id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Ограничения внешнего ключа таблицы `Homework`
--
ALTER TABLE `Homework`
  ADD CONSTRAINT `Homework_ibfk_1` FOREIGN KEY (`course_id`) REFERENCES `Course` (`course_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `Homework_ibfk_2` FOREIGN KEY (`squad_id`) REFERENCES `Squad` (`squad_id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Ограничения внешнего ключа таблицы `Homework_File`
--
ALTER TABLE `Homework_File`
  ADD CONSTRAINT `Homework_File_ibfk_1` FOREIGN KEY (`file_id`) REFERENCES `File` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Ограничения внешнего ключа таблицы `Material`
--
ALTER TABLE `Material`
  ADD CONSTRAINT `Material_ibfk_1` FOREIGN KEY (`id_course`) REFERENCES `Course` (`course_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `Material_ibfk_2` FOREIGN KEY (`id_squad`) REFERENCES `Squad` (`squad_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `Material_ibfk_3` FOREIGN KEY (`id_file`) REFERENCES `File` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Ограничения внешнего ключа таблицы `Recovery`
--
ALTER TABLE `Recovery`
  ADD CONSTRAINT `Recovery_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `User` (`user_id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Ограничения внешнего ключа таблицы `Schedule`
--
ALTER TABLE `Schedule`
  ADD CONSTRAINT `Schedule_ibfk_1` FOREIGN KEY (`squad_id`) REFERENCES `Squad` (`squad_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `Schedule_ibfk_2` FOREIGN KEY (`teacher_id`) REFERENCES `Teacher` (`teacher_id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Ограничения внешнего ключа таблицы `SocialMedia`
--
ALTER TABLE `SocialMedia`
  ADD CONSTRAINT `SocialMedia_ibfk_1` FOREIGN KEY (`id_teacher`) REFERENCES `Teacher` (`teacher_id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Ограничения внешнего ключа таблицы `Squad`
--
ALTER TABLE `Squad`
  ADD CONSTRAINT `Squad_ibfk_1` FOREIGN KEY (`id_course`) REFERENCES `Course` (`course_id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Ограничения внешнего ключа таблицы `SquadStudent`
--
ALTER TABLE `SquadStudent`
  ADD CONSTRAINT `SquadStudent_ibfk_1` FOREIGN KEY (`id_squad`) REFERENCES `Squad` (`squad_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `SquadStudent_ibfk_2` FOREIGN KEY (`id_student`) REFERENCES `Student` (`student_id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Ограничения внешнего ключа таблицы `Teacher`
--
ALTER TABLE `Teacher`
  ADD CONSTRAINT `Teacher_ibfk_1` FOREIGN KEY (`teacher_id`) REFERENCES `User` (`user_id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Ограничения внешнего ключа таблицы `TechSupport`
--
ALTER TABLE `TechSupport`
  ADD CONSTRAINT `TechSupport_ibfk_1` FOREIGN KEY (`id_user`) REFERENCES `User` (`user_id`) ON DELETE NO ACTION ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
