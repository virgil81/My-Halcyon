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
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `users` (
  `UUID` varchar(36) NOT NULL DEFAULT '',
  `username` varchar(32) NOT NULL,
  `lastname` varchar(32) NOT NULL,
  `passwordHash` varchar(32) NOT NULL,
  `passwordSalt` varchar(32) NOT NULL,
  `homeRegion` bigint(20) unsigned DEFAULT NULL,
  `homeLocationX` float DEFAULT NULL,
  `homeLocationY` float DEFAULT NULL,
  `homeLocationZ` float DEFAULT NULL,
  `homeLookAtX` float DEFAULT NULL,
  `homeLookAtY` float DEFAULT NULL,
  `homeLookAtZ` float DEFAULT NULL,
  `created` int(11) NOT NULL,
  `lastLogin` int(11) NOT NULL,
  `userInventoryURI` varchar(255) DEFAULT NULL,
  `userAssetURI` varchar(255) DEFAULT NULL,
  `profileCanDoMask` int(10) unsigned DEFAULT NULL,
  `profileWantDoMask` int(10) unsigned DEFAULT NULL,
  `profileAboutText` text DEFAULT NULL,
  `profileFirstText` text DEFAULT NULL,
  `profileImage` varchar(36) DEFAULT NULL,
  `profileFirstImage` varchar(36) DEFAULT NULL,
  `webLoginKey` varchar(36) DEFAULT NULL,
  `homeRegionID` char(36) NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000',
  `userFlags` int(11) NOT NULL DEFAULT 0,
  `godLevel` int(11) NOT NULL DEFAULT 0,
  `iz_level` int(1) unsigned NOT NULL,
  `customType` varchar(32) NOT NULL DEFAULT '',
  `businessType` varchar(255) NOT NULL DEFAULT '',
  `partner` char(36) NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000',
  `email` varchar(250) DEFAULT NULL,
  `profileURL` varchar(100) DEFAULT NULL,
  `skillsMask` int(10) unsigned NOT NULL DEFAULT 0,
  `skillsText` varchar(255) NOT NULL DEFAULT 'None',
  `wantToMask` int(10) unsigned NOT NULL DEFAULT 0,
  `wantToText` varchar(255) NOT NULL DEFAULT 'None',
  `languagesText` varchar(255) NOT NULL DEFAULT 'English',
  PRIMARY KEY (`UUID`),
  UNIQUE KEY `usernames` (`username`,`lastname`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2018-05-26 11:39:04
