-- MySQL dump 10.13  Distrib 5.7.17, for Win64 (x86_64)
--
-- Host: localhost    Database: mygrid
-- ------------------------------------------------------
-- Server version	5.5.5-10.2.15-MariaDB

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `avatarappearance`
--

DROP TABLE IF EXISTS `avatarappearance`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `avatarappearance` (
  `Owner` char(36) NOT NULL,
  `Serial` int(10) unsigned NOT NULL,
  `Visual_Params` blob NOT NULL,
  `Texture` blob NOT NULL,
  `Avatar_Height` float NOT NULL,
  `Body_Item` char(36) NOT NULL,
  `Body_Asset` char(36) NOT NULL,
  `Skin_Item` char(36) NOT NULL,
  `Skin_Asset` char(36) NOT NULL,
  `Hair_Item` char(36) NOT NULL,
  `Hair_Asset` char(36) NOT NULL,
  `Eyes_Item` char(36) NOT NULL,
  `Eyes_Asset` char(36) NOT NULL,
  `Shirt_Item` char(36) NOT NULL,
  `Shirt_Asset` char(36) NOT NULL,
  `Pants_Item` char(36) NOT NULL,
  `Pants_Asset` char(36) NOT NULL,
  `Shoes_Item` char(36) NOT NULL,
  `Shoes_Asset` char(36) NOT NULL,
  `Socks_Item` char(36) NOT NULL,
  `Socks_Asset` char(36) NOT NULL,
  `Jacket_Item` char(36) NOT NULL,
  `Jacket_Asset` char(36) NOT NULL,
  `Gloves_Item` char(36) NOT NULL,
  `Gloves_Asset` char(36) NOT NULL,
  `Undershirt_Item` char(36) NOT NULL,
  `Undershirt_Asset` char(36) NOT NULL,
  `Underpants_Item` char(36) NOT NULL,
  `Underpants_Asset` char(36) NOT NULL,
  `Skirt_Item` char(36) NOT NULL,
  `Skirt_Asset` char(36) NOT NULL,
  `alpha_item` char(36) NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000',
  `alpha_asset` char(36) NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000',
  `tattoo_item` char(36) NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000',
  `tattoo_asset` char(36) NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000',
  `physics_item` char(36) NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000',
  `physics_asset` char(36) NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000',
  PRIMARY KEY (`Owner`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `avatarappearance`
--

LOCK TABLES `avatarappearance` WRITE;
/*!40000 ALTER TABLE `avatarappearance` DISABLE KEYS */;
/*!40000 ALTER TABLE `avatarappearance` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2018-05-26 11:39:00
