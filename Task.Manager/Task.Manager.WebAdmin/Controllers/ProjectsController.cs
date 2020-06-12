using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Task.Manager.Entities;

namespace Task.Manager.WebAdmin.Controllers
{
    public class ProjectsController : Controller
    {
        private string baseUrl = "https://localhost:44358/";

        public async Task<IActionResult> Index()
        {
            using (var client = new HttpClient())
            {
                var projectsUrl = $"{baseUrl}api/projects";
                var projectsResponse = await client.GetStringAsync(projectsUrl);

                var projects = JsonConvert.DeserializeObject<List<Assignment>>(projectsResponse);

                return View(projects);
            }
        }

        public async Task<IActionResult> Get(int id)
        {
            using (var client = new HttpClient())
            {
                var projectUrl = $"{baseUrl}/api/projects/{id}";
                var projectResponse = await client.GetStringAsync(projectUrl);

                var project = JsonConvert.DeserializeObject<Assignment>(projectResponse);

                return View(project);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            return View();
        }
    }
}