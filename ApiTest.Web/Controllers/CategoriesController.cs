using ApiTest.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public class CategoriesController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CategoriesController(IHttpClientFactory httpClientFactory)
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

    public async Task<IActionResult> Index()
    {
        var client = GetAuthenticatedClient();
        var response = await client.GetAsync("categories");

        if (!response.IsSuccessStatusCode)
        {
            return View(new List<CategoryViewModel>());
        }

        var jsonString = await response.Content.ReadAsStringAsync();
        var categories = JsonSerializer.Deserialize<List<CategoryViewModel>>(jsonString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return View(categories);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CategoryViewModel model)
    {
        var client = GetAuthenticatedClient();
        var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

        var response = await client.PostAsync("categories", content);
        if (response.IsSuccessStatusCode)
        {
            
            return RedirectToAction(nameof(Index));
        }

        var errorDetails = await response.Content.ReadAsStringAsync();
        ModelState.AddModelError(string.Empty, "Error al crear la categoría.");
        return View(model);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var client = GetAuthenticatedClient();
        var response = await client.GetAsync($"categories/{id}");

        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var jsonString = await response.Content.ReadAsStringAsync();
        var category = JsonSerializer.Deserialize<CategoryViewModel>(jsonString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return View(category);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, CategoryViewModel model)
    {
        var client = GetAuthenticatedClient();
        var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

        var response = await client.PutAsync($"categories/{id}", content);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(string.Empty, "Error al actualizar la categoría.");
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

        var response = await client.DeleteAsync($"categories/{id}");

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

        var response = await client.PatchAsync($"categories/{id}/reactivate", null);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(string.Empty, "Error al reactivar la categoria.");
        return RedirectToAction(nameof(Index));
    }
}