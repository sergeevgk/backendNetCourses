WITH Factory_CTE
     AS (SELECT ROW_NUMBER() OVER (PARTITION BY [dbo].[Factory].Name, [dbo].[Factory].Description
                                      ORDER BY [dbo].[Factory].Id ASC) AS RowNumber
         FROM [dbo].[Factory])
DELETE FROM Factory_CTE
WHERE  RowNumber > 1;
