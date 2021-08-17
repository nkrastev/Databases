CREATE TABLE Teachers(
	TeacherID INT Primary Key,
	[Name] NVARCHAR(50) NOT NULL,
	ManagerID INT Foreign Key (ManagerID) References Teachers(TeacherID)
)

INSERT INTO Teachers(TeacherID,[Name],ManagerID)
VALUES
(101,'John',NULL),
(102,'Maya',106),
(103,'Silvia',106),
(104,'Ted',105),
(105,'Mark',101),
(106,'Greta',101)