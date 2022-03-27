--Simple Roll Back database from Bak File

ALTER DATABASE YourDatabase
SET SINGLE_USER
WITH ROLLBACK IMMEDIATE
GO

USE [master]
GO
RESTORE DATABASE YourDatabase FROM DISK = 'FullPathToBAKFile.bak' WITH 
MOVE 'Charterio' TO 'FullPathToMDFFile.mdf',
MOVE 'Charterio_log' TO 'FullPathToLDFFile.ldf', REPLACE
