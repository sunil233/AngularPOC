
CREATE PROC [dbo].[GetDescriptionbyTimeSheetMasterID] @TimeSheetMasterID INT
	,@ProjectID INT
AS
BEGIN
	SELECT Description
	FROM DescriptionTB
	WHERE TimeSheetMasterID = @TimeSheetMasterID
		AND ProjectID = @ProjectID
END
