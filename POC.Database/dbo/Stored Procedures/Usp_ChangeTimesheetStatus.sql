
CREATE PROC dbo.[Usp_ChangeTimesheetStatus] @Status INT
	,@TimeSheetMasterID INT
	,@Comment VARCHAR(100)
AS
BEGIN
	UPDATE TimeSheetAuditTB
	SET STATUS = @Status
		,Comment = @Comment
		,ProcessedDate = getdate()
	WHERE TimeSheetMasterID = @TimeSheetMasterID

	UPDATE TimeSheetMaster
	SET TimeSheetStatus = @Status
	WHERE TimeSheetMasterID = @TimeSheetMasterID

	UPDATE TimeSheetDetails
	SET TimeSheetStatus = @Status
	WHERE TimeSheetMasterID = @TimeSheetMasterID
END
