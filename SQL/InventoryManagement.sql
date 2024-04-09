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


-- Drop all tables
drop table if exists InventoryTransactionItems;
drop table if exists InventoryTransactions;
drop table if exists TransactionStatuses;
drop table if exists Products;
drop table if exists Employees;
drop table if exists Warehouses;


-- Create tables
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

CREATE TABLE TransactionStatuses (
	Id int primary key identity (1, 1) not null,
	StatusName varchar(50) not null
);

CREATE TABLE InventoryTransactions (
    Id int primary key identity (1, 1) not null,
    Employee int references Employees (Id) not null,
    TransactionStatus int references TransactionStatuses (Id) not null,
    TransactionDate datetime,
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
values 
    ('John', 'Doe', 'john.doe@example.com', 'password123'),
    ('Jane', 'Smith', 'jane.smith@example.com', 'securepass'),
    ('Michael', 'Johnson', 'michael.johnson@example.com', 'pass1234'),
    ('Alice', 'Johnson', 'alice.johnson@example.com', 'pass1234'),
    ('Bob', 'Williams', 'bob.williams@example.com', 'password123'),
    ('Emily', 'Brown', 'emily.brown@example.com', 'securepass'),
    ('Daniel', 'Miller', 'daniel.miller@example.com', 'pass1234'),
    ('Olivia', 'Davis', 'olivia.davis@example.com', 'password123'),
    ('William', 'Garcia', 'william.garcia@example.com', 'securepass'),
    ('Sophia', 'Martinez', 'sophia.martinez@example.com', 'pass1234');


-- insert into warehouses
insert into Warehouses (WarehouseName, Description)
values 
    ('Warehouse A', 'Main warehouse for storing electronics'),
    ('Warehouse B', 'Secondary warehouse for storing clothing'),
    ('Warehouse D', 'Warehouse for storing books'),
    ('Warehouse E', 'Warehouse for storing home appliances'),
    ('Warehouse F', 'Warehouse for storing furniture'),
    ('Warehouse G', 'Warehouse for storing sports equipment'),
    ('Warehouse I', 'Warehouse for storing cosmetics');


-- insert into products
insert into Products (ProductName, Description, IsUnitary)
values 
    ('High-performance Laptop', 'High-performance laptop with SSD', 0),
    ('Cotton Polo Shirt', 'Comfortable cotton polo shirt for casual wear', 1),
    ('Latest Smartphone', 'Latest smartphone model with OLED display', 0),
    ('Gaming Mouse', 'Ergonomic gaming mouse for precise control', 0),
    ('Running Shoes', 'Lightweight running shoes for fitness enthusiasts', 1),
    ('Wireless Headphones', 'Premium wireless headphones with noise-cancellation', 0),
    ('Portable Blender', 'Compact portable blender for making smoothies on the go', 0),
    ('Digital Camera', 'High-resolution digital camera for capturing memories', 0),
    ('Smart Watch', 'Intelligent smartwatch with fitness tracking features', 0),
    ('Home Theater System', 'Immersive home theater system for cinematic experience', 0),
    ('Handheld Vacuum Cleaner', 'Compact handheld vacuum cleaner for quick cleaning tasks', 0),
    ('Fitness Tracker', 'Wearable fitness tracker to monitor physical activity', 0),
    ('Smart Thermostat', 'Programmable smart thermostat for energy-efficient heating/cooling', 0),
    ('Electric Toothbrush', 'High-tech electric toothbrush for effective oral hygiene', 0),
    ('Robot Vacuum Cleaner', 'Autonomous robot vacuum cleaner for automated floor cleaning', 0),
    ('Security Camera System', 'Comprehensive security camera system for surveillance', 0),
    ('Wireless Router', 'High-speed wireless router for reliable internet connectivity', 0),
    ('External Hard Drive', 'Portable external hard drive for data storage and backup', 0),
    ('Coffee Maker', 'Automatic coffee maker for brewing fresh coffee at home', 0),
    ('Bluetooth Speaker', 'Portable Bluetooth speaker for wireless audio streaming', 0);


-- insert into transactionstatuses
insert into TransactionStatuses (StatusName)
values 
    ('Transaction opened'),
    ('Transaction closed');


-- insert into inventorytransactions
insert into InventoryTransactions (Employee, TransactionStatus, TransactionDate, AdditionalDetails)
values 
    (1, 1, getdate(), 'Inventory for 2023.'),
    (2, 2, getdate(), 'Inventory for 2024.'),
    (3, 1, getdate(), 'Inventory for 2025.'),
    (4, 2, getdate(), 'Inventory for 2026.'),
    (5, 1, getdate(), 'Inventory for 2027.');


-- Insert into inventorytransactionitems
insert into InventoryTransactionItems (InventoryTransaction, Product, Warehouse, Quantity)
values
-- InventoryTransactionId = 1
    (1, 1, 1, 12),
    (1, 2, 2, 3),
    (1, 3, 3, 5),
    (1, 4, 3, 1),
    (1, 5, 5, 10),
    (1, 6, 5, 1),
    (1, 7, 5, 1),  
    (1, 8, 2, 11),  
    (1, 9, 4, 23),  
    (1, 10, 1, 50), 
    (1, 11, 1, 6), 
    (1, 12, 1, 1),  
    (1, 13, 2, 8),  
    (1, 14, 3, 1),  
    (1, 15, 5, 12),  
    (1, 16, 1, 1), 
    (1, 17, 1, 19), 
    (1, 18, 2, 41),  
    (1, 19, 4, 1),  
    (1, 20, 4, 167),
-- InventoryTransactionID = 2
	(2, 1, 6, 12),
    (2, 2, 7, 13),
    (2, 3, 3, 11),
    (2, 4, 4, 51),
    (2, 5, 4, 17),
    (2, 6, 2, 91),
    (2, 7, 2, 26),
    (2, 8, 4, 3),
    (2, 9, 4, 1),
    (2, 10, 6, 1),
    (2, 11, 6, 1),
    (2, 12, 7, 1),
    (2, 13, 7, 1),
    (2, 14, 3, 1),
    (2, 15, 4, 1),
    (2, 16, 5, 1),
    (2, 17, 6, 1),
    (2, 18, 7, 1),
    (2, 19, 5, 1),
    (2, 20, 3, 1),
-- InventoryTransactionID = 3
    (3, 1, 3, 8),
    (3, 2, 4, 15),
    (3, 3, 2, 9),
    (3, 4, 5, 7),
    (3, 5, 1, 12),
    (3, 6, 3, 5),
    (3, 7, 1, 10),
    (3, 8, 5, 3),
    (3, 9, 2, 20),
    (3, 10, 4, 8),
    (3, 11, 5, 3),
    (3, 12, 1, 1),
    (3, 13, 2, 2),
    (3, 14, 4, 1),
    (3, 15, 5, 14),
    (3, 16, 4, 2),
    (3, 17, 3, 1),
    (3, 18, 1, 4),
    (3, 19, 5, 1),
    (3, 20, 2, 6),
-- InventoryTransactionID = 4
    (4, 1, 2, 6),
    (4, 2, 5, 8),
    (4, 3, 3, 10),
    (4, 4, 1, 15),
    (4, 5, 4, 9),
    (4, 6, 1, 4),
    (4, 7, 5, 7),
    (4, 8, 3, 11),
    (4, 9, 2, 6),
    (4, 10, 4, 5),
    (4, 11, 3, 2),
    (4, 12, 2, 1),
    (4, 13, 1, 3),
    (4, 14, 4, 2),
    (4, 15, 5, 10),
    (4, 16, 3, 1),
    (4, 17, 1, 2),
    (4, 18, 5, 5),
    (4, 19, 4, 3),
    (4, 20, 2, 7),
-- InventoryTransactionID = 5
    (5, 1, 4, 11),
    (5, 2, 2, 7),
    (5, 3, 5, 13),
    (5, 4, 3, 6),
    (5, 5, 1, 9),
    (5, 6, 2, 5),
    (5, 7, 4, 8),
    (5, 8, 3, 12),
    (5, 9, 5, 4),
    (5, 10, 1, 10),
    (5, 11, 2, 3),
    (5, 12, 5, 2),
    (5, 13, 3, 6),
    (5, 14, 4, 8),
    (5, 15, 1, 5),
    (5, 16, 2, 2),
    (5, 17, 3, 4),
    (5, 18, 4, 7),
    (5, 19, 5, 3),
    (5, 20, 1, 6);