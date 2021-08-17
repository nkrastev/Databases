SELECT  
	e.EmployeeId, 
	e.FirstName, 	
	--IIF(p.StartDate>='2005-01-01', NULL, p.Name) AS ProjectName	
	CASE
		WHEN p.StartDate>='2005-01-01' THEN NULL
		ELSE p.Name
	END AS ProjectName

FROM Employees AS e

LEFT JOIN EmployeesProjects AS ep ON e.EmployeeID=ep.EmployeeID
LEFT JOIN Projects AS p ON p.ProjectID=ep.ProjectID

WHERE e.EmployeeID=24
