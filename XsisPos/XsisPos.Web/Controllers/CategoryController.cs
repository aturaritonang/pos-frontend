using Microsoft.AspNetCore.Mvc;
using XsisPos.Dto;

namespace XsisPos.Web.Controllers
{
    public class CategoryController : Controller
    {
        private static List<CategoryDto> _list = new List<CategoryDto>() {
            new CategoryDto()
            {
                Id = 1,
                Initial = "Food",
                Name = "Fresh Food",
            },
            new CategoryDto()
            {
                Id = 2,
                Initial = "Drink",
                Name = "Fresh Drink",
            },
        };

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult List()
        {
            return View(_list);
        }

        public IActionResult Details(int id)
        {
            return View(_list.Where(o => o.Id == id).FirstOrDefault());
        }
    }
}
