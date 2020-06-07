SELECT TOP(5) Country,
		ISNULL([Highest Peak Name], '(no highest peak)'),
		ISNULL([Highest Peak Elevation], '0'),
		Mountain
FROM
	(
	SELECT *, 
		DENSE_RANK() OVER
		(PARTITION BY [Country] ORDER BY [Highest Peak Elevation] DESC) AS PeakRank
		FROM
		(SELECT 
			c.CountryName AS Country,	
			p.PeakName AS [Highest Peak Name],
			p.Elevation AS [Highest Peak Elevation],
			ISNULL(m.MountainRange, 'no mountain') AS [Mountain]	
		FROM Countries AS c
		LEFT JOIN MountainsCountries AS mc ON c.CountryCode=mc.CountryCode
		LEFT JOIN Mountains AS m ON m.Id=mc.MountainId
		LEFT JOIN Peaks AS p ON p.MountainID=mc.MountainId
		) 
		AS FullQuery
	) AS FullQuery2
WHERE PeakRank=1
ORDER BY Country, [Highest Peak Name]