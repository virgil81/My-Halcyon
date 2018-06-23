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
-- Table structure for table `offlines`
--

DROP TABLE IF EXISTS `offlines`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `offlines` (
  `fromAgentId` varchar(36) NOT NULL,
  `fromAgentName` varchar(36) NOT NULL,
  `toAgentId` varchar(36) NOT NULL,
  `dialogVal` tinyint(1) unsigned NOT NULL,
  `fromGroupVal` varchar(10) NOT NULL,
  `offlineMessage` text NOT NULL,
  `messageId` varchar(36) NOT NULL,
  `xPos` varchar(45) NOT NULL,
  `yPos` varchar(45) NOT NULL,
  `zPos` varchar(45) NOT NULL,
  `binaryBucket` varchar(45) NOT NULL,
  `parentEstateId` int(10) unsigned NOT NULL,
  `regionId` varchar(36) NOT NULL,
  `messageTimestamp` int(10) unsigned NOT NULL,
  `offlineVal` int(1) unsigned NOT NULL,
  KEY `IDX_toAgentId` (`toAgentId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `offlines`
--

LOCK TABLES `offlines` WRITE;
/*!40000 ALTER TABLE `offlines` DISABLE KEYS */;
/*!40000 ALTER TABLE `offlines` ENABLE KEYS */;
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
