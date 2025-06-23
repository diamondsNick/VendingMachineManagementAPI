using System.Collections.Generic;
using VendingMachineManagementAPI.Models;

namespace VendingMachineManagementAPI.DTOs
{
    public class PagedModems
    {
        public int TotalCount { get; set; }
        public List<Modem> Modems { get; set; }
    }
}
