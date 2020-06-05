SELECT [FirstName],DepartmentID, HireDate
FROM Employees
WHERE 
	DepartmentID IN (3,10) AND
	HireDate BETWEEN '1995-01-01' AND '2005-12-12'
	