using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Linq;

namespace veeb.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NordpoolController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public NordpoolController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("{country}/{start}/{end}")]
        public async Task<IActionResult> GetNordPoolPrices(string country, string start, string end)
        {
            // на всякий случай экранируем параметры
            var url = $"https://dashboard.elering.ee/api/nps/price?start={Uri.EscapeDataString(start)}&end={Uri.EscapeDataString(end)}";

            var response = await _httpClient.GetAsync(url);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, new
                {
                    message = "Upstream error from Elering",
                    url,
                    body = responseBody
                });
            }

            using var jsonDoc = JsonDocument.Parse(responseBody);
            var root = jsonDoc.RootElement;

            if (!root.TryGetProperty("data", out var dataEl) || dataEl.ValueKind != JsonValueKind.Object)
            {
                return BadRequest(new
                {
                    message = "Elering response doesn't contain expected 'data' object.",
                    url,
                    raw = root.ToString()
                });
            }

            var key = country.ToLowerInvariant();
            if (!dataEl.TryGetProperty(key, out var countryArray) || countryArray.ValueKind != JsonValueKind.Array)
            {
                var available = string.Join(",", dataEl.EnumerateObject().Select(p => p.Name));
                return NotFound(new
                {
                    message = $"Country '{key}' not found in Elering response.",
                    availableKeys = available
                });
            }

            // отдаем ровно массив по стране
            return Content(countryArray.GetRawText(), "application/json");
        }

        // Доп. удобный эндпоинт на всякий: текущая цена по стране (EE/LV/LT/FI)
        [HttpGet("{country}/current")]
        public async Task<IActionResult> GetCurrent(string country)
        {
            var url = $"https://dashboard.elering.ee/api/nps/price/{country.ToUpperInvariant()}/current";
            var response = await _httpClient.GetAsync(url);
            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, new { message = "Upstream error from Elering", url, body });

            using var doc = JsonDocument.Parse(body);
            if (!doc.RootElement.TryGetProperty("data", out var dataEl))
                return BadRequest(new { message = "No 'data' in Elering response.", raw = doc.RootElement.ToString() });

            return Content(dataEl.GetRawText(), "application/json");
        }
    }
}
