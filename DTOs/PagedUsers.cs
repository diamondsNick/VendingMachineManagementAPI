using System.Collections.Generic;
using VendingMachineManagementAPI.Models;

namespace VendingMachineManagementAPI.DTOs
{
    public class PagedUsers
    {
        public int TotalCount { get; set; }
        public List<User> Users { get; set; }
    }
}
