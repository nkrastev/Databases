CREATE VIEW V_EmployeesHiredAfter2000 AS
(
SELECT [FirstName],[LastName]
FROM Employees
WHERE HireDate>'2000-12-12'
)

--Select * FROM V_EmployeesHiredAfter2000