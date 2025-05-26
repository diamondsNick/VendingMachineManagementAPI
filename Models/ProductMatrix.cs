using System.Collections.Generic;

namespace VendingMachineManagementAPI.Models
{
    public class ProductMatrix
    {
        public long ID { get; set; }
        public long? ManufacturerID { get; set; }
        public long? MachineModelID { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public MachineModel MachineModel { get; set; }
        public IList<VendingMachineMatrix> VendingMachinesMatrices { get; set; }
    }
}
