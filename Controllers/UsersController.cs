using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using UserCatalogMvc.Models;
using System.Collections.Generic;
using System.Text;

namespace UserCatalogMvc.Controllers
{
    public class UsersController : Controller
    {
        private readonly HttpClient _httpClient;
        public UsersController()
        {
            _httpClient = new HttpClient();
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("https://jsonplaceholder.typicode.com/users");

            if (!response.IsSuccessStatusCode)
            {
                return View("Error al intentar visualizar los datos");
            }

            var json = await response.Content.ReadAsStringAsync();
            var users = JsonSerializer.Deserialize<List<User>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(users);
        }

        //Metodo para exportar la lista de datos
        [HttpGet]
        public async Task<IActionResult> ExportCsv()
        {
            var response = await _httpClient.GetAsync("https://jsonplaceholder.typicode.com/users");

            if (!response.IsSuccessStatusCode)
            {
                return Content("Error al obtener los datos.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var users = JsonSerializer.Deserialize<List<User>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var csv = new StringBuilder();
            csv.AppendLine("Nombre;Usuario;Email;Ciudad;Empresa");

            foreach (var user in users)
            {
                csv.AppendLine($"{user.Name};{user.Username};{user.Email};{user.Address.City};{user.Company.Name}");
            }

            var bytes = System.Text.Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", "usuarios.csv");
        }

    }
}
