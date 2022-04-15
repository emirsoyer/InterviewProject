using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace InterviewProjectMvc1.Controllers
{
    public class ManageFirm : Controller
    {
        public async Task<IActionResult> Index()
        {
            var httpClient = new HttpClient();
            var responseMessage = await httpClient.GetAsync("https://localhost:44345/api/Firms");
            var jsonString = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ClassFirm>>(jsonString);
            return View(values);
        }
        [HttpGet]
        public IActionResult AddFirm()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddFirm(ClassFirm firm)
        {
            var httpClient = new HttpClient();
            var jsonFirm = JsonConvert.SerializeObject(firm);
            StringContent content = new StringContent(jsonFirm, Encoding.UTF8, "application/json");
            var responseMessage = await httpClient.PostAsync("https://localhost:44345/api/Firms", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View(firm);
        }
        [HttpGet]
        public async Task<IActionResult> EditFirm(int id)
        {
            var httpClient = new HttpClient();
            var responseMessage = await httpClient.GetAsync("https://localhost:44345/api/Firms/" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonFirm = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<ClassFirm>(jsonFirm);
                return View(values);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> EditFirm(ClassFirm firm)
        {
            var httpClient = new HttpClient();
            var jsonFirm = JsonConvert.SerializeObject(firm);
            var content = new StringContent(jsonFirm, Encoding.UTF8, "application/json");
            var responseMessage = await httpClient.PutAsync("https://localhost:44345/api/Firms", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View(firm);
        }
    }

    public class ClassFirm
    {
        public int FirmId { get; set; }
        public string FirmName { get; set; }
    }
}
