WITH CTE_SumDeposits (DepositAmount) AS 
(
    SELECT	
	w.DepositAmount-(LEAD(w.DepositAmount) OVER(ORDER BY w.Id)) AS [Difference]
	FROM WizzardDeposits AS w
)
SELECT 
	SUM(DepositAmount) AS SumDifference
FROM CTE_SumDeposits



