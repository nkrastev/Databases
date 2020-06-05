
/****** CRUD Task 19 Find First 10 Started Projects ******/

SELECT TOP(10) *
FROM [Projects] 
ORDER BY StartDate, Name
