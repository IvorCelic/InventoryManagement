CREATE TABLE Users (
    UserID INT PRIMARY KEY,
    Username NVARCHAR(50),
    Password NVARCHAR(100),
    Role NVARCHAR(50)
);

CREATE TABLE Warehouses (
    WarehouseID INT PRIMARY KEY,
    WarehouseName NVARCHAR(100),
    Location NVARCHAR(100),
    Capacity INT
);

CREATE TABLE Products (
    ProductID INT PRIMARY KEY,
    ProductName NVARCHAR(100),
    Description NVARCHAR(MAX),
    UnitPrice DECIMAL(18, 2),
    QuantityOnHand INT
);

CREATE TABLE InventoryTransactions (
    TransactionID INT PRIMARY KEY,
    UserID INT,
    TransactionType NVARCHAR(50),
    TransactionStatus NVARCHAR(50),
    TransactionDateTime DATETIME,
    AdditionalDetails NVARCHAR(MAX),
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

CREATE TABLE InventoryTransactionItems (
    TransactionItemID INT PRIMARY KEY,
    TransactionID INT,
    ProductID INT,
    WarehouseID INT,
    Quantity INT,
    FOREIGN KEY (TransactionID) REFERENCES InventoryTransactions(TransactionID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID),
    FOREIGN KEY (WarehouseID) REFERENCES Warehouses(WarehouseID)
);
