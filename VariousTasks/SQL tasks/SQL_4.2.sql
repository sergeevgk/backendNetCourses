/****** Script for SelectTopNRows command from SSMS  ******/
SELECT Unit.Name,
		Factory.Name,
		Factory.Description,
		TankUnitTable.SumVolume,
		TankUnitTable.SumMaxVolume,
		TankUnitTable.TankCount
  FROM Unit
  INNER JOIN Factory ON Unit.FactoryId=Factory.Id
  INNER JOIN 
	(SELECT Unit.Id as Id,
			Sum(Tank.Volume) as SumVolume, 
			Sum(Tank.MaxVolume) as SumMaxVolume, 
			Count(Tank.Id) as TankCount
	FROM Unit
	INNER JOIN Tank ON Unit.Id=Tank.UnitId
	GROUP BY Unit.Id)
	AS TankUnitTable
	ON Unit.Id=TankUnitTable.Id