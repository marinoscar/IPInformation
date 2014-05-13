DROP SCHEMA IF EXISTS `GeoLocation`;

CREATE SCHEMA  `GeoLocation`;

USE `GeoLocation`;

CREATE TABLE `Country`
(
	`Id` int NOT NULL AUTO_INCREMENT,
	`Code` varchar(4) NOT NULL UNIQUE KEY,
	`LongCode` varchar(6) NOT NULL UNIQUE KEY,
	`NumericCode` int NOT NULL UNIQUE KEY,
	`EnglishName` varchar(120) NOT NULL,
	`FrenchName` varchar(120) NOT NULL,
	CONSTRAINT PK_Country PRIMARY KEY (`Id`)
);

CREATE TABLE `Location`
(
	`Id` int NOT NULL AUTO_INCREMENT,
	`LocationId` int NOT NULL UNIQUE KEY,
	`CountryCode` varchar(2) NOT NULL,
	`Region` varchar(60) NULL,
	`City` varchar(60) NULL,
	`PostalCode` varchar(25) NULL,
	`Latitude` double NULL,
	`Longitude` double NULL,
	`MetroCode` varchar(25) NULL,
	`AreaCode` varchar(25) NULL,
	`TimeZoneId` int NOT NULL DEFAULT 0, 
	CONSTRAINT PK_Location PRIMARY KEY (`Id`)
);

CREATE TABLE `TimeZone`
(
	`Id` int NOT NULL AUTO_INCREMENT,
	`TimeZoneId` varchar(100) NOT NULL UNIQUE,
	`TimeZoneNameEnglish` varchar(200) NOT NULL,
	`TimeZoneNameSpanish` varchar(200) NOT NULL,
	`UtcOffSet` double NULL,
	`UtcDstOffSet` double NULL,
	CONSTRAINT PK_TimeZone PRIMARY KEY (`Id`)
);

CREATE TABLE `IpBlock`
(
	`Id` int NOT NULL AUTO_INCREMENT,
	`LocationId` int NOT NULL,
	`StartIpNumber` bigint unsigned NOT NULL,
	`EndIpNumber` bigint unsigned NOT NULL,
	CONSTRAINT PK_IpBlock PRIMARY KEY (`Id`)
);

CREATE INDEX IX_IpBlock_IpNumber ON IpBlock (StartIpNumber, EndIpNumber) USING BTREE;


