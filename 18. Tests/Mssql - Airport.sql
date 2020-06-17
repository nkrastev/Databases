CREATE DATABASE Airport;
USE Airport;
CREATE TABLE Planes
(
  Id INT PRIMARY KEY IDENTITY, 
  [Name] VARCHAR(30) NOT NULL,
  [Seats] INT NOT NULL,
  [Range] INT NOT NULL
)
CREATE TABLE Flights
(
	Id INT PRIMARY KEY IDENTITY, 
	DepartureTime datetime,
	ArrivalTime datetime,
	Origin varchar(50) not null,
	Destination varchar(50) not null,
	PlaneId INT FOREIGN KEY REFERENCES Planes(Id) NOT NUll
)
CREATE TABLE Passengers
(
	Id INT PRIMARY KEY IDENTITY,
	FirstName varchar(30) NOT NULL,
	LastName varchar(30) NOT NULL,
	Age INT NOT NULL,
	[Address] varchar(30) NOT NULL,
	PassportId char(11) NOT NULL
)
CREATE TABLE LuggageTypes
(
	Id INT PRIMARY KEY IDENTITY,
	[Type] varchar(30) NOT NULL
)
CREATE TABLE Luggages
(
	Id INT PRIMARY KEY IDENTITY,
	LuggageTypeId INT FOREIGN KEY REFERENCES LuggageTypes(Id) NOT NULL,
	PassengerId INT FOREIGN KEY REFERENCES Passengers(Id) NOT NULL
)

CREATE TABLE Tickets
(
	Id INT PRIMARY KEY IDENTITY,
	PassengerId INT FOREIGN KEY REFERENCES Passengers(Id) NOT NULL,
	FlightId INT FOREIGN KEY REFERENCES Flights(Id) NOT NULL,
	LuggageId INT FOREIGN KEY REFERENCES Luggages(Id) NOT NULL,
	Price DECIMAL(15,2) NOT NULL
)
INSERT INTO Planes ([Name], Seats, [Range]) VALUES
	('Airbus 336', 112, 5132),
	('Airbus 330', 432, 5325),
	('Boeing 369', 231, 2355),
	('Stelt 297', 254,2143),
	('Boeing 338', 165,5111),
	('Airbus 558', 387,1342),
	('Boeing 128', 345,5541)

INSERT INTO LuggageTypes ([Type]) VALUES
	('Crossbody Bag'),
	('School Backpack'),
	('Shoulder Bag')

SELECT * FROM Flights WHERE Destination='Carlsbad'

SELECT * FROM Tickets AS t
LEFT JOIN Flights AS f ON t.FlightId=f.Id
WHERE f.Destination='Carlsbad'

UPDATE Tickets
SET Price=t.Price*1.13
FROM Tickets AS t
JOIN Flights AS f ON t.FlightId=f.Id
WHERE f.Destination='Carlsbad'

DELETE FROM Tickets
WHERE FlightId = (SELECT TOP(1) Id FROM Flights WHERE Destination = 'Ayn Halagim')
DELETE FROM Flights 
WHERE Destination='Ayn Halagim'

SELECT * 
FROM Planes
WHERE [Name] LIKE '%tr%'
ORDER BY Id ASC, [Name] ASC, Seats ASC, [Range] ASC

SELECT FlightId, Sum(Price) AS Price
FROM Tickets
GROUP BY FlightId
ORDER BY Price DESC, FlightId ASC

SELECT 
	FirstName + ' ' + LastName AS [Full Name],
	f.Origin,
	f.Destination
FROM Passengers AS p
JOIN Tickets AS t ON p.Id=t.PassengerId
JOIN Flights AS f ON t.FlightId=f.Id
ORDER BY [Full Name] ASC, Origin ASC, Destination ASC

SELECT 
	FirstName, 
	LastName,
	Age
FROM Passengers AS p
LEFT JOIN Tickets AS t ON p.Id=t.PassengerId
WHERE FlightId IS NULL
ORDER BY Age DESC, FirstName ASC, LastName ASC

SELECT 
	FirstName + ' ' + LastName AS [Full Name],
	pl.[Name] AS [Plane Name],
	f.Origin + ' - ' +f.Destination AS Trip,
	lt.[Type]
FROM Passengers AS p
JOIN Tickets AS t ON p.Id=t.PassengerId
JOIN Flights AS f ON t.FlightId=f.Id
JOIN Planes AS pl ON f.PlaneId=pl.Id
JOIN Luggages AS l ON l.Id = t.LuggageId
JOIN LuggageTypes As lt ON lt.Id=l.LuggageTypeId
ORDER BY [Full Name] ASC, [Name] ASC, Origin ASC, Destination ASC, [Type] ASC

SELECT [Name], Seats, COUNT(t.Id) AS [Passengers Count]
FROM Planes AS p
LEFT JOIN Flights AS f ON f.PlaneId=p.Id
LEFT JOIN Tickets AS t ON t.FlightId=f.Id
GROUP BY [Name], [Seats]
ORDER BY [Passengers Count] DESC, p.[Name] ASC, p.Seats ASC

CREATE OR ALTER FUNCTION udf_CalculateTickets
	(
		@origin AS Varchar(50), 
		@destination AS varchar(50), 
		@peopleCount AS INT
	) 
RETURNS VARCHAR(100)
AS
BEGIN
	IF (@peopleCount <= 0) 
		RETURN 'Invalid people count!';
	DECLARE @IsPeopleOnFlight INT;
	SET @IsPeopleOnFlight=(SELECT f.Id FROM Flights AS f JOIN Tickets AS t ON t.FlightId = f.Id 										  WHERE Destination = @destination AND Origin = @origin);
	IF (@IsPeopleOnFlight IS NULL)
		RETURN 'nema polet';

    DECLARE @ticketPrice DECIMAL(15,2) = (SELECT t.Price FROM Flights AS f
											  JOIN Tickets AS t ON t.FlightId = f.Id 
											  WHERE Destination = @destination AND Origin = @origin)

	DECLARE @totalPrice DECIMAL(15, 2) = @ticketPrice * @peoplecount;

	RETURN 'Total price ' + CAST(@totalPrice as VARCHAR(30));
	
END
SELECT dbo.udf_CalculateTickets('Kolyddddshley','Rancabolang', 15)

SELECT COUNT(Id) FROM Flights WHERE Origin LIKE 'Kolyshley' AND Destination LIKE 'Rancabolang'

CREATE OR ALTER PROCEDURE usp_CancelFlights
AS
 UPDATE Flights
 SET ArrivalTime=NULL, DepartureTime=NULL
 WHERE ArrivalTime>DepartureTime

GO

EXEC usp_CancelFlights
