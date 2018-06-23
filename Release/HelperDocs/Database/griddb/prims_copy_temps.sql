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
-- Table structure for table `prims_copy_temps`
--

DROP TABLE IF EXISTS `prims_copy_temps`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `prims_copy_temps` (
  `CreationDate` int(11) DEFAULT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `Text` varchar(255) DEFAULT NULL,
  `Description` varchar(255) DEFAULT NULL,
  `SitName` varchar(255) DEFAULT NULL,
  `TouchName` varchar(255) DEFAULT NULL,
  `ObjectFlags` int(11) DEFAULT NULL,
  `OwnerMask` int(11) DEFAULT NULL,
  `NextOwnerMask` int(11) DEFAULT NULL,
  `GroupMask` int(11) DEFAULT NULL,
  `EveryoneMask` int(11) DEFAULT NULL,
  `BaseMask` int(11) DEFAULT NULL,
  `PositionX` double DEFAULT NULL,
  `PositionY` double DEFAULT NULL,
  `PositionZ` double DEFAULT NULL,
  `GroupPositionX` double DEFAULT NULL,
  `GroupPositionY` double DEFAULT NULL,
  `GroupPositionZ` double DEFAULT NULL,
  `VelocityX` double DEFAULT NULL,
  `VelocityY` double DEFAULT NULL,
  `VelocityZ` double DEFAULT NULL,
  `AngularVelocityX` double DEFAULT NULL,
  `AngularVelocityY` double DEFAULT NULL,
  `AngularVelocityZ` double DEFAULT NULL,
  `AccelerationX` double DEFAULT NULL,
  `AccelerationY` double DEFAULT NULL,
  `AccelerationZ` double DEFAULT NULL,
  `RotationX` double DEFAULT NULL,
  `RotationY` double DEFAULT NULL,
  `RotationZ` double DEFAULT NULL,
  `RotationW` double DEFAULT NULL,
  `SitTargetOffsetX` double DEFAULT NULL,
  `SitTargetOffsetY` double DEFAULT NULL,
  `SitTargetOffsetZ` double DEFAULT NULL,
  `SitTargetOrientW` double DEFAULT NULL,
  `SitTargetOrientX` double DEFAULT NULL,
  `SitTargetOrientY` double DEFAULT NULL,
  `SitTargetOrientZ` double DEFAULT NULL,
  `OldUUID` char(36) NOT NULL DEFAULT '',
  `NewUUID` char(36) NOT NULL DEFAULT '',
  `RegionUUID` char(36) DEFAULT NULL,
  `CreatorID` char(36) DEFAULT NULL,
  `OwnerID` char(36) DEFAULT NULL,
  `GroupID` char(36) DEFAULT NULL,
  `LastOwnerID` char(36) DEFAULT NULL,
  `SceneGroupID` char(36) DEFAULT NULL,
  `PayPrice` int(11) NOT NULL DEFAULT 0,
  `PayButton1` int(11) NOT NULL DEFAULT 0,
  `PayButton2` int(11) NOT NULL DEFAULT 0,
  `PayButton3` int(11) NOT NULL DEFAULT 0,
  `PayButton4` int(11) NOT NULL DEFAULT 0,
  `LoopedSound` char(36) NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000',
  `LoopedSoundGain` double NOT NULL DEFAULT 0,
  `TextureAnimation` blob DEFAULT NULL,
  `OmegaX` double NOT NULL DEFAULT 0,
  `OmegaY` double NOT NULL DEFAULT 0,
  `OmegaZ` double NOT NULL DEFAULT 0,
  `CameraEyeOffsetX` double NOT NULL DEFAULT 0,
  `CameraEyeOffsetY` double NOT NULL DEFAULT 0,
  `CameraEyeOffsetZ` double NOT NULL DEFAULT 0,
  `CameraAtOffsetX` double NOT NULL DEFAULT 0,
  `CameraAtOffsetY` double NOT NULL DEFAULT 0,
  `CameraAtOffsetZ` double NOT NULL DEFAULT 0,
  `ForceMouselook` tinyint(4) NOT NULL DEFAULT 0,
  `ScriptAccessPin` int(11) NOT NULL DEFAULT 0,
  `AllowedDrop` tinyint(4) NOT NULL DEFAULT 0,
  `DieAtEdge` tinyint(4) NOT NULL DEFAULT 0,
  `SalePrice` int(11) NOT NULL DEFAULT 10,
  `SaleType` tinyint(4) NOT NULL DEFAULT 0,
  `ColorR` int(11) NOT NULL DEFAULT 0,
  `ColorG` int(11) NOT NULL DEFAULT 0,
  `ColorB` int(11) NOT NULL DEFAULT 0,
  `ColorA` int(11) NOT NULL DEFAULT 0,
  `ParticleSystem` blob DEFAULT NULL,
  `ClickAction` tinyint(4) NOT NULL DEFAULT 0,
  `Material` tinyint(4) NOT NULL DEFAULT 3,
  `CollisionSound` char(36) NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000',
  `CollisionSoundVolume` double NOT NULL DEFAULT 0,
  `LinkNumber` int(11) NOT NULL DEFAULT 0,
  PRIMARY KEY (`OldUUID`),
  KEY `prims_regionuuid` (`RegionUUID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `prims_copy_temps`
--

LOCK TABLES `prims_copy_temps` WRITE;
/*!40000 ALTER TABLE `prims_copy_temps` DISABLE KEYS */;
/*!40000 ALTER TABLE `prims_copy_temps` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2018-05-26 11:39:01
