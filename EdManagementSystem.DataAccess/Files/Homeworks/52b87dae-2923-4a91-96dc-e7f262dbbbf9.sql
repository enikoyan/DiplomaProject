-- phpMyAdmin SQL Dump
-- version 5.0.4deb2+deb11u1
-- https://www.phpmyadmin.net/
--
-- Хост: localhost:3306
-- Время создания: Апр 11 2024 г., 19:18
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
-- База данных: `user004`
--

-- --------------------------------------------------------

--
-- Структура таблицы `Material`
--

CREATE TABLE `Material` (
  `id` int(11) NOT NULL,
  `material_id` char(36) NOT NULL,
  `title` varchar(255) NOT NULL,
  `date_added` datetime NOT NULL,
  `type` varchar(10) NOT NULL,
  `id_course` int(11) NOT NULL,
  `id_squad` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Дамп данных таблицы `Material`
--

INSERT INTO `Material` (`id`, `material_id`, `title`, `date_added`, `type`, `id_course`, `id_squad`) VALUES
(17, '738bf88b-dcf8-4236-9e67-7c9baf5d671e', '8 лекция (Нормализация)', '2024-04-10 09:50:05', '.pdf', 3, NULL),
(18, '6eb62a0b-783e-46ef-80eb-dfdb2d0f1926', 'Её лекция', '2024-04-10 09:50:41', '.docx', 3, 1),
(19, '6eb62a0b-783e-46ef-80eb-dfdb2d0f1926', 'Её лекция', '2024-04-10 09:50:41', '.docx', 3, 2),
(20, '65e48cb9-4d1c-4bad-9321-297182b6d570', 'Лекция 1 (06.09.22)', '2024-04-10 09:50:42', '.docx', 3, 1),
(21, '65e48cb9-4d1c-4bad-9321-297182b6d570', 'Лекция 1 (06.09.22)', '2024-04-10 09:50:42', '.docx', 3, 2);

--
-- Индексы сохранённых таблиц
--

--
-- Индексы таблицы `Material`
--
ALTER TABLE `Material`
  ADD PRIMARY KEY (`id`),
  ADD KEY `id_course` (`id_course`),
  ADD KEY `id_squad` (`id_squad`);

--
-- AUTO_INCREMENT для сохранённых таблиц
--

--
-- AUTO_INCREMENT для таблицы `Material`
--
ALTER TABLE `Material`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=22;

--
-- Ограничения внешнего ключа сохраненных таблиц
--

--
-- Ограничения внешнего ключа таблицы `Material`
--
ALTER TABLE `Material`
  ADD CONSTRAINT `Material_ibfk_1` FOREIGN KEY (`id_course`) REFERENCES `Course` (`course_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `Material_ibfk_2` FOREIGN KEY (`id_squad`) REFERENCES `Squad` (`squad_id`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
