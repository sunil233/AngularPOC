using System;
using System.Linq;
using POC.Models;
using POC.ViewModels;

namespace POC.Repository.Interface
{
    public interface IExpenseRepository
    {
        int AddExpense(ExpenseModel ExpenseModel);
        bool CheckIsDateAlreadyUsed(DateTime? FromDate, DateTime? ToDate, int UserID);
        IQueryable<ExpenseModelView> ShowExpense(string sortColumn, string sortColumnDir, string Search, int UserID);
        bool IsExpenseSubmitted(int ExpenseID, int UserID);
        int DeleteExpensetByExpenseID(int ExpenseID ,int UserID);
        IQueryable<ExpenseModelView> ShowAllExpense(string sortColumn, string sortColumnDir, string Search, int UserID);
        ExpenseModelView ExpenseDetailsbyExpenseID(int ExpenseID);
        bool UpdateExpenseStatus(ExpenseApprovalModel ExpenseApprovalModel, int ExpenseStatus);
        void InsertExpenseAuditLog(ExpenseAuditTB expenseaudittb);
        DisplayViewModel GetExpenseAuditCountByAdminID(string AdminID);
        IQueryable<ExpenseModelView> ShowAllSubmittedExpense(string sortColumn, string sortColumnDir, string Search, int UserID);
        IQueryable<ExpenseModelView> ShowAllRejectedExpense(string sortColumn, string sortColumnDir, string Search, int UserID);
        IQueryable<ExpenseModelView> ShowAllApprovedExpense(string sortColumn, string sortColumnDir, string Search, int UserID);
        DisplayViewModel GetExpenseAuditCountByUserID(string UserID);
        bool UpdateExpenseAuditStatus(int ExpenseID, string Comment, int Status);
        bool IsExpenseALreadyProcessed(int ExpenseID);
        IQueryable<ExpenseModelView> ShowExpenseStatus(string sortColumn, string sortColumnDir, string Search, int UserID, int ExpenseStatus);
    }
}
