using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
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
            List<CategoryDto> list = new List<CategoryDto>();
            using (var response = await _httpClientHelper.GetHttpClient().GetAsync("categories"))
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                list = JsonConvert.DeserializeObject<List<CategoryDto>>(apiResponse)!;
            }
            return PartialView("_List", list);
        }

        public async Task<IActionResult> Details(int id)
        {
            CategoryDto item = new CategoryDto();
            using (var response = await _httpClientHelper.GetHttpClient().GetAsync($"categories/{id}"))
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                item = JsonConvert.DeserializeObject<CategoryDto>(apiResponse)!;
            }
            return View("_Details", item);
        }

        public IActionResult Create()
        {
            return View("_Create");
        }

        [HttpPost]
        public async Task<IActionResult> Create(ModifyCategoryDto dto)
        {
            string strPayload = JsonConvert.SerializeObject(dto);
            HttpContent content = new StringContent(strPayload, Encoding.UTF8, "application/json");
            using (var response = await _httpClientHelper.GetHttpClient().PostAsync($"categories/", content))
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    dto = JsonConvert.DeserializeObject<ModifyCategoryDto>(apiResponse)!;
                    ViewBag.Saved = "Create successful";
                }
                else
                {
                    ViewBag.Error = "Create error";
                }
            }
            return View("_Create", dto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            ModifyCategoryDto item = new ModifyCategoryDto();
            using (var response = await _httpClientHelper.GetHttpClient().GetAsync($"categories/{id}"))
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                item = JsonConvert.DeserializeObject<ModifyCategoryDto>(apiResponse)!;
            }
            return View("_Edit", item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ModifyCategoryDto dto)
        {
            string strPayload = JsonConvert.SerializeObject(dto);
            HttpContent content = new StringContent(strPayload, Encoding.UTF8, "application/json");
            using (var response = await _httpClientHelper.GetHttpClient().PutAsync($"categories/", content))
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    dto = JsonConvert.DeserializeObject<ModifyCategoryDto>(apiResponse)!;
                    ViewBag.Saved = "Edit successful";
                }
                else
                {
                    ViewBag.Error = "Edit error";
                }
            }
            return View("_Edit", dto);
        }
    }
}
