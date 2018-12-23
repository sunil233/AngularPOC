namespace POC.ViewModels
{
    public class ExpenseModelView
    {
        public int ExpenseID { get; set; }
        public string PurposeorReason { get; set; }
        public string ExpenseStatus { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int UserID { get; set; }
        public string VoucherID { get; set; }
        public int? HotelBills { get; set; }
        public int? TravelBills { get; set; }
        public int? MealsBills { get; set; }
        public int? LandLineBills { get; set; }
        public int? TransportBills { get; set; }
        public int? MobileBills { get; set; }
        public int? Miscellaneous { get; set; }
        public int? TotalAmount { get; set; }
        public string CreatedOn { get; set; }
        public string Comment { get; set; }
        public string ProjectName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public string FullName
        {
            get
            {
                string fullname = string.Empty;
                if (!string.IsNullOrEmpty(FirstName))
                {
                    fullname = FirstName.Trim();
                }
                if (!string.IsNullOrEmpty(MiddleName))
                {
                    fullname = fullname + "," + MiddleName.Trim();
                }
                if (!string.IsNullOrEmpty(LastName))
                {
                    fullname = fullname + "," + LastName.Trim();
                }
                return fullname;
            }
            set { }
        }

        public string Status { get; set; }

    }
}
