SELECT TOP(50)
	e.EmployeeID, 
	e.FirstName+' '+e.LastName AS EmployeeName,
	--e.ManagerID, 
	m.FirstName+' '+m.LastName AS ManagerName,
	d.Name AS DepartmentName

FROM Employees AS e
LEFT JOIN Employees AS m ON e.ManagerID=m.EmployeeID
LEFT JOIN Departments AS d ON e.DepartmentID=d.DepartmentID

ORDER BY EmployeeID