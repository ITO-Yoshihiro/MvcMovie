DROP TABLE IF EXISTS `trn_movie`;

CREATE TABLE `trn_movie` (
  `id_movie` int(4) NOT NULL,
  `kbn_genre` int(2) DEFAULT NULL,
  `kin_price` decimal(10,2) NOT NULL,
  `dt_release` datetime NOT NULL DEFAULT current_timestamp(),
  `nm_title` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`id_movie`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
