IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'RedarborStoreDB')
BEGIN
    CREATE DATABASE RedarborStoreDB;
END
GO

USE RedarborStoreDB;
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Categories')
BEGIN
    CREATE TABLE Categories (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Name NVARCHAR(100) NOT NULL,
        Description NVARCHAR(500),
        CreatedDate DATETIME DEFAULT GETDATE(),
        IsDeleted BIT DEFAULT 0,
        DeletedDate DATETIME NULL
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Products')
BEGIN
    CREATE TABLE Products (
        Id INT PRIMARY KEY IDENTITY(1,1),
        CategoryId INT NOT NULL,
        Name NVARCHAR(150) NOT NULL,
        Description NVARCHAR(MAX),
        Price DECIMAL(18, 2) NOT NULL DEFAULT 0.00,
        Stock INT NOT NULL DEFAULT 0,
        CreatedDate DATETIME DEFAULT GETDATE(),
        UpdatedDate DATETIME DEFAULT GETDATE(),
        IsDeleted BIT DEFAULT 0,
        DeletedDate DATETIME NULL
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'InventoryMovements')
BEGIN
    CREATE TABLE InventoryMovements (
        Id INT PRIMARY KEY IDENTITY(1,1),
        ProductId INT NOT NULL,
        MovementType NVARCHAR(20) NOT NULL, 
        Quantity INT NOT NULL,
        Reason NVARCHAR(250),
        MovementDate DATETIME DEFAULT GETDATE()
    );
END
GO

IF NOT EXISTS (SELECT * FROM Categories)
BEGIN
    INSERT INTO Categories (Name, Description) VALUES 
    ('Electrónica', 'Dispositivos y gadgets electrónicos'),
    ('Hogar', 'Artículos para el hogar y cocina');
    
    INSERT INTO Products (CategoryId, Name, Price, Stock) VALUES 
    (1, 'Laptop Gaming', 1200.00, 10),
    (1, 'Smartphone Pro', 800.00, 25);
END
GO