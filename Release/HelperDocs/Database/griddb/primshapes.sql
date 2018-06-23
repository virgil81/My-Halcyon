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
-- Table structure for table `primshapes`
--

DROP TABLE IF EXISTS `primshapes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `primshapes` (
  `Shape` int(11) DEFAULT NULL,
  `ScaleX` double NOT NULL DEFAULT 0,
  `ScaleY` double NOT NULL DEFAULT 0,
  `ScaleZ` double NOT NULL DEFAULT 0,
  `PCode` int(11) DEFAULT NULL,
  `PathBegin` int(11) DEFAULT NULL,
  `PathEnd` int(11) DEFAULT NULL,
  `PathScaleX` int(11) DEFAULT NULL,
  `PathScaleY` int(11) DEFAULT NULL,
  `PathShearX` int(11) DEFAULT NULL,
  `PathShearY` int(11) DEFAULT NULL,
  `PathSkew` int(11) DEFAULT NULL,
  `PathCurve` int(11) DEFAULT NULL,
  `PathRadiusOffset` int(11) DEFAULT NULL,
  `PathRevolutions` int(11) DEFAULT NULL,
  `PathTaperX` int(11) DEFAULT NULL,
  `PathTaperY` int(11) DEFAULT NULL,
  `PathTwist` int(11) DEFAULT NULL,
  `PathTwistBegin` int(11) DEFAULT NULL,
  `ProfileBegin` int(11) DEFAULT NULL,
  `ProfileEnd` int(11) DEFAULT NULL,
  `ProfileCurve` int(11) DEFAULT NULL,
  `ProfileHollow` int(11) DEFAULT NULL,
  `State` int(11) DEFAULT NULL,
  `Texture` longblob DEFAULT NULL,
  `ExtraParams` longblob DEFAULT NULL,
  `UUID` char(36) NOT NULL DEFAULT '',
  `Media` blob DEFAULT NULL,
  `Materials` longblob DEFAULT NULL,
  `PhysicsData` blob DEFAULT NULL,
  `PreferredPhysicsShape` tinyint(4) DEFAULT NULL,
  `VertexCount` int(11) DEFAULT NULL,
  `HighLODBytes` int(11) DEFAULT NULL,
  `MidLODBytes` int(11) DEFAULT NULL,
  `LowLODBytes` int(11) DEFAULT NULL,
  `LowestLODBytes` int(11) DEFAULT NULL,
  PRIMARY KEY (`UUID`),
  KEY `IDX_ATTACHMENT` (`PCode`,`State`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `primshapes`
--

LOCK TABLES `primshapes` WRITE;
/*!40000 ALTER TABLE `primshapes` DISABLE KEYS */;
/*!40000 ALTER TABLE `primshapes` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2018-05-26 11:39:09
