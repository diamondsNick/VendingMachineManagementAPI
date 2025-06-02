using Microsoft.AspNetCore.Mvc;

namespace VendingMachineManagementAPI.Controllers
{
    public class PaymentMethodController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
