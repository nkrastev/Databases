SELECT 
	SUBSTRING(FirstName, 1, 1) AS FirstLetter
	
FROM WizzardDeposits
WHERE DepositGroup='Troll Chest'

GROUP BY SUBSTRING(FirstName, 1, 1)
ORDER BY FirstLetter

