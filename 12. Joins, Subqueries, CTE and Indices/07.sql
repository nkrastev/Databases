SELECT TOP(5) e.EmployeeId, e.FirstName, p.Name AS ProjectName
FROM Employees AS e

LEFT JOIN EmployeesProjects AS ep ON e.EmployeeID=ep.EmployeeID
LEFT JOIN Projects AS p ON p.ProjectID=ep.ProjectID

WHERE p.EndDate IS NULL AND StartDate>'2002-08-13'

ORDER BY e.EmployeeID