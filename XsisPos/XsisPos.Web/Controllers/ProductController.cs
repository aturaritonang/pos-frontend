using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;
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

        [HttpPost]
        public async Task<IActionResult> Create(ChangeProductDto dto)
        {
            string strPayload = JsonConvert.SerializeObject(dto);
            HttpContent content = new StringContent(strPayload, Encoding.UTF8, "application/json");

            ViewBag.CategoryList = new SelectList((List<CategoryDto>)await GetCategories(), "Id", "Name");

            using (var response = await _httpClientHelper.GetHttpClient().PostAsync($"products/", content))
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    dto = JsonConvert.DeserializeObject<ChangeProductDto>(apiResponse)!;
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
            ChangeProductDto item = new ChangeProductDto();
            using (var response = await _httpClientHelper.GetHttpClient().GetAsync($"products/{id}"))
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                //item = JsonConvert.DeserializeObject<ChangeProductDto>(apiResponse)!;
                var result = JsonConvert.DeserializeObject<ProductDto>(apiResponse)!;
                item = new ChangeProductDto()
                {
                    Id = result.Id,
                    CategoryId = result.Category.Id,
                    Initial = result.Initial,
                    Name = result.Name,
                    Description = result.Description,
                    Active = result.Active,
                };
            }
            ViewBag.CategoryList = new SelectList(await GetCategories(), "Id", "Name");
            return View("_Edit", item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ChangeProductDto dto)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
            using (var response = await _httpClientHelper.GetHttpClient().PutAsync($"products/", content))
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    dto = JsonConvert.DeserializeObject<ChangeProductDto>(apiResponse)!;
                    ViewBag.Saved = "Edit successful";
                }
                else
                {
                    ViewBag.Error = "Edit error";
                }
            }

            ViewBag.CategoryList = new SelectList((List<CategoryDto>)await GetCategories(), "Id", "Name");
            return View("_Edit", dto);
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

        public async Task<IActionResult> Details(int id)
        {
            ViewBag.Category = "";
            ChangeProductDto item = new ChangeProductDto();
            using (var response = await _httpClientHelper.GetHttpClient().GetAsync($"products/{id}"))
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ProductDto>(apiResponse)!;
                item = new ChangeProductDto()
                {
                    Id = result.Id,
                    CategoryId = result.Category.Id,
                    Initial = result.Initial,
                    Name = result.Name,
                    Description = result.Description,
                    Active = result.Active,
                };
                ViewBag.Category = result.Category.Initial;
            }
            //ViewBag.CategoryList = new SelectList(await GetCategories(), "Id", "Name");
            return View("_Details", item);
        }
    }
}
