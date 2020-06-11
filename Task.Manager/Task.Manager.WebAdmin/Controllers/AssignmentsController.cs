using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Task.Manager.Entities;

namespace Task.Manager.AdminWeb.Controllers
{
    public class AssignmentsController : Controller
    {
        private string baseUrl = "https://localhost:44358/";
        
        public async Task<IActionResult> Index()
        {
            using (var client = new HttpClient())
            {
                var assignmentsUrl = $"{baseUrl}api/assignments";
                var assignmentsResponse = await client.GetStringAsync(assignmentsUrl);

                var assignments = JsonConvert.DeserializeObject<List<Assignment>>(assignmentsResponse);

                return View(assignments);
            }
        }

        public async Task<IActionResult> Get(int id)
        {
            using (var client=new HttpClient())
            {
                var assignmentsUrl = $"{baseUrl}/api/assignments/{id}";
                var assignmentsResponse = await client.GetStringAsync(assignmentsUrl);

                var assignments = JsonConvert.DeserializeObject<Assignment>(assignmentsResponse);

                return View(assignments);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            return View();
        }
    }
}