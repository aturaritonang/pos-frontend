using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using XsisPos.Dto;
using XsisPos.Web.Helpers;

namespace XsisPos.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClientHelper _httpClientHelper;
        public ProductController(IConfiguration configuration)
        {
            _httpClientHelper = new HttpClientHelper(configuration);
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> List()
        {
            List<ProductDto> list = new List<ProductDto>();
            using (var response = await _httpClientHelper.GetHttpClient().GetAsync("products"))
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                list = JsonConvert.DeserializeObject<List<ProductDto>>(apiResponse)!;
            }
            return PartialView("_List", list);
        }

        public async Task<IActionResult> Create()
        {
            var categories = await GetCategories();
            ViewBag.CategoryList = new SelectList((List<CategoryDto>)categories, "Id", "Name");
            return View("_Create");
        }

        public async Task<List<CategoryDto>> GetCategories()
        {
            List<CategoryDto> list = new List<CategoryDto>();
            using (var response = await _httpClientHelper.GetHttpClient().GetAsync("categories"))
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                list = JsonConvert.DeserializeObject<List<CategoryDto>>(apiResponse)!;
            }
            return list;
        }
    }
}
