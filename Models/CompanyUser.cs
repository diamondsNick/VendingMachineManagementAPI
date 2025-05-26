namespace VendingMachineManagementAPI.Models
{
    public class CompanyUser
    {
        public long CompanyID { get; set; }
        public long UserID { get; set; }
        public Company Company { get; set; }
        public User User { get; set; }
    }
}
