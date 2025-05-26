﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendingMachineManagementAPI.Models
{
    public class Money
    {
        public long ID { get; set; }
        [MaxLength (45)]
        [Required]
        public string Name { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Value { get; set; }
    }
}
