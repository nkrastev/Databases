
/****** CRUD Task 17 Create View with replace ******/

CREATE VIEW V_EmployeeNameJobTitle AS

	SELECT 
		FirstName +' '+ISNULL(MiddleName,'')+' '+LastName AS [Full Name],
		JobTitle AS [Job Title]
	FROM [Employees]
