using System;
using System.Collections.Generic;
using System.Linq;
using POC.Repository.Interface;
using POC.ViewModels;

namespace POC.Repository.Implementation
{
    public class ExpenseExportRepository : IExpenseExportRepository
    {
        public List<ExpenseModelView> GetReportofExpense(DateTime? FromDate, DateTime? ToDate, int UserID)
        {
            try
            {
                using (var db = new DatabaseContext())
                {

                    var queryresult = (from ex in db.ExpenseModel
                                       join Reg in db.Registration on ex.UserID equals Reg.RegistrationID
                                       join PM in db.ProjectMaster on ex.ProjectID equals PM.ProjectID
                                       join AR in db.AssignedRoles on Reg.RegistrationID equals AR.RegistrationID
                                       join EA in db.ExpenseAuditTB on ex.ExpenseID equals EA.ExpenseID
                                       where ex.FromDate >= FromDate && ex.FromDate <= @ToDate && AR.AssignToAdmin == UserID && EA.Status == 2
                                       select new
                                       {
                                           ProjectName = PM.ProjectName,
                                           PurposeorReason = ex.PurposeorReason,
                                           FirstName= Reg.FirstName,
                                           MiddleName = Reg.MiddleName,
                                           LastName = Reg.LastName,                                         
                                           Status = ex.ExpenseStatus == 1 ? "Submitted" : ex.ExpenseStatus == 2 ? "Approved" : ex.ExpenseStatus == 3 ? "Rejected" : null,
                                           Comment = EA.Comment,
                                           FromDate = ex.FromDate,
                                           ToDate = ex.ToDate,
                                           VoucherID = ex.VoucherID,
                                           HotelBills = ex.HotelBills,
                                           TravelBills = ex.TravelBills,
                                           MealsBills = ex.MealsBills,
                                           LandLineBills = ex.LandLineBills,
                                           TransportBills = ex.TransportBills,
                                           MobileBills = ex.MobileBills,
                                           Miscellaneous = ex.Miscellaneous,
                                           TotalAmount = ex.TotalAmount,
                                           CreatedOn = ex.CreatedOn
                                       }).ToList().Select(item => new ExpenseModelView
                                       {
                                           ProjectName = item.ProjectName,
                                           PurposeorReason = item.PurposeorReason,
                                           FirstName = item.FirstName,
                                           MiddleName = item.MiddleName,
                                           LastName = item.LastName,
                                           Status = item.Status,
                                           Comment = item.Comment,
                                           FromDate = Convert.ToString(item.FromDate),
                                           ToDate = Convert.ToString(item.ToDate),
                                           VoucherID = item.VoucherID,
                                           HotelBills = item.HotelBills,
                                           TravelBills = item.TravelBills,
                                           MealsBills = item.MealsBills,
                                           LandLineBills = item.LandLineBills,
                                           TransportBills = item.TransportBills,
                                           MobileBills = item.MobileBills,
                                           Miscellaneous = item.Miscellaneous,
                                           TotalAmount = item.TotalAmount,
                                           CreatedOn = Convert.ToString(item.CreatedOn)
                                       }).ToList();

                    return queryresult;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ExpenseModelView> GetAllReportofExpense(DateTime? FromDate, DateTime? ToDate)
        {
            try
            {
                using (var db = new DatabaseContext())
                {

                    var queryresult = (from ex in db.ExpenseModel
                                       join Reg in db.Registration on ex.UserID equals Reg.RegistrationID
                                       join PM in db.ProjectMaster on ex.ProjectID equals PM.ProjectID
                                       where ex.FromDate >= FromDate && ex.FromDate <= @ToDate
                                       select new
                                       {
                                           ProjectName = PM.ProjectName,
                                           PurposeorReason = ex.PurposeorReason,
                                           FirstName = Reg.FirstName,
                                           MiddleName = Reg.MiddleName,
                                           LastName = Reg.LastName,
                                           Status = ex.ExpenseStatus == 1 ? "Submitted" : ex.ExpenseStatus == 2 ? "Approved" : ex.ExpenseStatus == 3 ? "Rejected" : null,
                                           Comment = ex.Comment,
                                           FromDate = ex.FromDate,
                                           ToDate = ex.ToDate,
                                           VoucherID = ex.VoucherID,
                                           HotelBills = ex.HotelBills,
                                           TravelBills = ex.TravelBills,
                                           MealsBills = ex.MealsBills,
                                           LandLineBills = ex.LandLineBills,
                                           TransportBills = ex.TransportBills,
                                           MobileBills = ex.MobileBills,
                                           Miscellaneous = ex.Miscellaneous,
                                           TotalAmount = ex.TotalAmount,
                                           CreatedOn = ex.CreatedOn
                                       }).ToList().Select(item => new ExpenseModelView
                                       {
                                           ProjectName = item.ProjectName,
                                           PurposeorReason = item.PurposeorReason,
                                           FirstName = item.FirstName,
                                           MiddleName = item.MiddleName,
                                           LastName = item.LastName,
                                           Status = item.Status,
                                           Comment = item.Comment,
                                           FromDate = Convert.ToString(item.FromDate),
                                           ToDate = Convert.ToString(item.ToDate),
                                           VoucherID = item.VoucherID,
                                           HotelBills = item.HotelBills,
                                           TravelBills = item.TravelBills,
                                           MealsBills = item.MealsBills,
                                           LandLineBills = item.LandLineBills,
                                           TransportBills = item.TransportBills,
                                           MobileBills = item.MobileBills,
                                           Miscellaneous = item.Miscellaneous,
                                           TotalAmount = item.TotalAmount,
                                           CreatedOn = Convert.ToString(item.CreatedOn)
                                       }).ToList();

                    return queryresult;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       
      
    }
}
