using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VendingMachineManagementAPI.Models
{
    public class Role
    {
        public long ID { get; set; }
        [MaxLength (30)]
        [Required]
        public string Name { get; set; }
        public IList<User> Users { get; set; }
    }
}
