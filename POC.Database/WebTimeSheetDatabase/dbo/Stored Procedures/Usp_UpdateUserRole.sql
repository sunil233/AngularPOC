
CREATE PROC [dbo].[Usp_UpdateUserRole] @RegistrationID INT
AS
BEGIN
	DELETE
	FROM AssignedRoles
	WHERE RegistrationID = @RegistrationID
END
