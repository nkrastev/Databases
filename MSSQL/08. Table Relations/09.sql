Select MountainRange,PeakName,Elevation From Mountains
Join Peaks ON Mountains.Id=Peaks.MountainId
Where MountainRange LIKE 'Rila'
Order By Elevation Desc