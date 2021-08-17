WITH
CTE
AS
(
    SELECT
        ContinentCode, CurrencyCode, COUNT(*) AS UseCount
    FROM Countries
    GROUP BY ContinentCode, CurrencyCode
)
,CTE_rn
AS
(
    SELECT
        ContinentCode, CurrencyCode, UseCount,
        ROW_NUMBER() OVER (PARTITION BY ContinentCode ORDER BY UseCount DESC, CurrencyCode) AS rn
    FROM CTE
    WHERE UseCount > 1
)
SELECT
    ContinentCode, CurrencyCode, UseCount
FROM CTE_rn
WHERE rn = 1