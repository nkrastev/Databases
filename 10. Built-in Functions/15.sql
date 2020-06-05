SELECT 
	Username,
	--Email,	
	--CHARINDEX('@',Email) AS PositionOfMailSign,
	RIGHT(Email,(LEN(Email)-CHARINDEX('@',Email))) AS [Email Provider]
FROM Users
ORDER BY [Email Provider],[Username]
