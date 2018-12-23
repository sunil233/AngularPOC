
CREATE PROC [dbo].[Usp_GetTimeSheetMasterIDTimeSheet] @FromDate DATE = NULL
	,@ToDate DATE = NULL
	,@UserID INT
AS
BEGIN
	SELECT [TimeSheetMasterID]
	FROM [WebTimeSheetDB].[dbo].[TimeSheetMaster]
	WHERE FromDate BETWEEN @FromDate
			AND @ToDate
		AND UserID = @UserID
END
