CREATE TABLE roamler.Locations (
	Address TEXT NULL,
	Latitude DECIMAL(10,8) NOT NULL,
	Longitude DECIMAL(11,8) NOT NULL,
	Id INT auto_increment NOT NULL,
	CONSTRAINT Location_PK PRIMARY KEY (Id)
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8mb4
COLLATE=utf8mb4_0900_ai_ci;

SET GLOBAL local_infile=1;
LOAD DATA INFILE '/var/lib/mysql-files/locations.csv' INTO TABLE roamler.Locations  FIELDS TERMINATED BY ',' ENCLOSED BY '"' LINES TERMINATED BY "\r\n" IGNORE 1 LINES (Address, Latitude, Longitude);