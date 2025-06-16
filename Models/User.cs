using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
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
        [MaxLength(11)]
        [Required]
        public string Phone { get; set; }
        public string RegistrationDate { get; set; }
        public long? RoleID { get; set; }
        public long? CompanyID { get; set; }
        [MaxLength (10)]
        [AllowNull]
        public string Language { get; set; }
        [MinLength(12)]
        [MaxLength(100)]
        [Required]
        public string Login { get; set; }
        [Required]
        [MinLength(12)]
        [MaxLength(24)]
        public string Password { get; set; }
        
        public Role Role { get; set; }
        
        public Company Company { get; set; }
        [JsonIgnore]
        public IList<Maintenance> Maintenances { get; set; }
    }
}
