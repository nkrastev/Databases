SELECT *  INTO TempTable FROM Employees WHERE Salary > 30000 

DELETE  FROM TempTable WHERE ManagerID = 42

UPDATE TempTable
SET Salary += 5000
WHERE DepartmentID = 1

SELECT DepartmentID, AVG(Salary) AS AverageSalary FROM TempTable GROUP BY DepartmentID

/*WITH CTE_TempTable (DepartmentID, ManagerID, Salary) AS 
(
	SELECT 
		DepartmentID, 
		ManagerID, 
		CASE 
			WHEN DepartmentID=1 THEN (Salary+5000)
			ELSE Salary
		END AS Salary
	FROM Employees
	WHERE 
		Salary>30000 AND ManagerID<>42
)
SELECT 
	DepartmentID,
	AVG(Salary) AS AverageSalary
FROM CTE_TempTable
GROUP BY DepartmentID*/


