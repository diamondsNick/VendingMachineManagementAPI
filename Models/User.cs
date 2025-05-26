using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace VendingMachineManagementAPI.Models
{
    public class User
    {
        public long ID { get; set; }
        [MaxLength (100)]
        public string FullName { get; set; }
        [MaxLength (100)]
        [Required]
        public string Email { get; set; }
        public long? RoleID { get; set; }
        [MaxLength (10)]
        [AllowNull]
        public string Language { get; set; }
        [MaxLength(100)]
        [Required]
        public string Login { get; set; }
        [MaxLength(100)]
        [Required]
        public string Password { get; set; }
        public Role Role { get; set; }
        public IList<CompanyUser> CompanyUsers { get; set; }
        public IList<Maintenance> Maintenances { get; set; }
    }
}
