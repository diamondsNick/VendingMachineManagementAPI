using System.ComponentModel.DataAnnotations;

namespace VendingMachineManagementAPI.DTOs.V1
{
    public class UserDTO
    {
        public long ID { get; set; }
        [MaxLength(100)]
        public string FullName { get; set; }
        [MaxLength(100)]
        public string Email { get; set; }
        public long? RoleID { get; set; }
        public string RegistrationDate { get; set; }
        public string Phone { get; set; }
        public long? CompanyID { get; set; }
        [MaxLength(10)]
        public string Language { get; set; }
        [MaxLength(100)]
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
