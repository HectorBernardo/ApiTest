using ApiTest.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ApiTest.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate()
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.PostAsync("auth/login", null);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Error al generar el token.");
                return View("Login");
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            var tokenObj = JsonSerializer.Deserialize<TokenResponse>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (tokenObj != null && !string.IsNullOrEmpty(tokenObj.Token))
            {
                HttpContext.Session.SetString("JWToken", tokenObj.Token);

                return RedirectToAction("Index", "Movements");
            }

            return View("Login");
        }
    }
}
