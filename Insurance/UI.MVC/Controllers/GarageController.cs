using Insurance.BL;
using Microsoft.AspNetCore.Mvc;

namespace UI.MVC.Controllers
{
    public class GarageController : Controller
    {
        private readonly IManager _manager;
        public GarageController(IManager manager)
        {
            _manager = manager;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult Details()
        {
            return View();
        }
    }
}