-- MySQL dump 10.13  Distrib 5.7.17, for Win64 (x86_64)
--
-- Host: localhost    Database: myweb
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
-- Table structure for table `regionxml`
--

DROP TABLE IF EXISTS `regionxml`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `regionxml` (
  `UUID` varchar(36) NOT NULL,
  `regionName` varchar(32) NOT NULL,
  `status` int(1) NOT NULL DEFAULT 0,
  `locationX` int(10) NOT NULL,
  `locationY` int(10) NOT NULL,
  `internalIP` varchar(64) NOT NULL,
  `port` int(10) NOT NULL,
  `externalIP` varchar(64) NOT NULL,
  `ownerUUID` varchar(36) NOT NULL,
  `lastmapUUID` varchar(36) NOT NULL,
  `lastmapRefresh` varchar(64) NOT NULL,
  `primMax` int(10) NOT NULL,
  `physicalMax` int(10) NOT NULL,
  `productType` int(1) NOT NULL,
  `CreateDate` timestamp NOT NULL DEFAULT current_timestamp(),
  `MachineName` varchar(50) NOT NULL DEFAULT '',
  `ProcessHandle` int(10) NOT NULL DEFAULT 0,
  PRIMARY KEY (`UUID`),
  KEY `regionName` (`regionName`),
  KEY `locationX` (`locationX`),
  KEY `locationY` (`locationY`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `regionxml`
--

LOCK TABLES `regionxml` WRITE;
/*!40000 ALTER TABLE `regionxml` DISABLE KEYS */;
/*!40000 ALTER TABLE `regionxml` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2018-05-26 11:38:59
