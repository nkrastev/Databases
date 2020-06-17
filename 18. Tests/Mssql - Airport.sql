CREATE DATABASE Airport;
CREATE TABLE Planes
(
  Id INT PRIMARY KEY IDENTITY NOT NULL, 
  [Name] VARCHAR(30) NOT NULL,
  [Seats] INT NOT NULL,
  [Range] INT NOT NULL
)
CREATE TABLE Flights
(
	Id INT PRIMARY KEY IDENTITY NOT NULL, 
	DepartureTime datetime,
	ArrivalTime datetime,
	Origin varchar(50) not null,
	Destination varchar(50) not null,
	PlaneId INT not null,
	FOREIGN KEY (PlaneId) REFERENCES Planes(Id)
)