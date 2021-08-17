CREATE DATABASE [Service]
USE [Service]
create table Users
(
	Id INT Primary Key Identity,
	Username varchar(30) UNIQUE NOT NULL,
	[Password] varchar(50) NOT NULL,
	[Name] varchar(50),
	Birthdate datetime ,
	Age INT CHECK (Age >= 14 AND Age <= 110),
	Email varchar (50) NOT NULL
)
Create table Departments
(
	Id INT Primary Key Identity,
	[Name] varchar(50) not null
)
Create Table Employees
(
	Id INT Primary Key Identity,
	FirstName varchar(25),
	LastName Varchar(25),
	Birthdate Datetime,
	Age INT CHECK (Age >= 18 AND Age <= 110),
	DepartmentId INT REFERENCES Departments(Id) 
)
Create Table Categories
(
	Id INT Primary Key Identity,
	[Name] varchar(50) not null,
	DepartmentId INT REFERENCES Departments(Id) NOT NULL
)
Create Table [Status]
(
	Id INT Primary Key Identity,
	[Label] varchar(50) not null
)
Create Table Reports
(
	Id INT Primary Key Identity,
	CategoryId INT FOREIGN KEY REFERENCES Categories(Id) NOT NULL ,
	StatusId INT FOREIGN KEY REFERENCES [Status](Id) NOT NULL , 
	OpenDate Datetime NOT NULL,
	CloseDate Datetime,
	[Description] varchar(200) not null,
	UserId INT REFERENCES Users(Id) NOT NULL,
	EmployeeId INT REFERENCES Employees(Id)
)

INSERT INTO Employees (FirstName,LastName,Birthdate,DepartmentId)
VALUES
('Marlo','O''Malley','1958-9-21',1),
('Niki','Stanaghan','1969-11-26',4),
('Ayrton','Senna','1960-03-21',9),
('Ronnie','Peterson','1944-02-14',9),
('Giovanna','Amati','1959-07-20',5)

INSERT INTO Reports (CategoryId,StatusId,OpenDate,CloseDate,[Description],UserId,EmployeeId)
VALUES
(1,1,'2017-04-13',NULL,'Stuck Road on Str.133',6,2),
(6,3,'2015-09-05','2015-12-06','Charity trail running',3,5),
(14,2,'2015-09-07',NULL,'Falling bricks on Str.58',5,2),
(4,3,'2017-07-03','2017-07-06','Cut off streetlight on Str.11',1,1)

UPDATE Reports SET
CloseDate=GETDATE() WHERE CloseDate IS NULL

DELETE FROM Reports WHERE StatusId=4

SELECT [Description], FORMAT(OpenDate, 'dd-MM-yyyy') AS OpenDate
FROM Reports
WHERE EmployeeId IS NULL
ORDER BY Reports.OpenDate ASC, [Description] ASC

SELECT [Description], c.[Name] AS CategoryName
FROM Reports AS r
JOIN Categories AS c ON c.Id=r.CategoryId
ORDER BY [Description] ASC, CategoryName ASC

SELECT TOP(5)
	[Name] AS CategoryName,
	Count(CategoryId) AS ReportsNumber
FROM Reports AS r
JOIN Categories AS c ON c.Id=r.CategoryId
GROUP BY [Name]
ORDER BY ReportsNumber DESC, [Name] ASC

SELECT Username, c.[Name] AS CategoryName
FROM Users AS u
JOIN Reports AS r ON u.Id=r.UserId
JOIN Categories AS c ON c.Id=r.CategoryId
WHERE 
(DATEPART(month,u.Birthdate)=DATEPART(month,r.OpenDate))AND
(DATEPART(day,u.Birthdate)=DATEPART(day,r.OpenDate))
ORDER BY Username, CategoryName

SELECT 
	e.FirstName +' '+ e.LastName AS FullName, 	
	COUNT(DISTINCT UserId) AS UsersCount
FROM Employees AS e
JOIN Reports AS r ON e.Id=r.EmployeeId
GROUP BY EmployeeId, FirstName, LastName
ORDER BY UsersCount DESC,FullName ASC

SELECT 
	ISNULL(e.FirstName+' '+e.LastName,'None') AS Employee,
	ISNULL(d.[Name],'None') As Department,
	ISNULL(c.[Name],'None') AS Category,
	ISNULL(r.[Description],'None') AS [Description],
	FORMAT(r.OpenDate, 'dd.MM.yyyy') AS OpenDate,
	ISNULL(s.[Label],'None') AS [Status],
	ISNULL(u.[Name],'None') AS [User]
FROM Reports AS r
LEFT JOIN Employees AS e ON r.EmployeeId=e.Id
LEFT JOIN Categories AS c ON r.CategoryId=c.Id
LEFT JOIN Departments AS d ON d.Id=c.DepartmentId
LEFT JOIN Users AS u ON u.Id=r.UserId
LEFT JOIN [Status] AS s ON s.Id=r.StatusId
ORDER BY
FirstName DESC,
LastName DESC,
Department ASC,
Category ASC,
[Description] ASC,
OpenDate ASC,
[Status] ASC,
[User] ASC

CREATE OR ALTER FUNCTION udf_HoursToComplete(@StartDate DATETIME, @EndDate DATETIME)
RETURNS INT
-- ah tozi Judge *%(%#!^&(%(!#%
AS
BEGIN
	IF @StartDate IS NULL RETURN (0)
	IF @EndDate IS NULL RETURN (0)
	RETURN (DATEDIFF ( hour , @StartDate , @EndDate ))
END
SELECT OpenDate, CloseDate FROM Reports
SELECT dbo.udf_HoursToComplete(OpenDate, CloseDate) AS TotalHours
   FROM Reports

CREATE OR ALTER PROCEDURE usp_AssignEmployeeToReport (@EmployeeId INT, @ReportId INT)
AS
	DECLARE @DepIdOfEmployee INT;
	DECLARE @DepReportCategory INT;
	SET @DepIdOfEmployee=(SELECT DepartmentId FROM Employees WHERE Id=@EmployeeId);
	SET @DepReportCategory=(
		SELECT DepartmentId FROM Categories AS c
		JOIN Reports AS r ON r.CategoryId=c.Id		
		WHERE r.Id=@ReportId);

	If (@DepIdOfEmployee<>@DepReportCategory)
		THROW 50001, 'Employee doesn''t belong to the appropriate department!',1;
	UPDATE Reports SET EmployeeId=@EmployeeId WHERE Id=@ReportId;
GO
EXEC usp_AssignEmployeeToReport 30, 1




