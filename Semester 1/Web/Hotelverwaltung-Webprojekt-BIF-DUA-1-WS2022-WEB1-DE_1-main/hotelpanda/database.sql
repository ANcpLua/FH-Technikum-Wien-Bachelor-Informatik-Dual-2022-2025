-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Erstellungszeit: 15. Jan 2023 um 23:16
-- Server-Version: 10.4.25-MariaDB
-- PHP-Version: 8.1.10

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Datenbank: `hotelpanda`
--

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `news`
--

CREATE TABLE `news` (
  `newsid` int(11) NOT NULL,
  `newsfile_path` varchar(255) NOT NULL,
  `newstitle` varchar(255) NOT NULL,
  `newsarticle` varchar(2000) NOT NULL,
  `newsdate` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Daten für Tabelle `news`
--

INSERT INTO `news` (`newsid`, `newsfile_path`, `newstitle`, `newsarticle`, `newsdate`) VALUES
(4, 'res/uploads/thumbnails/mads-eneqvist-zqiE16q_Ju0-unsplash(1).jpg', 'Exklusives Dinner Menu im Hotel Restaurant Umai!', 'Ein besonderes kulinarisches Erlebnis können ab 01.01.2023 in unserem hoteleigenen Restaurant Umai genießen. Wir bieten ein 8 Gänge Dinner Menü inklusive Weinbegleitung (alternativ alkoholfreie Cocktails Ihrer Wahl) nach Wahl des Head Chefs Mike Anderson für Einzelpersonen und Gruppen bis zu 8 Personen (120 € pro Person inkl. Gedeck und Aperitiv). Bitte kündigen Sie uns Ihren Besuch unter umai@hotelpanda.at an, da die Gerichte einige Vorbereitungszeit benötigen. Wir freuen uns auf Sie!', '2023-01-13 18:37:29'),
(108, 'res/uploads/thumbnails/yibei-geng--UdYbiywGeg-unsplash.jpg', 'Seminarraum verfügbar für Ihre Meetings', 'Wir freuen uns, unseren Business-Gästen neue Möglichkeiten zur Verfügung zu stellen, um den Aufenthalt so produktiv und erfolgreich wie möglich zu gestalten. Unser Seminarraum ist nun mit den neuesten Technologien wie einem lichtstarken Beamer, der neuesten Licht-, Klima- und Lüftungstechnik sowie mit einer schnellen und zuverlässigen WLAN-Verbindung ausgestattet. Natürlich stellen wir auch Flipcharts, Whiteboards, Stifte, Papier sowie Getränke und Snacks zur Verfügung. Mit Platz für bis zu 40 Personen eignet sich der Seminarraum damit ideal für Meetings, Präsentationen und Schulungen. Für unsere Gäste, die ihre Arbeit im Zimmer erledigen möchten, bieten wir jetzt Schreibtische in allen Zimmern und Suiten sowie kostenlosen High-Speed-WLAN-Zugang. Nutzen Sie außerdem unseren neuen Express-Check-in und Check-out-Service für unsere Geschäftsreisenden, um Zeit zu sparen und den Aufenthalt so reibungslos wie möglich zu gestalten. ', '2023-01-15 19:44:24');

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `reservations`
--

CREATE TABLE `reservations` (
  `usersId` int(11) NOT NULL,
  `start_date` date NOT NULL,
  `end_date` date NOT NULL,
  `room_type` varchar(255) NOT NULL,
  `breakfast` varchar(255) NOT NULL,
  `parking` varchar(255) NOT NULL,
  `pets` varchar(255) NOT NULL,
  `total_cost` decimal(10,2) NOT NULL,
  `status` varchar(255) NOT NULL,
  `reservation_date` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Daten für Tabelle `reservations`
--

INSERT INTO `reservations` (`usersId`, `start_date`, `end_date`, `room_type`, `breakfast`, `parking`, `pets`, `total_cost`, `status`, `reservation_date`) VALUES
(1, '2023-01-15', '2023-01-18', 'Deluxe', 'Ja', '1', '0', '528.00', 'neu', '2023-01-15 22:10:28'),
(1, '2023-01-19', '2023-01-21', 'Suite', '', '0', '2', '480.00', 'neu', '2023-01-15 22:10:49'),
(3, '2023-01-05', '2023-01-07', 'Standard', 'Ja', '0', '0', '232.00', 'neu', '2023-01-15 22:14:48');

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `users`
--

CREATE TABLE `users` (
  `usersId` int(11) NOT NULL,
  `usersAnrede` varchar(128) NOT NULL,
  `usersVorname` varchar(255) NOT NULL,
  `usersNachname` varchar(255) NOT NULL,
  `usersEmail` varchar(255) NOT NULL,
  `usersUid` varchar(255) NOT NULL,
  `usersPassword` varchar(255) NOT NULL,
  `usersTyp` varchar(12) NOT NULL DEFAULT 'anonyme',
  `usersStatus` varchar(12) NOT NULL DEFAULT 'active',
  `oneAktiv_zeroInaktiv` tinyint(4) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Daten für Tabelle `users`
--

INSERT INTO `users` (`usersId`, `usersAnrede`, `usersVorname`, `usersNachname`, `usersEmail`, `usersUid`, `usersPassword`, `usersTyp`, `usersStatus`, `oneAktiv_zeroInaktiv`) VALUES
(1, 'Herr', 'Micheal', 'Mayer', 'mmayer@gmail.com', 'guest1', '$2y$10$SZnTLgfwb5Uw4IJucXb7peQk7olP8667nioyBtXAICBx0JQq3UFGi', 'guest', 'active', 1),
(2, '', 'admin', 'admin', 'admin@hotelimperial.at', 'admin', '$2y$10$Wzv1TTYGv8bCOhBZu8kA4.tL2H9KgyTFGhQi3dPY03yEWVRB/CP7W', 'admin', 'active', 1),
(3, '', 'Michaela', 'Huber', 'mhuber@gmx.at', 'guest2', '$2y$10$x4d4bIaKUWWgAKZjEEsTRuIMiyMgqxBlpzYiKIthfhdxO2Ap1kJWC', 'guest', 'active', 1),
(4, '', 'andreas', 'bauer', 'andibauer@gmail.com', 'guest3', '$2y$10$I7yipyHPQ2FdrnX1zgrbSe.NC39UdxsRJCogaOPqf1fmjobKjptP.', 'guest', 'inactive', 1);

--
-- Indizes der exportierten Tabellen
--

--
-- Indizes für die Tabelle `news`
--
ALTER TABLE `news`
  ADD PRIMARY KEY (`newsid`);

--
-- Indizes für die Tabelle `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`usersId`);

--
-- AUTO_INCREMENT für exportierte Tabellen
--

--
-- AUTO_INCREMENT für Tabelle `news`
--
ALTER TABLE `news`
  MODIFY `newsid` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=109;

--
-- AUTO_INCREMENT für Tabelle `users`
--
ALTER TABLE `users`
  MODIFY `usersId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
