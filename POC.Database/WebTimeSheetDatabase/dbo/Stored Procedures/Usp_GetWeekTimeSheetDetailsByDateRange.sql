
CREATE PROC [dbo].[Usp_GetWeekTimeSheetDetailsByDateRange] @FromDate DATE = NULL
	,@ToDate DATE = NULL
	,@UserID INT
AS
BEGIN
	DECLARE @TimeSheetMasterID INT

	SELECT @TimeSheetMasterID = [TimeSheetMasterID]
	FROM [WebTimeSheetDB].[dbo].[TimeSheetMaster]
	WHERE FromDate >= @FromDate
		AND ToDate <= @ToDate
		AND UserID = @UserID

	SELECT TM.DaysofWeek
		,TM.Hours
		,TM.Period
		,PM.ProjectName
		,PM.ProjectCode
		,PM.ProjectID
		,TM.TimeSheetStatus
		,TimeSheetMasterID
		,TimeSheetID
	FROM TimeSheetDetails TM
	INNER JOIN ProjectMaster PM ON TM.ProjectID = PM.ProjectID
	WHERE TimeSheetMasterID = @TimeSheetMasterID
END
