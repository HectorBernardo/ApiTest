using ApiTest.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ApiTest.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient GetAuthenticatedClient()
        {
            var client = _httpClientFactory.CreateClient("ApiClient");

            var token = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return client;
        }

        private async Task LoadCategoriesAsync(HttpClient client)
        {
            try
            {
                var response = await client.GetAsync("categories");
                if (response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var categories = await JsonSerializer.DeserializeAsync<IEnumerable<CategoryViewModel>>(stream, options);
                    ViewBag.Categories = new SelectList(categories?.Where(c => c.IsDeleted), "CategoryId", "Name");
                    return;
                }
            }
            catch { }

            ViewBag.Categories = new SelectList(Enumerable.Empty<SelectListItem>());
        }

        public async Task<IActionResult> Index()
        {
            var client = GetAuthenticatedClient();
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            // 1. Obtenemos los productos
            var prodResponse = await client.GetAsync("products");
            if (!prodResponse.IsSuccessStatusCode) return View("Error");

            var prodJson = await prodResponse.Content.ReadAsStringAsync();
            var products = JsonSerializer.Deserialize<List<ProductViewModel>>(prodJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<ProductViewModel>();

            // 2. Obtenemos las categorías para hacer el cruce
            var catResponse = await client.GetAsync("categories");
            if (catResponse.IsSuccessStatusCode)
            {
                var catJson = await catResponse.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(catJson);

                var categoryDict = new Dictionary<int, string>();
                foreach (var element in doc.RootElement.EnumerateArray())
                {
                    int id = element.TryGetProperty("categoryId", out var idProp) ? idProp.GetInt32() :
                            (element.TryGetProperty("CategoryId", out idProp) ? idProp.GetInt32() : 0);

                    string name = element.TryGetProperty("name", out var nameProp) ? nameProp.GetString() :
                                 (element.TryGetProperty("Name", out nameProp) ? nameProp.GetString() : "Sin nombre");

                    if (id > 0 && !string.IsNullOrEmpty(name))
                    {
                        categoryDict[id] = name;
                    }
                }

                // 3. Asignamos el nombre de la categoría a cada producto en su propiedad CategoryName
                foreach (var p in products)
                {
                    if (categoryDict.TryGetValue(p.CategoryId, out var catName))
                    {
                        p.CategoryName = catName;
                    }
                    else
                    {
                        p.CategoryName = "Sin categoría";
                    }
                }
            }

            return View(products);
        }

        public async Task<IActionResult> Create()
        {
            var client = GetAuthenticatedClient();
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            await LoadCategoriesAsync(client);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name, string description, decimal price, int stock, int categoryId)
        {
            var client = GetAuthenticatedClient();
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            var payload = new
            {
                Name = name,
                Description = description,
                Price = price,
                Stock = stock,
                CategoryId = categoryId
            };

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("products", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            var errorDetails = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"Error al crear producto: {errorDetails}");

            await LoadCategoriesAsync(client);
            return View();
        }

        public async Task<IActionResult> Edit(int id)
        {
            var client = GetAuthenticatedClient();
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            var response = await client.GetAsync($"products/{id}");

            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var product = await JsonSerializer.DeserializeAsync<ProductViewModel>(stream, options);

                await LoadCategoriesAsync(client);
                return View(product);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductViewModel model)
        {
            var client = GetAuthenticatedClient();
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"products/{model.ProductId}", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            var errorDetails = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"Error al actualizar: {errorDetails}");

            await LoadCategoriesAsync(client);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var client = GetAuthenticatedClient();
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            var response = await client.DeleteAsync($"products/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, "Error al eliminar la categoría.");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Reactivate(int id)
        {
            var client = GetAuthenticatedClient();
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            var response = await client.PatchAsync($"products/{id}/reactivate", null);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, "Error al reactivar el producto.");
            return RedirectToAction(nameof(Index));
        }
    }
}