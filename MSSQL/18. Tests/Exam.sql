Create Database TripService
Use TripService

Create Table Cities
(
	Id INT Primary Key Identity,
	[Name] nvarchar(20) NOT NULL,
	CountryCode char(2) NOT NULL
)
Create Table Hotels
(
	Id INT Primary Key Identity,
	[Name] nvarchar(30) NOT NULL,
	CityId INT REFERENCES Cities(Id) NOT NULL,
	EmployeeCount INT NOT NULL,
	BaseRate DECIMAL(15,2)
)
Create Table Rooms
(
	Id INT Primary Key Identity,
	Price DECIMAL (15,2) NOT NULL,
	[Type] nvarchar(20) NOT NULL,
	Beds INT NOT NULL,
	HotelId INT References Hotels(Id) NOT NULL
)
Create Table Trips
(
	Id INT Primary Key Identity,
	RoomId INT References Rooms(Id) NOT NULL,
	ReturnDate DATETIME NOT NULL,
	ArrivalDate DATETIME NOT NULL,
	BookDate DATETIME NOT NULL,
	CancelDate DATETIME,
	CONSTRAINT CheckArrivalDate CHECK (ArrivalDate < ReturnDate),
	CONSTRAINT CheckBookDate CHECK (BookDate < ArrivalDate)

)
Create Table Accounts
(
	Id INT Primary Key Identity,
	FirstName nvarchar(50) NOT NULL,
	MiddleName nvarchar(20),
	LastName nvarchar(50) NOT NULL,
	CityId INT References Cities(Id) NOT NULL,
	BirthDate DATETIME NOT NULL,
	Email varchar(100) NOT NULL UNIQUE
)
Create Table AccountsTrips
(
	AccountId INT References Accounts(Id) NOT NULL,
	TripId INT References Trips(Id) NOT NULL,
	Luggage INT NOT NULL CHECK (Luggage >=0),
	PRIMARY KEY (AccountId, TripId)
)

--2. Insert

INSERT INTO Accounts(FirstName, MiddleName, LastName, CityId, BirthDate, Email) VALUES
('John','Smith','Smith',34,'1975-07-21','j_smith@gmail.com'),
('Gosho',NULL,'Petrov',11,'1978-05-16','g_petrov@gmail.com'),
('Ivan','Petrovich','Pavlov',59,'1849-09-26','i_pavlov@softuni.bg'),
('Friedrich','Wilhelm','Nietzsche',2,'1844-10-15','f_nietzsche@softuni.bg')

INSERT INTO Trips(RoomId, BookDate, ArrivalDate, ReturnDate, CancelDate) VALUES
(101,'2015-04-12','2015-04-14','2015-04-20','2015-02-02'),
(102,'2015-07-07','2015-07-15','2015-07-22','2015-04-29'),
(103,'2013-07-17','2013-07-23','2013-07-24',NULL),
(104,'2012-03-17','2012-03-31','2012-04-01','2012-01-10'),
(109,'2017-08-07','2017-08-28','2017-08-29',NULL)

--3. Update
UPDATE Rooms
SET Price=Price+Price*14/100
WHERE HotelId IN (5,7,9)

--4. Delete

DELETE FROM AccountsTrips Where AccountId=47
DELETE FROM Accounts Where Id=47

--5. EEE-Mails
Select 
	FirstName,
	LastName,
	Format(BirthDate,'MM-dd-yyyy') As BirthDate,
	c.[Name] AS Hometown,
	Email
From Accounts AS a
Join Cities AS c ON a.CityId=c.Id
Where Email Like 'e%'
Order By Hometown

--6. City Statistics
Select
	c.[Name],
	Count(c.[Name]) As Hotels

From Cities As c
Join Hotels As h ON h.CityId=c.Id
Group By c.[Name]
Having Count(c.[Name])>0
Order By Hotels Desc, c.[Name]

--7. Longest and Shortest Trips
Select 
	a.Id As AccountId,
	a.FirstName+' '+a.LastName AS FullName,
	MAX(DATEDIFF(day, ArrivalDate,ReturnDate)) AS LongestTrip,
	MIN(DATEDIFF(day, ArrivalDate,ReturnDate)) AS ShortestTrip

From Accounts As a
Join AccountsTrips As at ON at.AccountId=a.Id
Join Trips As t ON t.Id=at.TripId
Where a.MiddleName IS NULL AND t.CancelDate IS NULL
Group by a.Id, a.FirstName, a.LastName
Order by LongestTrip Desc, ShortestTrip

--8. Metropolis
Select TOP(10)
	c.Id,
	c.[Name],
	c.CountryCode,
	Count(a.Id) As Accounts
		
From Cities As c
Join Accounts As a On c.Id=a.CityId
Group By c.Id, 	c.[Name],	c.CountryCode
Order By Accounts Desc

--9. Romantic Getaways
Select
	a.Id,
	Email,
	c.[Name] As City, 
	Count(c.[Name]) As Trips	
From Accounts As a
Join Cities As c On a.CityId=c.Id
Join AccountsTrips As at ON at.AccountId=a.Id
Join Trips As t ON t.Id=at.TripId
Join Rooms As r ON r.Id=t.RoomId
Join Hotels As h ON h.Id=r.HotelId

Where h.CityId=a.CityId
Group By a.Id, Email,c.[Name] 
Order By Trips Desc, a.Id

--10. GDPR Violation
Select 
	t.Id As Id,
	a.FirstName+ISNULL(' '+MiddleName,'')+' '+a.LastName As [Full Name],	
	c.[Name] As [From],
	(Select [Name] From Cities Where Id= h.CityId) As [To],
	Case
		When CancelDate IS  NOT NULL Then 'Canceled'
		Else
		Cast(DATEDIFF(DAY,t.ArrivalDate,t.ReturnDate) AS nvarchar(5))+' '+'days'
	End As Duration
	
From Trips As t
Join AccountsTrips As at On at.TripId=t.Id
Join Accounts As a On a.Id=at.AccountId
Join Cities As c On c.Id=a.CityId
Join Rooms As r On t.RoomId=r.Id
Join Hotels As h On h.Id=r.HotelId
Order By [Full Name], TripId

--11. Available Room
Create Or Alter Function udf_GetAvailableRoom(@HotelId INT, @Date DATE, @People INT)
RETURNS NVARCHAR(200)
AS
BEGIN
    DECLARE @hotelBaseRate DECIMAL(18,2)
    SET @hotelBaseRate = (SELECT Hotels.BaseRate FROM Hotels WHERE Hotels.Id = @HotelId)
 
    DECLARE @roomId INT
    SET @roomId = (SELECT TOP(1) tempDB.roomId
                    FROM(
                    SELECT Rooms.Id AS roomId, Price, [Type], Beds, ArrivalDate, ReturnDate, CancelDate
                    FROM Rooms
                    JOIN Hotels ON Hotels.Id = Rooms.HotelId
                    JOIN Trips ON Trips.RoomId = Rooms.Id
                    WHERE Hotels.Id = @HotelId AND Rooms.Beds >= @People ) as tempDB
                    WHERE NOT EXISTS (SELECT tempDBTwo.roomId
                                FROM(
                                SELECT RoomsTwo.Id AS roomId, Price, [Type], Beds, ArrivalDate, ReturnDate, CancelDate
                                FROM Rooms as RoomsTwo
                                JOIN Hotels AS HotelsTwo ON HotelsTwo.Id = RoomsTwo.HotelId
                                JOIN Trips AS TripsTwo ON TripsTwo.RoomId = RoomsTwo.Id
                                WHERE HotelsTwo.Id = @HotelId AND RoomsTwo.Beds >= @People ) as tempDBTwo
                                WHERE (CancelDate IS NULL AND @Date > ArrivalDate AND @Date < ReturnDate))
                    ORDER BY tempDB.Price DESC)
 
    IF(@roomId IS NULL) RETURN 'No rooms available'
 
    DECLARE @highestPrice DECIMAL(18,2)
    SET @highestPrice = (SELECT Rooms.Price FROM Rooms WHERE Rooms.Id = @roomId)
 
    DECLARE @roomType NVARCHAR(200);
    SET @roomType = (SELECT Rooms.[Type] FROM Rooms WHERE Rooms.Id = @roomId);
 
    DECLARE @roomBeds INT
    SET @roomBeds = (SELECT Rooms.Beds FROM Rooms WHERE Rooms.Id = @roomId)
 
    DECLARE @totalPrice DECIMAL(18,2)  
    SET @totalPrice = (@hotelBaseRate + @highestPrice) * @People
    RETURN FORMATMESSAGE('Room %i: %s (%i beds) - $%s', @roomId, @roomType, @roomBeds, CONVERT(NVARCHAR(100),@totalPrice))
END

SELECT dbo.udf_GetAvailableRoom(112, '2011-12-17', 2)
SELECT dbo.udf_GetAvailableRoom(94, '2015-07-26', 3)

Create Or Alter Procedure usp_SwitchRoom( @TripID INT, @TargetRoomID INT)
As
Begin
    Declare @HotelID INT; 
	Declare @HotelID2 INT;
	Declare @TripAccounts INT;
	Declare @BedsCounts INT;
		Select @HotelID = HotelID 
		From trips t
		JOIN rooms r ON r.id = t.roomid
		JOIN hotels h ON h.id = r.hotelid
		Where t.id = @TripID
 
	
    Select @HotelID2 = hotelID FROM rooms
		Where id = @TargetRoomID
		Select @TripAccounts INT, @BedsCounts
		Select @TripAccounts = COUNT(*) FROM AccountsTrips
		Where tripID = @TripID
 
    Select @BedsCounts =  Beds FROM rooms
    Where id = @TargetRoomID

    IF @HotelID != @HotelID2
    BEGIN
      RAISERROR('Target room is in another hotel!',16,1)
    END

    ELSE
		BEGIN

			IF @TripAccounts > @BedsCounts
				BEGIN
				  RAISERROR('Not enough beds in target room!',16,1)
				END

			Else
				BEGIN
				  Update Trips 
				  SET RoomId = @TargetRoomID 
				  Where Id = @TripID
				END
		END
END
