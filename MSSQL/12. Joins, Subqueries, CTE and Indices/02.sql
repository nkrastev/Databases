SELECT TOP(50) e.FirstName, e.LastName, t.[Name] AS Town, a.AddressText
FROM Employees AS e
LEFT JOIN Addresses AS a ON e.AddressID=a.AddressID
LEFT JOIN Towns AS t ON t.TownID=a.TownID
ORDER BY e.FirstName, e.LastName