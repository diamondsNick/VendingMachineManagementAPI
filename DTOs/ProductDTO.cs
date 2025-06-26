using System.Collections.Generic;
using VendingMachineManagementAPI.Models;

namespace VendingMachineManagementAPI.DTOs
{
    public class ProductDTO
    {
        public int TotalCount { get; set; }
        public List<Product> Products { get; set; }
    }
}
