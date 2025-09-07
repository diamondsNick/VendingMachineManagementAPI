using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace VendingMachineManagementAPI.Models
{
    public class Company
    {
        public long ID { get; set; }
        [MaxLength (200)]
        [Required]
        public string Name { get; set; }
        [Column (TypeName = "decimal(18,2)")]
        [Required]
        public decimal Finances { get; set; }
        [MaxLength(11)]
        [Required]
        public string Phone { get; set; }
        public string Adress { get; set; }
        public string RegistrationDate { get; set; }
        [AllowNull]
        public long? ParentCompanyID { get; set; }
        
        public IList<User> CompanyUsers { get; set; }
        
        public IList<VendingMachine> VendingMachines { get; set; }
        
        public IList<Modem> Modems { get; set; }
        
        public IList<SimCard> SimCards { get; set; }
    }
}
