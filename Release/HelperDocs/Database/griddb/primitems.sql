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
-- Table structure for table `primitems`
--

DROP TABLE IF EXISTS `primitems`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `primitems` (
  `invType` int(11) DEFAULT NULL,
  `assetType` int(11) DEFAULT NULL,
  `name` varchar(255) DEFAULT NULL,
  `description` varchar(255) DEFAULT NULL,
  `creationDate` bigint(20) DEFAULT NULL,
  `nextPermissions` int(11) DEFAULT NULL,
  `currentPermissions` int(11) DEFAULT NULL,
  `basePermissions` int(11) DEFAULT NULL,
  `everyonePermissions` int(11) DEFAULT NULL,
  `groupPermissions` int(11) DEFAULT NULL,
  `flags` int(11) NOT NULL DEFAULT 0,
  `itemID` char(36) NOT NULL DEFAULT '',
  `primID` char(36) DEFAULT NULL,
  `assetID` char(36) DEFAULT NULL,
  `parentFolderID` char(36) DEFAULT NULL,
  `creatorID` char(36) DEFAULT NULL,
  `ownerID` char(36) DEFAULT NULL,
  `groupID` char(36) DEFAULT NULL,
  `lastOwnerID` char(36) DEFAULT NULL,
  `canDebitOwner` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`itemID`),
  KEY `primitems_primid` (`primID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `primitems`
--

LOCK TABLES `primitems` WRITE;
/*!40000 ALTER TABLE `primitems` DISABLE KEYS */;
/*!40000 ALTER TABLE `primitems` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2018-05-26 11:39:07
