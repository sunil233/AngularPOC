
CREATE PROC [dbo].[Usp_DeleteExpenseandDocuments] @ExpenseID INT
	,@UserID INT
AS
BEGIN
	DELETE
	FROM Expense
	WHERE ExpenseID = @ExpenseID
		AND UserID = @UserID

	IF EXISTS (
			SELECT DocumentID
			FROM dbo.Documents
			WHERE ExpenseID = @ExpenseID
				AND UserID = @UserID
			)
	BEGIN
		DELETE
		FROM Documents
		WHERE ExpenseID = @ExpenseID
			AND UserID = @UserID
	END

	IF EXISTS (
			SELECT ApprovaExpenselLogID
			FROM dbo.ExpenseAuditTB
			WHERE ExpenseID = @ExpenseID
				AND UserID = @UserID
			)
	BEGIN
		DELETE
		FROM ExpenseAuditTB
		WHERE ExpenseID = @ExpenseID
			AND UserID = @UserID
	END
END
