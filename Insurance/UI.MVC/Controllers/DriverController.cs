using Insurance.BL;
using Insurance.DAL.EF;
using Microsoft.AspNetCore.Mvc;

namespace UI.MVC.Controllers
{
    public class DriverController : Controller
    {
        private readonly IManager _manager;
        public DriverController(IManager manager)
        {
            _manager = manager;
        }
        
        [HttpGet]
        public IActionResult Details(int socialnumber)
        {
            return View(_manager.GetDriver(socialnumber));
        }
    }
}