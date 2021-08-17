
/****** CRUD Task 20 Last 7 Hired Employees ******/

SELECT TOP(7) FirstName, LastName, HireDate
FROM Employees 
ORDER BY HireDate DESC
