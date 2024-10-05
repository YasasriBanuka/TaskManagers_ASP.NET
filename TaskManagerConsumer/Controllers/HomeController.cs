using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TaskManagers.Models;

namespace TaskManagerConsumer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        // Admin Login (POST)
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            // Hardcoded admin credentials
            string Username = "manager";
            string Password = "task_11";

            // Validate the credentials
            if (username == Username && password == Password)
            {
                Session["IsAuthenticated"] = true;
                return RedirectToAction("GetTasks");

            }
            else
            {
                ViewBag.Message = "Invalid credentials. Please try again.";
                return View();
            }
        }

        public async Task<ActionResult> GetTasks()
        {
            List<TaskManagers.Models.TASK> reservationList = new List<TaskManagers.Models.TASK>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44324/api/TASKs"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    reservationList = JsonConvert.DeserializeObject<List<TASK>>(apiResponse);
                } 
            }
            return View(reservationList);
        }


        // ADD PRODUCT CODE  

        public async Task<ActionResult> AddTask()
        {
            return View();
        }

        //----------------------
        [HttpPost]

        public async Task<ActionResult> AddTask(TASK pr)
        {
            TASK p = new TASK();
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(pr), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync("https://localhost:44324/api/TASKs", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    JsonConvert.DeserializeObject<TASK>(apiResponse);
                }
            }
            return RedirectToAction("GetTasks");
        }
        public async Task<ActionResult> UpdateTask(string id)
        {
             TASK task = new TASK();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    using (var response = await httpClient.GetAsync($"https://localhost:44324/api/TASKs/{id}"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            task = JsonConvert.DeserializeObject<TASK>(apiResponse);
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Error fetching Task details.");
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    ModelState.AddModelError(string.Empty, $"Request error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                }
            }

            return View(task);
        }
        //----------------------------------------------------------------------------------------------------


        [HttpPost]
        public async Task<ActionResult> UpdateTask(TASK tr)
        {
            if (!ModelState.IsValid)
            {
                return View(tr);
            }

            using (var httpClient = new HttpClient())
            {
                try
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(tr), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PutAsync($"https://localhost:44324/api/TASKs/{tr.TaskID}", content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            // Optionally, redirect or provide a success message
                            return RedirectToAction("GetTasks");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Error updating the Task.");
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    ModelState.AddModelError(string.Empty, $"Request error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                }
            }

            return View(tr);
        }

        [HttpGet]
        public async Task<ActionResult> DeleteTask(int id)
        {
            TASK p = new TASK();
            using (var httpClient = new HttpClient())
            using (var response = await httpClient.DeleteAsync($"https://localhost:44324/api/TASKs/{id}"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                p = JsonConvert.DeserializeObject<TASK>(apiResponse);
                return RedirectToAction("GetTasks");
            }
        }






    }
}