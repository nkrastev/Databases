CREATE DATABASE Table05OSD
USE Table05OSD
Create Table Cities(
	CityID INT Primary Key,
	[Name] VARCHAR(50)
)
Create Table Customers(
	CustomerID INT Primary Key,
	[Name] VARCHAR(50),
	Birthday DATE,
	CityID INT NOT NULL Foreign Key References Cities(CityID)
)
Create Table Orders(
	OrderID INT Primary Key,
	CustomerID INT NOT NULL Foreign Key References Customers(CustomerID)
)
/*From the other side of the task of tables*/
Create Table ItemTypes(
	ItemTypeID INT Primary Key,
	[Name] VARCHAR (50)
)
Create Table Items(
	ItemID INT Primary Key,
	[Name] VARCHAR (50),
	ItemTypeID INT NOT NULL Foreign Key References ItemTypes(ItemTypeID)
)
/*Multiple Connections table*/
Create Table OrderItems(
	ItemID INT Foreign Key References Items(ItemID) not null,
	OrderID INT Foreign Key References Orders(OrderID) not null,	
	Primary Key (OrderID,ItemID)
)
