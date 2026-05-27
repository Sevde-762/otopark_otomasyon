CREATE DATABASE  IF NOT EXISTS `garanti_db_otopark_otomasyon` /*!40100 DEFAULT CHARACTER SET utf8mb3 */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `garanti_db_otopark_otomasyon`;
-- MySQL dump 10.13  Distrib 8.0.11, for Win64 (x86_64)
--
-- Host: localhost    Database: mydb
-- ------------------------------------------------------
-- Server version	8.0.46

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
 SET NAMES utf8 ;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `abonelikler`
--

DROP TABLE IF EXISTS `abonelikler`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `abonelikler` (
  `abonelik_id` int NOT NULL AUTO_INCREMENT,
  `baslangic_tarihi` date NOT NULL,
  `bitis_tarihi` date NOT NULL,
  `abonelik_turu` varchar(30) NOT NULL,
  `araclar_id` int NOT NULL,
  PRIMARY KEY (`abonelik_id`),
  KEY `fk_Abonelikler_Araclar1_idx` (`araclar_id`),
  CONSTRAINT `fk_Abonelikler_Araclar1` FOREIGN KEY (`araclar_id`) REFERENCES `araclar` (`araclar_id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `abonelikler`
--

LOCK TABLES `abonelikler` WRITE;
/*!40000 ALTER TABLE `abonelikler` DISABLE KEYS */;
INSERT INTO `abonelikler` VALUES (1,'2026-01-10','2026-02-10','AYLIK',1),(2,'2026-05-12','2026-11-12','6 AYLIK',2),(3,'2026-03-03','2026-06-03','3 AYLIK',3),(4,'2026-05-24','2026-06-24','AYLIK ',4),(5,'2026-02-28','2026-08-28','6 AYLIK',5),(6,'2026-04-17','2026-07-17','3 AYLIK',6),(7,'2026-02-09','2026-03-09','AYLIK',7),(8,'2026-05-23','2026-06-23','AYLIK',8),(9,'2026-01-18','2026-07-18','6 AYLIK',9),(10,'2026-06-01','2026-09-01','3 AYLIK',10);
/*!40000 ALTER TABLE `abonelikler` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `araclar`
--

DROP TABLE IF EXISTS `araclar`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `araclar` (
  `araclar_id` int NOT NULL AUTO_INCREMENT,
  `plaka` varchar(20) NOT NULL,
  `arac_turu` varchar(30) DEFAULT NULL,
  `musteri_id` int NOT NULL,
  PRIMARY KEY (`araclar_id`),
  KEY `fk_Araclar_musteriler1_idx` (`musteri_id`),
  CONSTRAINT `fk_Araclar_musteriler1` FOREIGN KEY (`musteri_id`) REFERENCES `musteriler` (`musteri_id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `araclar`
--

LOCK TABLES `araclar` WRITE;
/*!40000 ALTER TABLE `araclar` DISABLE KEYS */;
INSERT INTO `araclar` VALUES (1,'41 AGA 123','SUV',1),(2,'34 BJK 1903','SUV',2),(3,'06 ASD 312','Hatchback',3),(4,'35 BNK 6985','Minibüs',4),(5,'42 SML 762','Sedan',5),(6,'28 SEV 3438','Roadster',6),(7,'16 MST 1503','Hatchback',7),(8,' 34 ZYN 2013','Sedan',8),(9,'41 KCL 4141','SUV',9),(10,'34 GGK 2004','Roadster',10);
/*!40000 ALTER TABLE `araclar` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `giris_cikislar`
--

DROP TABLE IF EXISTS `giris_cikislar`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `giris_cikislar` (
  `islem_id` int NOT NULL AUTO_INCREMENT,
  `giris_zamani` datetime NOT NULL,
  `cikis_zamani` datetime DEFAULT NULL,
  `durum` varchar(20) NOT NULL,
  `araclar_id` int NOT NULL,
  `alan_id` int NOT NULL,
  PRIMARY KEY (`islem_id`),
  KEY `fk_Giris_Cikislar_Araclar1_idx` (`araclar_id`),
  KEY `fk_Giris_Cikislar_ParkAlanlari1_idx` (`alan_id`),
  CONSTRAINT `fk_Giris_Cikislar_Araclar1` FOREIGN KEY (`araclar_id`) REFERENCES `araclar` (`araclar_id`),
  CONSTRAINT `fk_Giris_Cikislar_ParkAlanlari1` FOREIGN KEY (`alan_id`) REFERENCES `parkalanlari` (`alan_id`)
) ENGINE=InnoDB AUTO_INCREMENT=41 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `giris_cikislar`
--

LOCK TABLES `giris_cikislar` WRITE;
/*!40000 ALTER TABLE `giris_cikislar` DISABLE KEYS */;
INSERT INTO `giris_cikislar` VALUES (11,'2026-05-22 08:00:00','2026-05-22 10:30:00','Tamamlandi',1,1),(12,'2026-05-22 09:15:00','2026-05-22 13:00:00','Tamamlandi',2,2),(13,'2026-05-22 10:00:00','2026-05-22 11:15:00','Tamamlandi',3,3),(14,'2026-05-22 11:30:00','2026-05-22 17:45:00','Tamamlandi',4,4),(15,'2026-05-22 12:00:00','2026-05-22 14:00:00','Tamamlandi',5,5),(16,'2026-05-22 13:45:00','2026-05-22 15:15:00','Tamamlandi',6,6),(17,'2026-05-22 15:00:00','2026-05-22 19:30:00','Tamamlandi',7,7),(18,'2026-05-22 16:20:00','2026-05-22 18:00:00','Tamamlandi',8,8),(19,'2026-05-22 17:00:00','2026-05-22 21:00:00','Tamamlandi',9,9),(20,'2026-05-22 18:30:00','2026-05-22 20:00:00','Tamamlandi',10,10);
/*!40000 ALTER TABLE `giris_cikislar` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `musteriler`
--

DROP TABLE IF EXISTS `musteriler`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `musteriler` (
  `musteri_id` int NOT NULL AUTO_INCREMENT,
  `musteri_adi` varchar(45) NOT NULL,
  `musteri_soyadi` varchar(45) NOT NULL,
  `telefon` varchar(15) NOT NULL,
  `eposta` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`musteri_id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `musteriler`
--

LOCK TABLES `musteriler` WRITE;
/*!40000 ALTER TABLE `musteriler` DISABLE KEYS */;
INSERT INTO `musteriler` VALUES (1,'Ahmet','Yılmaz','05321112233','ahmetyilmaz33@gmail.com'),(2,'Elif ','Kaya','05422223344','kayaeliff@gmail.com'),(3,'Mehmed','Demir','05053334455','mehmedemir@gmail.com'),(4,'Ayşe ','Çelik','05554445566','cellayse@gmail.com'),(5,'Can','Öztürk','05435448890','ozzturk34@gmail.com'),(6,'Merve ','Kartal','05065781040','mervekkartal@gmail.com'),(7,'Mustafa','Aydın','05067778899','aydınustafa@gmail.com'),(8,'Zeynep','Arslan','05337497829','05781238899'),(9,'Burak','Polat','05421910706','polat2burak@gmail.com'),(10,'Fatma','Yıldız','05789631144','yıldızfatma@gmail.com');
/*!40000 ALTER TABLE `musteriler` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `odemeler`
--

DROP TABLE IF EXISTS `odemeler`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `odemeler` (
  `odeme_id` int NOT NULL AUTO_INCREMENT,
  `tutar` decimal(10,2) NOT NULL,
  `odeme_tarihi` datetime NOT NULL,
  `islem_id` int NOT NULL,
  PRIMARY KEY (`odeme_id`),
  KEY `fk_Odemeler_Giris_Cikislar_idx` (`islem_id`),
  CONSTRAINT `fk_Odemeler_Giris_Cikislar` FOREIGN KEY (`islem_id`) REFERENCES `giris_cikislar` (`islem_id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `odemeler`
--

LOCK TABLES `odemeler` WRITE;
/*!40000 ALTER TABLE `odemeler` DISABLE KEYS */;
INSERT INTO `odemeler` VALUES (1,50.00,'2026-05-22 10:30:00',11),(2,75.00,'2026-05-22 13:00:00',12),(3,30.00,'2026-05-22 11:15:00',13),(4,120.00,'2026-05-22 17:45:00',14),(5,40.00,'2026-05-22 14:00:00',15),(6,35.00,'2026-05-22 15:15:00',16),(7,90.00,'2026-05-22 19:30:00',17),(8,40.00,'2026-05-22 18:00:00',18),(9,80.00,'2026-05-22 21:00:00',19),(10,35.00,'2026-05-22 20:00:00',20);
/*!40000 ALTER TABLE `odemeler` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `parkalanlari`
--

DROP TABLE IF EXISTS `parkalanlari`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `parkalanlari` (
  `alan_id` int NOT NULL AUTO_INCREMENT,
  `alan_adi` varchar(50) NOT NULL,
  `kapasite` int NOT NULL,
  `bos_kapasite` int NOT NULL,
  PRIMARY KEY (`alan_id`),
  UNIQUE KEY `alan_adi_UNIQUE` (`alan_adi`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `parkalanlari`
--

LOCK TABLES `parkalanlari` WRITE;
/*!40000 ALTER TABLE `parkalanlari` DISABLE KEYS */;
INSERT INTO `parkalanlari` VALUES (1,'A-1',7,7),(2,'A-2',7,7),(3,'A-3',7,7),(4,'B-1',10,10),(5,'B-2',10,10),(6,'B-3',10,10),(7,'C-1',5,5),(8,'C-2',5,5),(9,'C-3',5,5),(10,'D-1',3,3);
/*!40000 ALTER TABLE `parkalanlari` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-05-24 12:58:23
-- ========================================================
-- SAMET HOCANIN İSTEDİĞİ EK YAPILAR (INDEX, VIEW, TRIGGER, PROCEDURE)
-- ========================================================

-- 1. İNDEKS YAPILARI
CREATE INDEX idx_arac_plaka ON araclar(plaka);
CREATE INDEX idx_musteri_telefon ON musteriler(telefon);

-- 2. VIEW YAPILARI
CREATE VIEW view_otopark_durumu AS
SELECT alan_adi AS 'Park Alani', kapasite AS 'Toplam Kapasite', bos_kapasite AS 'Bos Yer'
FROM parkalanlari;

CREATE VIEW view_musteri_arac_listesi AS
SELECT m.musteri_adi AS 'Ad', m.musteri_soyadi AS 'Soyad', a.plaka AS 'Plaka', a.arac_turu AS 'Arac Turu'
FROM musteriler m
INNER JOIN araclar a ON m.musteri_id = a.musteri_id;

-- 3. TRIGGER YAPILARI
DELIMITER //
CREATE TRIGGER trg_arac_giris_sonrasi_kapasite_azalt
AFTER INSERT ON giris_cikislar
FOR EACH ROW
BEGIN
    UPDATE parkalanlari 
    SET bos_kapasite = bos_kapasite - 1 
    WHERE alan_id = NEW.alan_id;
END//
DELIMITER;

DELIMITER //
CREATE TRIGGER trg_arac_cikis_sonrasi_kapasite_artir
AFTER UPDATE ON giris_cikislar
FOR EACH ROW
BEGIN
    IF NEW.durum = 'Tamamlandi' AND OLD.durum != 'Tamamlandi' THEN
        UPDATE parkalanlari 
        SET bos_kapasite = bos_kapasite + 1 
        WHERE alan_id = NEW.alan_id;
    END IF;
END//
DELIMITER;

-- 4. STORED PROCEDURE YAPILARI
DELIMITER //
CREATE PROCEDURE sp_musteri_ekle(
    IN p_adi VARCHAR(45),
    IN p_soyadi VARCHAR(45),
    IN p_telefon VARCHAR(15),
    IN p_eposta VARCHAR(100)
)
BEGIN
    INSERT INTO musteriler(musteri_adi, musteri_soyadi, telefon, eposta) 
    VALUES (p_adi, p_soyadi, p_telefon, p_eposta);
END//
DELIMITER;

DELIMITER //
CREATE PROCEDURE sp_odeme_al(
    IN p_tutar DECIMAL(10,2),
    IN p_islem_id INT
)
BEGIN
    INSERT INTO odemeler(tutar, odeme_tarihi, islem_id) 
    VALUES (p_tutar, NOW(), p_islem_id);
END//
DELIMITER;
