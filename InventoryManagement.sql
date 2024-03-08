-- For local use
--use master;
--go
--drop database if exists InventoryManagement;
--go
--create database InventoryManagement;
--go
--alter database InventoryManagement collate Croatian_CI_AS;
--go
--use InventoryManagement;

-- For production use
SELECT name, collation_name FROM sys.databases;
GO
-- Apply the following statements three times, replacing 'db_aa5e80_ivorcelic' with your database name
ALTER DATABASE db_aa5e80_ivorcelic SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
GO
ALTER DATABASE db_aa5e80_ivorcelic COLLATE Croatian_CI_AS;
GO
ALTER DATABASE db_aa5e80_ivorcelic SET MULTI_USER;
GO
SELECT name, collation_name FROM sys.databases;
GO

CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1, 1) NOT NULL,
    FirstName VARCHAR(20) NOT NULL,
    LastName VARCHAR(20) NOT NULL,
    Email VARCHAR(50) NOT NULL,
    Password VARCHAR(100) NOT NULL
);

CREATE TABLE Products (
    Id INT PRIMARY KEY IDENTITY(1, 1) NOT NULL,
    Name VARCHAR(50) NOT NULL,
    Description VARCHAR(100) NULL,
    IsUnitary BIT NOT NULL,
    User INT REFERENCES Users(Id) NOT NULL
);

CREATE TABLE Locations (
    Id INT PRIMARY KEY IDENTITY(1, 1) NOT NULL,
    Name VARCHAR(50) NOT NULL,
    Description VARCHAR(100) NULL
);

CREATE TABLE ProductsLocations (
    Id INT PRIMARY KEY IDENTITY(1, 1) NOT NULL,
    Quantity INT NOT NULL,
    Price DECIMAL NOT NULL,
    Description VARCHAR(255) NULL,
    Product INT REFERENCES Products(Id) NOT NULL,
    Location INT REFERENCES Locations(Id) NOT NULL,
    User INT REFERENCES Users(Id) NOT NULL
);

---------- INSERTS ----------

INSERT INTO Users (FirstName, LastName, Email, Password) VALUES
    ('Ivor', 'Ćelić', 'ivorcelic@gmail.com', 'lozinka'),
    ('Tomislav', 'Jakopec', 'tjakopec@gmail.com', 'TomoCar123'),
    ('Pero', 'Đurić', 'peroduric@gmail.com', 'lozinka');

INSERT INTO Products (Name, Description, IsUnitary, User) VALUES
    ('Cannon LBP6650', 'Printer crno-bijeli', 0, 1),
    ('T-shirt', 'Majice sa EPSO 2023', 1, 2),
    ('Rama za 100m', 'Drvena rama', 1, 1);

INSERT INTO Locations (Name, Description) VALUES
    ('46', 'El.mete, ekrani, televizori'),
    ('41', 'Metalurgija, rame...'),
    ('24', 'Printeri, monitori, stativi, projektori, mete, uredski materijal...'),
    ('26', 'Alati'),
    ('lokacija 1', 'opis 1'),
    ('lokacija 2', 'opis 2'),
    ('lokacija 3', 'opis 3');

INSERT INTO ProductsLocations (Quantity, Price, Description, Product, Location, User) VALUES
    (1, 1000, null, 1, 3, 1),
    (200, 5, '5xM, 10xS, 25xXS', 2, 3, 1),
    (40, 5, '20 komada u neiskoristivom stanju', 3, 2, 2);

---------- SELECT ----------

SELECT 
    b.IsUnitary,
    a.Price,
    a.Description,
    b.Name AS 'Product name',
    c.Name AS 'Location',
    CONCAT(d.FirstName, ' ', d.LastName) AS 'Operator'
FROM 
    ProductsLocations a
    INNER JOIN Products b ON a.Id = b.Id
    INNER JOIN Locations c ON a.Id = c.Id
    INNER JOIN Users d ON a.Id = d.Id;
