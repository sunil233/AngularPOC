
CREATE PROC [dbo].[Usp_ChangeTimesheetStatus] @Status INT
	,@TimeSheetMasterID INT
	,@Comment VARCHAR(100)
AS
BEGIN
	UPDATE dbo.TimeSheetAuditTB
	SET STATUS = @Status
		,Comment = @Comment
		,ProcessedDate = getdate()
	WHERE TimeSheetMasterID = @TimeSheetMasterID

	UPDATE dbo.TimeSheetMaster
	SET TimeSheetStatus = @Status
	WHERE TimeSheetMasterID = @TimeSheetMasterID

	UPDATE dbo.TimeSheetDetails
	SET TimeSheetStatus = @Status
	WHERE TimeSheetMasterID = @TimeSheetMasterID
END
