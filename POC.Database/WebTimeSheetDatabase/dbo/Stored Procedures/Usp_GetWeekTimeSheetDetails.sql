
CREATE PROC [dbo].[Usp_GetWeekTimeSheetDetails] @TimeSheetMasterID INT
AS
BEGIN
	SELECT TM.DaysofWeek
		,TM.Hours
		,TM.Period
		,PM.ProjectName
		,TM.CreatedOn
	FROM TimeSheetDetails TM
	INNER JOIN ProjectMaster PM ON TM.ProjectID = PM.ProjectID
	WHERE TimeSheetMasterID = @TimeSheetMasterID
END
