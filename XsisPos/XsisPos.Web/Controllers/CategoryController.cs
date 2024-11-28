using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using XsisPos.Dto;
using XsisPos.Web.Helpers;

namespace XsisPos.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly HttpClientHelper _httpClientHelper;
        public CategoryController(IConfiguration configuration)
        {
            _httpClientHelper = new HttpClientHelper(configuration);
        }

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

        public async Task<IActionResult> List()
        {
            using (var response = await _httpClientHelper.GetHttpClient().GetAsync("categories"))
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                return View(JsonConvert.DeserializeObject<List<CategoryDto>>(apiResponse));
            }
            return View(new List<CategoryDto>());
        }

        public IActionResult Details(int id)
        {
            return View(_list.Where(o => o.Id == id).FirstOrDefault());
        }
    }
}
