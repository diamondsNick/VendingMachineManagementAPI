using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace VendingMachineManagementAPI.Models
{
    public class SimCard
    {
        public long Number { get; set; }
        [MaxLength (25)]
        [Required]
        public string Vendor { get; set; }
        public Modem Modem { get; set; }
    }
}
