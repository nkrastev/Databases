SELECT 
	mc.CountryCode, 
	COUNT(m.MountainRange)
FROM MountainsCountries AS mc
LEFT JOIN Mountains AS m ON mc.MountainId=m.Id
WHERE mc.CountryCode IN ('BG','US','RU')
GROUP BY mc.CountryCode

