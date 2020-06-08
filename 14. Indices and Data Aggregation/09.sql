SELECT
	CASE 
				when Age BETWEEN 1 AND 10 then '[1-10]'
				when Age BETWEEN 11 AND 20 then '[11-20]'
				when Age BETWEEN 21 AND 30 then '[21-30]'
				when Age BETWEEN 31 AND 40 then '[31-40]'
				when Age BETWEEN 41 AND 50 then '[41-50]'
				when Age BETWEEN 51 AND 60 then '[51-60]'            
		   ELSE '[61+]' END AS AgeGroup,
	COUNT (Id) AS WizardCount
FROM WizzardDeposits

GROUP BY (CASE 
				when Age BETWEEN 1 AND 10 then '[1-10]'
				when Age BETWEEN 11 AND 20 then '[11-20]'
				when Age BETWEEN 21 AND 30 then '[21-30]'
				when Age BETWEEN 31 AND 40 then '[31-40]'
				when Age BETWEEN 41 AND 50 then '[41-50]'
				when Age BETWEEN 51 AND 60 then '[51-60]'            
		   else '[61+]' END) 

