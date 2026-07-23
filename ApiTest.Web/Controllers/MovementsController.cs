using ApiTest.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public class MovementsController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public MovementsController(IHttpClientFactory httpClientFactory)
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

    private async Task LoadProductsDropdown(HttpClient client)
    {
        try
        {
            var response = await client.GetAsync("products");
            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var products = await JsonSerializer.DeserializeAsync<IEnumerable<ProductViewModel>>(stream, options);
                ViewBag.Products = new SelectList(products?.Where(c => c.IsDeleted), "ProductId", "Name");
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
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.GetAsync("movements");

        if (!response.IsSuccessStatusCode)
        {
            return View(new List<MovementViewModel>());
        }

        var jsonString = await response.Content.ReadAsStringAsync();
        var movements = JsonSerializer.Deserialize<List<MovementViewModel>>(jsonString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return View(movements);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var client = GetAuthenticatedClient();
        var token = HttpContext.Session.GetString("JWToken");
        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Login", "Account");
        }

        // Método auxiliar para llenar el select con los productos activos
        await LoadProductsDropdown(client);
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(MovementViewModel model)
    {
        var client = GetAuthenticatedClient();
        var token = HttpContext.Session.GetString("JWToken");
        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Login", "Account");
        }

        var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("movements", content);

        if (response.IsSuccessStatusCode)
        {
            
            return RedirectToAction(nameof(Index));
        }

        var errorDetails = await response.Content.ReadAsStringAsync();
        ModelState.AddModelError(string.Empty, "Error al registrar el movimiento de inventario.");

        await LoadProductsDropdown(client);
        return View(model);
    }
}