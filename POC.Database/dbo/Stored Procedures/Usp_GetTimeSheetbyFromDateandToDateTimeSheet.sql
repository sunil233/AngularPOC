
CREATE PROC [dbo].[Usp_GetTimeSheetbyFromDateandToDateTimeSheet] @FromDate DATE = NULL
	,@ToDate DATE = NULL
AS
BEGIN
	SELECT [TimeSheetMasterID]
	FROM [dbo].[TimeSheetMaster]
	WHERE FromDate BETWEEN @FromDate
			AND @ToDate
END
