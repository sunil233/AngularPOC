using System;
using System.Collections.Generic;
using POC.ViewModels;

namespace POC.Repository.Interface
{
    public interface IExpenseExportRepository
    {
        List<ExpenseModelView> GetReportofExpense(DateTime? FromDate, DateTime? ToDate, int UserID);
        List<ExpenseModelView> GetAllReportofExpense(DateTime? FromDate, DateTime? ToDate);
    }
}
