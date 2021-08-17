SELECT e.FirstName, e.LastName, e.HireDate, d.[Name] AS DeptName		
FROM Employees AS e
LEFT JOIN Departments AS d ON e.DepartmentID=d.DepartmentID

WHERE HireDate>'1999-01-01' AND (d.[Name]='Sales' OR d.Name='Finance')

ORDER BY HireDate