CREATE PROC [dbo].[Usp_GetTimeSheetDetailsByID] @TimeSheetMasterID INT
AS
BEGIN
	
	SELECT TM.DaysofWeek
		,TM.Hours
		,TM.Period
		,PM.ProjectName
		,PM.ProjectCode
		,PM.ProjectID
		,TM.TimeSheetStatus
		,TimeSheetMasterID
		,TimeSheetID
		,T.TaskID
		,T.Taskname
	   ,(Row_number()OVER(ORDER BY TM.CreatedOn) - 1 ) / 7 + 1 AS "RowNo"    
	FROM TimeSheetDetails TM
	LEFT JOIN ProjectMaster PM ON TM.ProjectID = PM.ProjectID
	LEFT JOIN  Task T ON TM.TaskID=T.TaskID
	WHERE TimeSheetMasterID = @TimeSheetMasterID	

END

GO