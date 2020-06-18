CREATE DATABASE Bitbucket
USE Bitbucket
CREATE TABLE Users
(
	Id INT Primary Key Identity,
	Username varchar(30) NOT NULL,
	[Password] varchar(30) NOT NULL,
	Email varchar(30) NOT NULL
)
CREATE TABLE Commits
(
	Id INT Primary Key Identity,
	[Message] varchar(255) NOT NULL,
	IssueId INT NOT NULL,
	RepositoryId INT NOT NULL,
	ContributorId INT REFERENCES Users(Id) NOT NULL
)

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