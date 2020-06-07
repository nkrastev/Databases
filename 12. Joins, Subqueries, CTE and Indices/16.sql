
	SELECT COUNT(*) AS [Count]
	FROM Countries AS c
	LEFT JOIN MountainsCountries AS mc ON c.CountryCode=mc.CountryCode
	WHERE MountainId IS NULL
