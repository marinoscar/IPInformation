USE `GeoLocation`;

LOAD DATA INFILE 'C:\\GeoLocation\\CountryCodes.csv' INTO TABLE `Country`
	FIELDS TERMINATED BY ','
	OPTIONALLY ENCLOSED BY '"'
	IGNORE 1 LINES
(`EnglishName`, `FrenchName`, `Code`, `LongCode`, `NumericCode`);

LOAD DATA INFILE 'C:\\GeoLocation\\GeoLiteCity-Location.csv' INTO TABLE `Location`
	FIELDS TERMINATED BY ','
	OPTIONALLY ENCLOSED BY '"'
	IGNORE 2 LINES
(`LocationId`, `CountryCode`, `Region`, `City`, `PostalCode`, `Latitude`, `Longitude`, `MetroCode`, `AreaCode`);


LOAD DATA INFILE 'C:\\GeoLocation\\GeoLiteCity-Blocks.csv' INTO TABLE `IpBlock`
	FIELDS TERMINATED BY ','
	OPTIONALLY ENCLOSED BY '"'
	IGNORE 2 LINES
(`StartIpNumber`, `EndIpNumber`, `LocationId`);