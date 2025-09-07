using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VendingMachineManagementAPI.Models
{
    public class PaymentMethod
    {
        public long ID { get; set; }
        [MaxLength (20)]
        [Required]
        public string Name { get; set; }
        public IList<Sale> Sales { get; set; }
        public IList<MachinePaymentMethod> MachinePaymentMethods { get; set; }
    }
}
