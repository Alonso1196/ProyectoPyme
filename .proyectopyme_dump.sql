-- MySQL dump 10.13  Distrib 8.0.45, for Win64 (x86_64)
--
-- Host: localhost    Database: proyectopyme
-- ------------------------------------------------------
-- Server version	8.0.45

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `__efmigrationshistory`
--

DROP TABLE IF EXISTS `__efmigrationshistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__efmigrationshistory`
--

LOCK TABLES `__efmigrationshistory` WRITE;
/*!40000 ALTER TABLE `__efmigrationshistory` DISABLE KEYS */;
INSERT INTO `__efmigrationshistory` VALUES ('20260202195132_InitialCreate','10.0.3'),('20260202201414_UpdateProductoPerfume','10.0.3'),('20260202201902_AddProductosTable','10.0.3'),('20260202203826_AddCategoriaRelacion','10.0.3'),('20260202205913_AddCategoriaToProducto','10.0.3'),('20260202215927_Inicial','10.0.3'),('20260202220621_SeedCategorias','10.0.3'),('20260203000940_AddRutaImagenToProductos','10.0.3'),('20260205174529_AddEsencias','10.0.3'),('20260309202633_SeedRoles','10.0.2');
/*!40000 ALTER TABLE `__efmigrationshistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `carrito`
--

DROP TABLE IF EXISTS `carrito`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `carrito` (
  `IdCarrito` int NOT NULL AUTO_INCREMENT,
  `UsuarioId` int NOT NULL,
  `IdProducto` int NOT NULL,
  `Cantidad` int NOT NULL,
  PRIMARY KEY (`IdCarrito`),
  KEY `IX_Carrito_IdProducto` (`IdProducto`),
  CONSTRAINT `FK_Carrito_Productos_IdProducto` FOREIGN KEY (`IdProducto`) REFERENCES `productos` (`ProductoId`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `carrito`
--

LOCK TABLES `carrito` WRITE;
/*!40000 ALTER TABLE `carrito` DISABLE KEYS */;
INSERT INTO `carrito` VALUES (6,1,8,1),(7,1,7,1),(8,6,7,1),(9,6,8,1),(10,6,6,1);
/*!40000 ALTER TABLE `carrito` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `categorias`
--

DROP TABLE IF EXISTS `categorias`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `categorias` (
  `CategoriaId` int NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Descripcion` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`CategoriaId`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `categorias`
--

LOCK TABLES `categorias` WRITE;
/*!40000 ALTER TABLE `categorias` DISABLE KEYS */;
INSERT INTO `categorias` VALUES (1,'Dise├▒ador','Perfume de dise├▒ador'),(2,'Dupe','Perfume r├⌐plica'),(3,'├ürabe','Perfume ├írabe');
/*!40000 ALTER TABLE `categorias` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `esencias`
--

DROP TABLE IF EXISTS `esencias`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `esencias` (
  `EsenciaId` int NOT NULL AUTO_INCREMENT,
  `Nombre` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Activo` tinyint(1) NOT NULL,
  PRIMARY KEY (`EsenciaId`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `esencias`
--

LOCK TABLES `esencias` WRITE;
/*!40000 ALTER TABLE `esencias` DISABLE KEYS */;
INSERT INTO `esencias` VALUES (1,'Eau de Toilette',1),(2,'Eau de Parfum',1),(3,'Parfum',1),(4,'Elixir',1);
/*!40000 ALTER TABLE `esencias` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ordenes`
--

DROP TABLE IF EXISTS `ordenes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ordenes` (
  `IdOrden` int NOT NULL AUTO_INCREMENT,
  `NumeroOrden` varchar(255) DEFAULT NULL,
  `UsuarioId` int NOT NULL,
  `Fecha` datetime NOT NULL,
  `NombreCliente` varchar(255) DEFAULT NULL,
  `Direccion` varchar(255) DEFAULT NULL,
  `Telefono` varchar(255) DEFAULT NULL,
  `MetodoPago` varchar(255) DEFAULT NULL,
  `Total` decimal(18,2) NOT NULL,
  `Estado` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`IdOrden`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ordenes`
--

LOCK TABLES `ordenes` WRITE;
/*!40000 ALTER TABLE `ordenes` DISABLE KEYS */;
INSERT INTO `ordenes` VALUES (1,'ORD-639091925219510477',1,'2026-03-15 17:28:42','asd','asd','555','Tarjeta',0.00,'Pendiente'),(2,'ORD-639091951741142129',1,'2026-03-15 18:12:54','77','00000&&&&&','%%%%%%%%%%%ostia','Tarjeta',0.00,'Pendiente'),(3,'ORD-639091964001107149',1,'2026-03-15 18:33:20','asd','ads','asd','Stripe',453.00,'Pagado'),(4,'ORD-639091999007978996',1,'2026-03-15 19:31:41','Create','paspdads','999','Stripe',176.00,'Pagado'),(5,'ORD-639093020222849750',1,'2026-03-16 23:53:42','dasd','asd','8888','Stripe',88.00,'Pendiente');
/*!40000 ALTER TABLE `ordenes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `productos`
--

DROP TABLE IF EXISTS `productos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `productos` (
  `ProductoId` int NOT NULL AUTO_INCREMENT,
  `Nombre` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Precio` decimal(65,30) NOT NULL,
  `Disponibilidad` tinyint(1) NOT NULL,
  `CategoriaId` int NOT NULL DEFAULT '0',
  `RutaImagen` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `EsenciaId` int NOT NULL DEFAULT '1',
  `Stock` int NOT NULL DEFAULT '0',
  PRIMARY KEY (`ProductoId`),
  KEY `IX_Productos_CategoriaId` (`CategoriaId`),
  KEY `IX_Productos_EsenciaId` (`EsenciaId`),
  CONSTRAINT `FK_Productos_categorias_CategoriaId` FOREIGN KEY (`CategoriaId`) REFERENCES `categorias` (`CategoriaId`) ON DELETE CASCADE,
  CONSTRAINT `FK_Productos_esencias_EsenciaId` FOREIGN KEY (`EsenciaId`) REFERENCES `esencias` (`EsenciaId`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `productos`
--

LOCK TABLES `productos` WRITE;
/*!40000 ALTER TABLE `productos` DISABLE KEYS */;
INSERT INTO `productos` VALUES (1,'Jean Paul Gaultier Le Male Le Parfum',95.000000000000000000000000000000,1,1,'/images/Productos/Jean Paul Gaultier Le Male Le Parfum.jfif',3,0),(2,'Invictus Legend - Paco Rabanne',85.000000000000000000000000000000,1,1,'/images/Productos/Invictus legend Paco Rabanne.jfif',1,0),(3,'Carolina Herrera 212 Sexy',90.000000000000000000000000000000,1,1,'/images/Productos/Carolina Herrera 212 Sexy.jfif',1,0),(4,'Valentino Born In Roma Intense',100.000000000000000000000000000000,1,1,'/images/Productos/Valentino Born In Roma Intense Eau De Parfum.jfif',2,0),(5,'Dior Sauvage',84.000000000000000000000000000000,1,1,'/images/Productos/Sauvage Dior .jfif',1,0),(6,'Chanel Bleu',105.000000000000000000000000000000,1,1,'/images/Productos/Bleu de Chanel.jfif',2,0),(7,'Versace Eros',88.000000000000000000000000000000,1,1,'/images/Productos/Versace Eros.jfif',1,0),(8,'1 Million',84.000000000000000000000000000000,1,1,'/images/Productos/1million.jfif',1,0),(9,'Karlos Circ',25.000000000000000000000000000000,1,2,'/uploads/Productos/c676b4ed-61b6-4003-9a22-c23031a60b1b.jpg',2,60),(10,'Nil',99.000000000000000000000000000000,1,2,'/uploads/Productos/1661e2b0-10b6-42ce-b28a-8bcac84bb0f3.jpg',4,99);
/*!40000 ALTER TABLE `productos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `roles`
--

DROP TABLE IF EXISTS `roles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `roles` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Nombre` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `roles`
--

LOCK TABLES `roles` WRITE;
/*!40000 ALTER TABLE `roles` DISABLE KEYS */;
INSERT INTO `roles` VALUES (1,'Admin'),(2,'Cliente');
/*!40000 ALTER TABLE `roles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `usuarios`
--

DROP TABLE IF EXISTS `usuarios`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `usuarios` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Nombre` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Email` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `PasswordHash` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Activo` tinyint(1) NOT NULL,
  `RolId` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_usuarios_RolId` (`RolId`),
  CONSTRAINT `FK_usuarios_roles_RolId` FOREIGN KEY (`RolId`) REFERENCES `roles` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usuarios`
--

LOCK TABLES `usuarios` WRITE;
/*!40000 ALTER TABLE `usuarios` DISABLE KEYS */;
INSERT INTO `usuarios` VALUES (3,'Correcamino','daniconio@gmail.com','pollyanna23',1,1),(6,'MarianoBodega','bodegas12345566@gmail.com','pollyanna23',1,2),(8,'AntonioZapato','antoniozapato@gmail.com','pollyanna23',0,2),(9,'Administrador','admin@proyectopyme.com','Admin123',1,1),(10,'UrielMa','oidosnitidos@q.com','pollyanna23',1,2);
/*!40000 ALTER TABLE `usuarios` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-03-17 15:37:37
