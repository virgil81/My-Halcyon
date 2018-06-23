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
-- Table structure for table `regions`
--

DROP TABLE IF EXISTS `regions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `regions` (
  `uuid` varchar(36) NOT NULL,
  `regionHandle` bigint(20) unsigned NOT NULL,
  `regionName` varchar(32) DEFAULT NULL,
  `regionRecvKey` varchar(128) DEFAULT NULL,
  `regionSendKey` varchar(128) DEFAULT NULL,
  `regionSecret` varchar(128) DEFAULT NULL,
  `regionDataURI` varchar(255) DEFAULT NULL,
  `serverIP` varchar(64) DEFAULT NULL,
  `serverPort` int(10) unsigned DEFAULT NULL,
  `serverURI` varchar(255) DEFAULT NULL,
  `locX` int(10) unsigned DEFAULT NULL,
  `locY` int(10) unsigned DEFAULT NULL,
  `locZ` int(10) unsigned DEFAULT NULL,
  `eastOverrideHandle` bigint(20) unsigned DEFAULT NULL,
  `westOverrideHandle` bigint(20) unsigned DEFAULT NULL,
  `southOverrideHandle` bigint(20) unsigned DEFAULT NULL,
  `northOverrideHandle` bigint(20) unsigned DEFAULT NULL,
  `regionAssetURI` varchar(255) DEFAULT NULL,
  `regionAssetRecvKey` varchar(128) DEFAULT NULL,
  `regionAssetSendKey` varchar(128) DEFAULT NULL,
  `regionUserURI` varchar(255) DEFAULT NULL,
  `regionUserRecvKey` varchar(128) DEFAULT NULL,
  `regionUserSendKey` varchar(128) DEFAULT NULL,
  `regionMapTexture` varchar(36) DEFAULT NULL,
  `serverHttpPort` int(10) DEFAULT NULL,
  `serverRemotingPort` int(10) DEFAULT NULL,
  `owner_uuid` varchar(36) NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000',
  `originUUID` varchar(36) DEFAULT NULL,
  `access` int(10) unsigned DEFAULT 1,
  `ScopeID` char(36) NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000',
  `sizeX` int(11) unsigned NOT NULL DEFAULT 0,
  `sizeY` int(11) unsigned NOT NULL DEFAULT 0,
  `product` tinyint(4) NOT NULL DEFAULT 0,
  `outside_ip` varchar(64) DEFAULT NULL,
  PRIMARY KEY (`uuid`),
  KEY `regionName` (`regionName`),
  KEY `regionHandle` (`regionHandle`),
  KEY `overrideHandles` (`eastOverrideHandle`,`westOverrideHandle`,`southOverrideHandle`,`northOverrideHandle`),
  KEY `IDX_XY` (`locX`,`locY`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Rev. 3';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `regions`
--

LOCK TABLES `regions` WRITE;
/*!40000 ALTER TABLE `regions` DISABLE KEYS */;
/*!40000 ALTER TABLE `regions` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2018-05-26 11:39:02
