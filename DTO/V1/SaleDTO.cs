using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendingMachineManagementAPI.DTO.V1
{
    public class SaleDTO
    {
        public long ID { get; set; }
        public long VendingMachineID { get; set; }
        public long? ProductID { get; set; }
        public long? PaymentMethodID { get; set; }
        public byte ProductAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Cost { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Deposit { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Change { get; set; }
        public DateTime Date { get; set; }
    }
}
