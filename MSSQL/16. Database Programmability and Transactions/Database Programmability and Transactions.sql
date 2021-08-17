--Problem 1. Employees with Salary Above 35000
CREATE OR ALTER PROCEDURE usp_GetEmployeesSalaryAbove35000
AS
SELECT [FirstName],[LastName]
FROM Employees
WHERE Salary>35000
GO
EXEC usp_GetEmployeesSalaryAbove35000

--Problem 2. Employees with Salary Above Number
CREATE OR ALTER PROCEDURE usp_GetEmployeesSalaryAboveNumber (@Salary DECIMAL(18,4))
AS
SELECT [FirstName], [LastName]
FROM Employees
WHERE Salary>=@Salary
GO
EXEC usp_GetEmployeesSalaryAboveNumber 50000

--Problem 3. Town Names Starting With
CREATE OR ALTER PROCEDURE usp_GetTownsStartingWith ( @StartWith nvarchar(50))
AS
SELECT [Name] FROM Towns WHERE [Name] LIKE @StartWith+'%'
SELECT @StartWith
GO
EXEC usp_GetTownsStartingWith 'Sno'

--Problem 4. Employees from Town
CREATE OR ALTER PROCEDURE usp_GetEmployeesFromTown ( @TownName nvarchar(50) )
AS
SELECT
e.[FirstName],e.[LastName]
FROM Employees AS e
LEFT JOIN Addresses AS a ON e.AddressID=a.AddressID
LEFT JOIN Towns AS t ON t.TownID=a.TownID
WHERE t.Name=@TownName
GO
EXEC usp_GetEmployeesFromTown Sofia

--Problem 5. Salary Level Function
CREATE OR ALTER FUNCTION ufn_GetSalaryLevel(@salary DECIMAL(18,4))
RETURNS nvarchar(20)
AS
BEGIN
DECLARE @SalaryLevel nvarchar(20);
IF @salary<30000 SET @SalaryLevel='Low'
IF (@salary>=30000 AND @salary<=50000) SET @SalaryLevel='Average'
IF @salary>50000 SET @SalaryLevel='High'
RETURN @SalaryLevel;
END
SELECT dbo.ufn_GetSalaryLevel(15000)

--Problem 6. Employees by Salary Level
CREATE OR ALTER PROCEDURE usp_EmployeesBySalaryLevel (@LevelOfSalary nvarchar(50))
AS
SELECT
[FirstName],[LastName]
FROM Employees

WHERE dbo.ufn_GetSalaryLevel(Salary)=@LevelOfSalary

GO
EXEC usp_EmployeesBySalaryLevel 'High'

--Problem 7. Define Function
--Problem 7. Define Function
CREATE OR ALTER FUNCTION ufn_IsWordComprised(@setOfLetters nvarchar(50), @word nvarchar(50))
RETURNS nvarchar(50) AS
BEGIN
DECLARE @Result nvarchar(50);
DECLARE @LenghtOfWord int;
DECLARE @LenghtOfSet int;
DECLARE @i int;
DECLARE @LetterFromWord nvarchar(1);

SET @LenghtOfWord=LEN(@word);
SET @i=0;
SET @Result='minava li loop-a?';

WHILE (@i<=@LenghtOfWord)
BEGIN
SET @LetterFromWord =SUBSTRING(@Word, @i, 1);

IF (CHARINDEX(@LetterFromWord, @setOfLetters)>0 )
SET @Result='OK E';
ELSE
BEGIN
SET @Result='nqma nqkoq bukva';
BREAK;
END

SET @i=@i+1;
END

RETURN @Result;
END
SELECT dbo.ufn_IsWordComprised('Sofia', 'So')
--neshto ne raboti korektno