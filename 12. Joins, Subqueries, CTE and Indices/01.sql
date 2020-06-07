SELECT TOP(5) e.EmployeeID, e.JobTitle, a.AddressID, a.AddressText
FROM Employees AS e
LEFT JOIN Addresses AS a ON e.AddressID=a.AddressID
ORDER BY a.AddressID