DROP TABLE IF EXISTS `mst_genre`;

CREATE TABLE `mst_genre` (
  `id_genre` int(2) NOT NULL,
  `nm_genre` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`id_genre`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
