DROP TABLE IF EXISTS `movie`;

CREATE TABLE `movie` (
  `ID` int(4) NOT NULL,
  `Genre` varchar(20) DEFAULT NULL,
  `Price` decimal(10,2) NOT NULL,
  `ReleaseDate` datetime NOT NULL DEFAULT current_timestamp(),
  `Title` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
