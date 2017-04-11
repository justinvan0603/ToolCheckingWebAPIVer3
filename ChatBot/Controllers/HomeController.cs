using System.Linq;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ChatBot.Controllers
{
    public class HomeController : Controller
    {
        //private IBotDomainService _botDomainService;

        //public HomeController(IBotDomainService botDomainService)
        //{
        //    //_botDomainService = botDomainService;
        //}


        // GET: /<controller>/
        public IActionResult Index()
        {
          //  _botDomainService.GetAll();

            //var x = _botDomainService.GetAll().ToList();
         //   int a = 5;
            return View();
        }
    }
}
