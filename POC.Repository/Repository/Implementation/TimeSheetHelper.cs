

namespace POC.Repository
{
    public class TimeSheetHelper
    {
        public static string GetFullName(string FirstName, string MiddleName, string LastName)
        {
            string FullName = string.Empty;
            if (!string.IsNullOrWhiteSpace(FirstName))
            {
                FullName = FirstName.Trim();
            }
            if (!string.IsNullOrWhiteSpace(MiddleName))
            {
                FullName = FullName + "," + MiddleName.Trim();
            }
            if (!string.IsNullOrWhiteSpace(LastName))
            {
                FullName = FullName + "," + LastName.Trim();
            }
            return FullName;
        }
    }
}
