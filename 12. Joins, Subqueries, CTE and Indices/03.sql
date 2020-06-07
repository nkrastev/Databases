SELECT TOP(50) e.EmployeeID, e.FirstName, e.LastName, d.[Name] AS DepartmentName
FROM Employees AS e
LEFT JOIN Departments AS d ON e.DepartmentID=d.DepartmentID
WHERE d.[Name]='Sales'
ORDER BY e.EmployeeID