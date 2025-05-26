using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace VendingMachineManagementAPI.Models
{
    public class Maintenance
    {
        public long ID { get; set; }
        public long VengingMachineID { get; set; } 
        public long? MaintainerID { get; set; }
        public DateTime MaintenanceDate { get; set; }
        [AllowNull]
        [MaxLength(250)]
        public string WorkDescription { get; set; }
        [MaxLength(250)]
        [AllowNull]
        public string ProblemDescription { get; set; }
        public User Maintainer { get; set; }
        public VendingMachine VendingMachine { get; set; }
    }
}
