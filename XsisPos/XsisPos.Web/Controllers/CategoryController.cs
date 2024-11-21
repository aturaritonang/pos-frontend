using Microsoft.AspNetCore.Mvc;

namespace XsisPos.Web.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
