-- MySQL dump 10.15  Distrib 10.0.14-MariaDB, for Win64 (x86)
--
-- Host: localhost    Database: hw
-- ------------------------------------------------------
-- Server version	10.0.14-MariaDB

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
-- Table structure for table `build`
--

DROP TABLE IF EXISTS `build`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `build` (
  `ID` char(36) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Description` text,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `build`
--

LOCK TABLES `build` WRITE;
/*!40000 ALTER TABLE `build` DISABLE KEYS */;
INSERT INTO `build` VALUES ('0720d277-4363-4704-8f22-c18d661b6eb9','炮塔','通过防御塔升级而来，攻击力和防御力更强'),('1ef38086-5c86-4f93-8f29-027c362b24bb','城堡','巨型的城堡是奢华的象征'),('31cd4869-1529-4bb2-874b-46c1639a30ab','双层平房','不错的房子'),('34a169c8-f870-45ff-b976-5669be3e2612','瞭望塔','简单的瞭望塔，没有攻击力'),('61448f87-2fc2-11e5-a581-60eb6958ce43','冶炼厂','可以把矿石变成金属的场所'),('8b243957-2fc0-11e5-a581-60eb6958ce43','荒地','烂地一块'),('92570f08-7f48-4e34-a951-f06e631e24cc','防御塔','通过瞭望塔升级而成，具有基本的攻击力'),('9567c2c6-2fbc-11e5-a581-60eb6958ce43','茅屋','仅能遮风挡雨的小屋子'),('ac63a9ae-2fbc-11e5-a581-60eb6958ce43','木屋','高级一点的房子，可以抵挡台风来袭'),('b5248c0a-67af-42d3-a598-0918c09e8525','庄园','超大型建筑，极致奢华'),('c893e866-1c2b-4df1-9d31-596b283ba898','木头厂','加工木头');
/*!40000 ALTER TABLE `build` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `construction`
--

DROP TABLE IF EXISTS `construction`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `construction` (
  `ID` char(36) NOT NULL,
  `OriginBuilding` char(36) NOT NULL,
  `Building` char(36) NOT NULL,
  `Items` text,
  `Time` int(11) NOT NULL,
  `Description` text,
  `Workers` int(255) DEFAULT NULL,
  `Name` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `construction`
--

LOCK TABLES `construction` WRITE;
/*!40000 ALTER TABLE `construction` DISABLE KEYS */;
INSERT INTO `construction` VALUES ('44a4d878-8cdf-4b87-b5d2-717ff6523e82','34a169c8-f870-45ff-b976-5669be3e2612','92570f08-7f48-4e34-a951-f06e631e24cc','[{\"6b87bd04-2fc0-11e5-a581-60eb6958ce43\":1000},{\"4564d5dd-2fc0-11e5-a581-60eb6958ce43\":5000}]',500,'升级到防御塔，再也不是单纯的挨打了',2,'升级瞭望塔'),('46da9bd4-2fc3-11e5-a581-60eb6958ce43','8b243957-2fc0-11e5-a581-60eb6958ce43','9567c2c6-2fbc-11e5-a581-60eb6958ce43','{\"357255d6-2fc0-11e5-a581-60eb6958ce43\":2}',600,'饥荒老玩家十分钟就把基地建好了',1,'建造茅屋'),('96276873-7957-461b-8b36-89c1b8ebdae8','0720d277-4363-4704-8f22-c18d661b6eb9','92570f08-7f48-4e34-a951-f06e631e24cc','[{\"24d7c1ea-bbaa-4aab-805d-26b443b6dc37\":1},{\"6b87bd04-2fc0-11e5-a581-60eb6958ce43\":3000},{\"4564d5dd-2fc0-11e5-a581-60eb6958ce43\":1000}]',500,'升级为炮塔，拥有较强的威力',4,'升级防御塔'),('cd67156f-2fc4-11e5-a581-60eb6958ce43','8b243957-2fc0-11e5-a581-60eb6958ce43','61448f87-2fc2-11e5-a581-60eb6958ce43','{\"4564d5dd-2fc0-11e5-a581-60eb6958ce43\":100}',3600,'建造冶炼厂',2,'建筑冶炼厂');
/*!40000 ALTER TABLE `construction` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `item`
--

DROP TABLE IF EXISTS `item`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `item` (
  `ID` char(36) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Description` text,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `item`
--

LOCK TABLES `item` WRITE;
/*!40000 ALTER TABLE `item` DISABLE KEYS */;
INSERT INTO `item` VALUES ('24d7c1ea-bbaa-4aab-805d-26b443b6dc37','大炮','炮弹来袭。。。。'),('2ec32330-c0e2-4f13-836d-448fb091be27','陨石','天上掉下来的石头，里面可能有一些好东西'),('357255d6-2fc0-11e5-a581-60eb6958ce43','干草','到处可见的干草'),('375797d9-1c79-497f-b508-b857216ab08e','金矿','加工可以成为各种金制品'),('37927066-ac6c-45d6-a86e-20cc96794b8c','精致的木头','高级木头'),('4564d5dd-2fc0-11e5-a581-60eb6958ce43','木头','一捆一捆的木头，很有用'),('57f9a25f-2fc0-11e5-a581-60eb6958ce43','矿石','矿石，加工可以得到金属'),('6b87bd04-2fc0-11e5-a581-60eb6958ce43','金属','由矿石加工得到，可以用于锻造'),('6e4a0334-ac49-467a-a3c4-f4aaaeb46929','火药','四大发明，黑火药'),('8483d41a-acaa-4ec9-a291-7940d211f746','枪','热武器的一种，对人威力不错'),('aca115f3-f72d-4271-a2e1-e7d6f2437b2d','钻石矿石','含有钻石的矿石，可提炼'),('c5c6976d-cb20-4ad4-85cc-862ea5c798d4','钻石','昂贵的奢侈品'),('cbda6da6-1f82-423c-97d8-497d297c3090','普通石头','普通的石头，随处可见'),('e5669821-ac5a-4897-bc62-07c6c275b4e2','高纯度矿石','纯度更高，能够提炼更多的金属'),('f6dd21a5-e7ed-476a-8a12-19385524e9e8','金币','货币');
/*!40000 ALTER TABLE `item` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `product`
--

DROP TABLE IF EXISTS `product`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `product` (
  `ID` char(36) NOT NULL,
  `Resource` text,
  `Products` text,
  `Name` varchar(255) NOT NULL,
  `Description` text,
  `Building` char(36) DEFAULT NULL,
  `Time` int(11) NOT NULL,
  `Workers` int(255) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `product`
--

LOCK TABLES `product` WRITE;
/*!40000 ALTER TABLE `product` DISABLE KEYS */;
INSERT INTO `product` VALUES ('c407236a-fb8c-4086-9771-dc2d007cc5f3','[{\"375797d9-1c79-497f-b508-b857216ab08e\":10}]','[{\"f6dd21a5-e7ed-476a-8a12-19385524e9e8\":1}]','铸币','铸金币','61448f87-2fc2-11e5-a581-60eb6958ce43',100,10);
/*!40000 ALTER TABLE `product` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2015-07-25 23:49:32
