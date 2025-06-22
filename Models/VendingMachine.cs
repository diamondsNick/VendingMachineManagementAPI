﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Microsoft.VisualBasic;

namespace VendingMachineManagementAPI.Models
{
    public class VendingMachine
    {
        public long ID { get; set; }
        public long? StatusID { get; set; }
        public long? OperatingModeID { get; set; }
        public long? CompanyID { get; set; }
        public long? ModelID { get; set; }
        public long? ModemID { get; set; }
        [MaxLength (100)]
        [Required]
        public string TimeZone { get; set; }
        [MaxLength (100)]
        [Required]
        public string Name { get; set; }
        [MaxLength(350)]
        [Required]
        public string Adress { get; set; }
        [MaxLength(100)]
        [AllowNull]
        public string Coordinates { get; set; }
        [MaxLength (100)]
        [Required]
        public string PlacementType { get; set; }
        [MaxLength(19)]
        [Required]
        public string PlacementDate { get; set; }
        [MaxLength(5)]
        [Required]
        public string StartHours { get; set; }
        [MaxLength(5)]
        [Required]
        public string EndHours { get; set; }
        [JsonIgnore]
        public IList<MachinePaymentMethod> MachinePaymentMethods { get; set; }
        public Status Status { get; set; }
        public Modem Modem { get; set; }
        [JsonIgnore]
        public IList<Sale> Sales { get; set; }
        [JsonIgnore]
        public OperatingMode OperatingMode { get; set; }
        [JsonIgnore]
        public IList<Maintenance> Maintenances { get; set; }
        [JsonIgnore]
        public IList<VendingMachineMoney> VendingMachineMoney { get; set; }
        [JsonIgnore]
        public IList<VendingAvaliability> VendingAvaliabilities { get; set; }
        public VendingMachineMatrix VendingMachineMatrix { get; set; }
        public Company Company { get; set; }
    }
}
