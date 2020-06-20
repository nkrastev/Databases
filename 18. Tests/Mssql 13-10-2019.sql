CREATE DATABASE Bitbucket
USE Bitbucket

CREATE TABLE Users
(
	Id INT Primary Key Identity,
	Username varchar(30) NOT NULL,
	[Password] varchar(30) NOT NULL,
	Email varchar(30) NOT NULL
)
Create Table Repositories
(
	Id Int Primary Key Identity,
	[Name] varchar(50) NOT NULL
)
Create Table RepositoriesContributors
(
	RepositoryId INT References Repositories(Id) NOT NULL,
	ContributorId INT References Users(Id) NOT NULL,
	PRIMARY KEY (RepositoryId, ContributorId)
)
Create Table Issues
(
	Id INT Primary Key Identity,
	Title varchar(255) NOT NULL,
	IssueStatus char(6) NOT NULL,
	RepositoryId INT References Repositories(Id) NOT NULL,
	AssigneeId INT References Users(Id) NOT NULL
)
CREATE TABLE Commits
(
	Id INT Primary Key Identity,
	[Message] varchar(255) NOT NULL,
	IssueId INT REFERENCES Issues(Id),
	RepositoryId INT references Repositories(Id) NOT NULL,
	ContributorId INT REFERENCES Users(Id) NOT NULL
)
Create Table Files
(
	Id INT Primary Key Identity,
	[Name] varchar(100) NOT NULL,
	Size Decimal(15,2) NOT NULL,
	ParentId INT REFERENCEs Files(Id),
	CommitId INT References Commits(Id) NOT NULL
)
--2.	Insert
INSERT INTO Files ([Name], Size, ParentId, CommitId) VALUES
('Trade.idk',2598.0,1,1),
('menu.net',9238.31,2,2),
('Administrate.soshy',1246.93,3,3),
('Controller.php',7353.15,4,4),
('Find.java',9957.86,5,5),
('Controller.json',14034.87,3,6),
('Operate.xix',7662.92,7,7)

INSERT INTO Issues(Title, IssueStatus, RepositoryId, AssigneeId) VALUES
('Critical Problem with HomeController.cs file', 'open', 1,4),
('Typo fix in Judge.html', 'open', 4,3),
('Implement documentation for UsersService.cs', 'closed', 8,2),
('Unreachable code in Index.cs', 'open', 9,8)

--03. Update
UPDATE Issues SET IssueStatus='closed' WHERE AssigneeId=6

--04. Delete
DELETE FROM Issues WHERE RepositoryId=3
DELETE FROM RepositoriesContributors WHERE RepositoryId=3

--05. Commits
SELECT Id, [Message], RepositoryId, ContributorId From Commits
ORDER BY Id, [Message], RepositoryId, ContributorId

--06. Heavy HTML
Select Id, [Name], Size
From Files
Where Size>1000 AND [Name] LIKE '%html%'
ORDER BY Size DESC, Id, [Name]

--07. Issues and Users
SELECT 
	i.Id, 
	u.Username+' : '+i.Title AS IssueAssignee
FROM Issues AS i
JOIN Users AS u ON u.Id=i.AssigneeId
ORDER BY i.Id DESC, IssueAssignee 

--08. Non-Directory Files
Select 
	main.Id,
	main.[Name],
	--slave.Id,
	CAST(main.Size AS varchar)+'KB' AS Size
From Files AS main
LEFT JOIN Files AS slave ON main.Id=slave.ParentId
WHERE slave.Id Is NULL
ORDER BY main.Id, main.[Name], main.Size DESC

--09. Most Contributed Repositories
SELECT TOP(5) 
	r.Id, 
	r.[Name], 
	COUNT(c.RepositoryId) AS [Commits] 
FROM Repositories AS r
JOIN Commits AS c ON c.RepositoryId = r.Id
LEFT JOIN RepositoriesContributors AS rc
ON rc.RepositoryId = r.Id
GROUP BY r.Id, r.[Name]
ORDER BY [Commits] DESC, r.Id, r.[Name]

--10. User and Files
Select
	u.Username,
	AVG(f.Size) AS Size
From Commits AS c
Join Users AS u ON u.Id=c.ContributorId
Join Files AS f ON f.CommitId=c.Id
GROUP BY u.Username
ORDER BY Size DESC, u.Username

--11. User Total Commits
CREATE OR ALTER FUNCTION udf_UserTotalCommits
	(
		@username varchar(30)
	) 
RETURNS INT
AS
BEGIN
    DECLARE @UserId INT;
	DECLARE @TotalCommits INT;
	
	SET @UserId=(SELECT Id FROM Users WHERE Username LIKE @username);
	SET @TotalCommits=(SELECT Count(ContributorId) From Commits);
    
RETURN @TotalCommits
END

SELECT dbo.udf_UserTotalCommits('UnderSinduxrein')
SELECT Count(2) From Commits

--12.	 Find by Extensions
Create Or Alter Procedure usp_FindByExtension(@extension varchar(50))
AS		
	SELECT 
		Id,
		[Name],
		CAST(Size AS varchar)+'KB' AS Size
	From Files 
	WHERE [Name] LIKE '%.'+@extension ORDER BY Id, [Name], Size DESC;				
GO
EXEC usp_FindByExtension 'txt'
