-- For local use
use master;
go
drop database if exists InventoryManagement;
go
create database InventoryManagement;
go
alter database InventoryManagement collate Croatian_CI_AS;
go
use InventoryManagement;


-- For production use
SELECT name, collation_name FROM sys.databases;
GO
ALTER DATABASE db_aa5e80_ivorcelic SET SINGLE_USER WITH
ROLLBACK IMMEDIATE;
GO
ALTER DATABASE db_aa5e80_ivorcelic COLLATE Croatian_CI_AS;
GO
ALTER DATABASE db_aa5e80_ivorcelic SET MULTI_USER;
GO
SELECT name, collation_name FROM sys.databases;
GO


CREATE TABLE Employees (
    Id int primary key identity (1, 1) not null,
    FirstName varchar(50) not null,
    LastName varchar(50) not null,
    Email varchar(50) not null,
    Password varchar(100) not null
    -- Role varchar(50)
);

CREATE TABLE Warehouses (
    Id int primary key identity (1, 1) not null,
    WarehouseName varchar(100) not null,
    Description varchar(255),
);

CREATE TABLE Products (
    Id int primary key identity (1, 1) not null,
    ProductName varchar(100) not null,
    Description varchar(255),
    IsUnitary bit not null
);

CREATE TABLE InventoryTransactions (
    Id int primary key identity (1, 1) not null,
    Employee int references Employees (Id) not null,
    TransactionStatus int not null,
    TransactionDateTime datetime,
    AdditionalDetails varchar(255)
);

CREATE TABLE InventoryTransactionItems (
    Id int primary key identity (1, 1) not null,
    InventoryTransaction int references InventoryTransactions (Id) not null,
    Product int references Products (Id) not null,
    Warehouse int references Warehouses (Id) not null,
    Quantity int
);



-- insert into employees
insert into Employees (FirstName, LastName, Email, Password)
values ('John', 'Doe', 'john.doe@example.com', 'password123'),
       ('Jane', 'Smith', 'jane.smith@example.com', 'securepass'),
       ('Michael', 'Johnson', 'michael.johnson@example.com', 'pass1234');

-- insert into warehouses
insert into Warehouses (WarehouseName, Description)
values ('Warehouse A', 'Main warehouse for storing electronics'),
       ('Warehouse B', 'Secondary warehouse for storing clothing'),
       ('Warehouse C', 'Overflow warehouse for general items');

-- insert into products
insert into Products (ProductName, Description, IsUnitary)
values ('Laptop', 'High-performance laptop with SSD', 1),
       ('T-shirt', 'Cotton t-shirt for casual wear', 1),
       ('Smartphone', 'Latest smartphone model with OLED display', 1);

-- insert into inventorytransactions
insert into InventoryTransactions (Employee, TransactionStatus, TransactionDateTime, AdditionalDetails)
values (1, 'Completed', getdate(), 'Received new shipment of laptops'),
       (2, 'In progress', getdate(), 'Preparing order for shipment'),
       (3, 'Planned', getdate(), 'Inventory audit scheduled for next week');

-- insert into inventorytransactionitems
insert into InventoryTransactionItems (InventoryTransaction, Product, Warehouse, Quantity)
values (1, 1, 1, 10),
       (2, 3, 2, 20),
       (3, 2, 3, 50);
