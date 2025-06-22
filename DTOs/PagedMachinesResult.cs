using System.Collections.Generic;
using VendingMachineManagementAPI.Models;

namespace VendingMachineManagementAPI.DTOs
{
    public class PagedMachinesResult
    {
        public int TotalCount { get; set; }
        public List<VendingMachine> VendingMachines { get; set; }
    }
}
