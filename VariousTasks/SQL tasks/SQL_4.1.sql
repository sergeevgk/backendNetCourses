/****** Script for SelectTopNRows command from SSMS  ******/
SELECT Unit.Id, Unit.Name, Unit.FactoryId, Factory.Name
  FROM Unit
  INNER JOIN Factory ON Unit.FactoryId=Factory.Id