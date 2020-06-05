
/****** Script for SelectTopNRows command from SSMS  ******/

SELECT FirstName, LastName, JobTitle 
FROM [Employees]
WHERE Salary>=20000 AND Salary<=30000
