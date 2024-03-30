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
    (1, 1, 1, 1),
    (1, 2, 2, 1),
    (1, 3, 3, 1),
    (1, 4, 3, 1),
    (1, 5, 5, 1),
    (1, 6, 5, 1),
    (1, 7, 5, 1),  
    (1, 8, 2, 1),  
    (1, 9, 4, 1),  
    (1, 10, 1, 1), 
    (1, 11, 1, 1), 
    (1, 12, 1, 1),  
    (1, 13, 2, 1),  
    (1, 14, 3, 1),  
    (1, 15, 5, 1),  
    (1, 16, 1, 1), 
    (1, 17, 1, 1), 
    (1, 18, 2, 1),  
    (1, 19, 4, 1),  
    (1, 20, 4, 1),
-- InventoryTransactionID = 2
    (2, 1, 6, 1),
    (2, 2, 7, 1),
    (2, 3, 3, 1),
    (2, 4, 4, 1),
    (2, 5, 4, 1),
    (2, 6, 2, 1),
    (2, 7, 2, 1),
    (2, 8, 4, 1),
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
    (2, 20, 3, 1);



