using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using InterviewProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace InterviewProjectMvc1.Controllers
{
    
    public class ManageUser : Controller
    {
        public async Task<IActionResult> Index()
        {
            var httpClient = new HttpClient();
            var responseMessage = await httpClient.GetAsync("https://localhost:44345/api/Users");
            var jsonString = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ClassUser>>(jsonString);
            return View(values);
        }
        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddUser(ClassUser user)
        {
            var httpClient = new HttpClient();
            var jsonUser = JsonConvert.SerializeObject(user);
            StringContent content = new StringContent(jsonUser,Encoding.UTF8,"application/json");
            var responseMessage = await httpClient.PostAsync("https://localhost:44345/api/Users", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View(user);
        }
        [HttpGet]
        public async Task<IActionResult> EditUser(int id)
        {
            var httpClient = new HttpClient();
            var responseMessage = await httpClient.GetAsync("https://localhost:44345/api/Users/" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonUser = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<ClassUser>(jsonUser);
                return View(values);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> EditUser(ClassUser user)
        {
            var httpClient = new HttpClient();
            var jsonUser = JsonConvert.SerializeObject(user);
            var content = new StringContent(jsonUser, Encoding.UTF8, "application/json");
            var responseMessage = await httpClient.PutAsync("https://localhost:44345/api/Users", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View(user);
        }

        public async Task<IActionResult> DeleteUser(int id)
        {
            var httpClient = new HttpClient();
            var responseMessage = await httpClient.DeleteAsync("https://localhost:44345/api/Users/" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
    }

    public class ClassUser
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
