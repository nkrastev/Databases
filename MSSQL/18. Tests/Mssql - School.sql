CREATE DATABASE School
USE School

CREATE TABLE Students 
(
	Id INT PRIMARY KEY IDENTITY,
	FirstName nvarchar(30) NOT NULL,
	MiddleName nvarchar(25),
	LastName nvarchar(30) NOT NULL,
	Age INT CHECK (Age BETWEEN 5 and 100 ) NOT NULL,
	[Address ] nvarchar(50),
	Phone nvarchar(10)
)

CREATE Table Subjects
(
	Id INT Primary key Identity,
	[Name] nvarchar(20) NOT NULL,
	Lessons INT not null CHECK (Lessons >0 )
)

Create Table StudentsSubjects
(
	Id INT PRIMARY KEY IDENTITY,
	StudentId INT NOT NULL,
	SubjectId INT NOT NULL,
	Grade DECIMAL(15,2) NOT NULL  CHECK (Grade >= 2 AND Grade <= 6),
	CONSTRAINT FK_StudentsSubjects_Students FOREIGN KEY (StudentId) REFERENCES Students (Id),
	CONSTRAINT FK_StudentsSubjects_Subjects FOREIGN KEY (SubjectId) REFERENCES Subjects (Id),
)

CREATE Table Exams
(
	Id INT Primary Key Identity,
	[Date] date,
	SubjectId INT FOREIGN KEY REFERENCES Subjects(Id) NOT NUll
)
Create Table StudentsExams
(
	StudentId INT NOT NULL,
	ExamId INT NOT NULL,
	Grade DECIMAL(15,2) NOT NULL CHECK (Grade >= 2 AND Grade <= 6),
	CONSTRAINT PK_StudentsExams PRIMARY KEY (StudentId, ExamId),
	CONSTRAINT FK_StudentsExams_Students FOREIGN KEY (StudentId) REFERENCES Students (Id),
	CONSTRAINT FK_StudentsExams_Exams FOREIGN KEY (ExamId) REFERENCES Exams (Id)
)

Create Table Teachers
(
	Id INT Primary Key Identity,
	FirstName nvarchar(20) NOT NULL,
	LastName nvarchar(20) NOT NULL,
	[Address] nvarchar(20) NOT NULL,
	Phone nvarchar(10),
	SubjectId INT FOREIGN KEY REFERENCES Subjects(Id) NOT NUll
)

Create Table StudentsTeachers
(
	StudentId INT FOREIGN KEY REFERENCES Students(Id) NOT NUll,
	TeacherId INT FOREIGN KEY REFERENCES Teachers(Id) NOT NUll,
	CONSTRAINT PK_StudentsTeachers PRIMARY KEY (StudentId, TeacherId)
)

INSERT INTO Teachers (FirstName, LastName, [Address], Phone, SubjectId) VALUES
	('Ruthanne', 'Bamb','84948 Mesta Junction','3105500146',6),
	('Gerrard', 'Lowin','370 Talisman Plaza','3324874824',2),
	('Merrile', 'Lambdin','81 Dahle Plaza','4373065154',5),
	('Bert', 'Ivie','2 Gateway Circle','4409584510',4)

INSERT INTO Subjects ([Name], Lessons) VALUES
	('Geometry', 12),
	('Health', 10),
	('Drama', 7),
	('Sports', 9)

UPDATE StudentsSubjects
SET Grade=6 WHERE (SubjectId=1 OR SubjectId=2) AND Grade>=5.50

DELETE FROM StudentsTeachers WHERE TeacherID IN (SELECT Id FROM Teachers WHERE Phone LIKE '%72%')
DELETE FROM Teachers WHERE Phone LIKE '%72%'

SELECT FirstName, LastName, Age FROM Students
WHERE Age>=12
ORDER BY FirstName, LastName

SELECT FirstName, LastName, COUNT(st.TeacherID) AS TeachersCount
FROM Students AS s
JOIN StudentsTeachers AS st ON s.Id=st.StudentId

GROUP BY FirstName, LastName

SELECT FirstName+' '+LastName AS [Full Name]
FROM Students AS s
LEFT JOIN StudentsExams AS se ON s.Id=se.StudentId
WHERE ExamId IS NULL
ORDER BY [Full Name]

SELECT TOP(10)
	FirstName AS [First Name], 
	LastName AS [Last Name], 
	CONVERT(DECIMAL(10,2),(AVG(se.Grade))) AS Grade
FROM Students AS s
JOIN StudentsExams AS se ON se.StudentId=s.Id
GROUP BY FirstName, LastName
ORDER BY Grade DESC, FirstName ASC, LastName ASC

SELECT FirstName+ISNULL(' '+MiddleName, '')+' '+LastName AS [Full Name] 
FROM Students AS s
LEFT JOIN StudentsSubjects AS ss ON ss.StudentId=s.Id
WHERE SubjectId IS NULL
ORDER BY [Full Name]

SELECT [Name], AVG(ss.Grade) AS AverageGrade
FROM Subjects AS sub
JOIN StudentsSubjects AS ss ON ss.SubjectId=sub.Id
GROUP BY [Name], SubjectId
ORDER BY SubjectId

CREATE FUNCTION udf_ExamGradesToUpdate
	(
		@StudentId AS INT,
		@Grade AS DECIMAL(15,2)
	)
RETURNS nvarchar(100)
AS
BEGIN
	DECLARE @CheckForStudentID INT;
	DECLARE @NumberOfGrades INT;
	DECLARE @StudentFirstName nvarchar(50);
	IF (@Grade > 6) 
	RETURN 'Grade cannot be above 6.00!';
	
	SET @CheckForStudentID=(SELECT Id FROM Students WHERE Id=@StudentId);
	IF (@CheckForStudentID IS NULL) 
	RETURN 'The student with provided id does not exist in the school!'

	SET @StudentFirstName=(SELECT FirstName From Students WHERE Id=@StudentId);
	
	SET @NumberOfGrades=
	(SELECT COUNT(StudentId) FROM StudentsExams 
		WHERE StudentId=@StudentId AND Grade>=@Grade AND @Grade<=@Grade+0.5)

	RETURN 'You have to update '+CAST(@NumberOfGrades as VARCHAR(5))+' grades for the student '+@StudentFirstName;	
END

SELECT dbo.udf_ExamGradesToUpdate(12, 5.50)


CREATE PROCEDURE usp_ExcludeFromSchool @StudentId INT
AS
	DECLARE @CheckForStudentID INT;
	SET @CheckForStudentID=(SELECT Id FROM Students WHERE Id=@StudentId);
	IF (@CheckForStudentID IS NULL) 
	THROW 50001, 'This school has no student with the provided id!', 1;  

	DELETE FROM StudentsExams WHERE StudentId=@StudentId;
	DELETE FROM StudentsSubjects WHERE StudentId=@StudentId;
	DELETE FROM StudentsTeachers WHERE StudentId=@StudentId;
	DELETE FROM Students WHERE Id=@StudentId;
	
GO
EXEC usp_ExcludeFromSchool 1

SELECT COUNT(*) FROM Students