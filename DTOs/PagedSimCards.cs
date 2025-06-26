using System.Collections.Generic;
using VendingMachineManagementAPI.Models;

namespace VendingMachineManagementAPI.DTOs
{
    public class PagedSimCards
    {
        public int TotalCount { get; set; }
        public List<SimCard> Sims { get; set; }
    }
}
