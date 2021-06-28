/****** Script for SelectTopNRows command from SSMS  ******/
SELECT  Factory.Name,
		Sum(Volume) as SumVolume,
		Sum(MaxVolume) as SumMaxVolume,
		Count(Tank.Id) as TankCount
  FROM Unit
  INNER JOIN Factory ON Unit.FactoryId=Factory.Id
  INNER JOIN Tank ON Unit.Id=Tank.UnitId
  GROUP BY Factory.Name
