Create Table Subjects(
	SubjectID INT Primary Key,
	SubjectName Varchar(50)
)
Create Table Majors(
	MajorID INT Primary Key,
	[Name] Varchar(50)
)
Create Table Students
(
	StudentID INT Primary Key,
	StudentNumber INT NOT NULL,
	StudentName Varchar(50) NOT NULL,
	MajorID INT NOT NULL Foreign Key References Majors(MajorID)
)
Create Table Payments
(
	PaymentID INT Primary Key,
	PaymentDate DATE,
	PaymentAmount DECIMAL (7,2),
	StudentID INT NOT NULL Foreign Key References Students(StudentID)
)
Create Table Agenda
(
	StudentID INT NOT NULL Foreign Key References Students(StudentID),
	SubjectID INT NOT NULL Foreign Key References Subjects(SubjectID),
	Primary Key (StudentID, SubjectID)
)