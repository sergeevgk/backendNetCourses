SELECT  UnitName,
		TankIdCount
FROM 
	(SELECT Unit.Name as UnitName,
			Count(Tank.Id) as TankIdCount
	FROM Unit
	INNER JOIN Tank ON Unit.Id=Tank.UnitId
	WHERE Tank.Volume > 1000
	GROUP BY Unit.Name)	AS UnitTankVolume
WHERE TankIdCount >= 1
