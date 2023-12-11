DECLARE @SumOfIntegers INT
DECLARE @MedianOfDoubles FLOAT

SELECT @SumOfIntegers = SUM(EvenInteger)
FROM YourTableName; 

;WITH OrderedDoubles AS
(
    SELECT RandomDouble,
           ROW_NUMBER() OVER (ORDER BY RandomDouble) AS RowAsc,
           ROW_NUMBER() OVER (ORDER BY RandomDouble DESC) AS RowDesc
    FROM YourTableName 
    WHERE RandomDouble IS NOT NULL AND RandomDouble <> 0
)
SELECT @MedianOfDoubles =
    CASE
        WHEN COUNT(*) % 2 = 1 THEN MAX(RandomDouble)
        ELSE (MAX(CASE WHEN RowAsc = COUNT(*) / 2 THEN RandomDouble END) +
              MAX(CASE WHEN RowDesc = COUNT(*) / 2 THEN RandomDouble END)) / 2.0
    END
FROM OrderedDoubles;

SELECT @SumOfIntegers AS SumOfIntegers,
       @MedianOfDoubles AS MedianOfDoubles;
