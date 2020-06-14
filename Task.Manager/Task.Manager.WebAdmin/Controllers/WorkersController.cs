using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Task.Manager.DTO;
using Task.Manager.Entities;

namespace Task.Manager.WebAdmin.Controllers
{
    public class WorkersController : Controller
    {
        private string baseUrl = "https://localhost:44358/";

        public async Task<IActionResult> Index()
        {
            using (var client = new HttpClient())
            {
                var workersUrl = $"{baseUrl}api/workers";
                var workersResponse = await client.GetStringAsync(workersUrl);

                var workers = JsonConvert.DeserializeObject<List<Assignment>>(workersResponse);

                return View(workers);
            }
        }

        public async Task<IActionResult> Get(int id)
        {
            using (var client = new HttpClient())
            {
                var workersUrl = $"{baseUrl}api/workers/{id}";
                var workersResponse = await client.GetStringAsync(workersUrl);

                var workers = JsonConvert.DeserializeObject<Assignment>(workersResponse);

                return View(workers);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            using (var client = new HttpClient())
            {
                //Workers
                var workersUrl = $"{baseUrl}api/workers/{id}";
                var workersResponse = await client.GetStringAsync(workersUrl);
                var worker = JsonConvert.DeserializeObject<WorkerDto>(workersResponse);

                //Projects
                var projectsUrl = $"{baseUrl}api/projects";
                var projectsResponse = await client.GetStringAsync(projectsUrl);
                var projects = JsonConvert.DeserializeObject<List<ProjectDto>>(projectsResponse);
                ViewBag.Projects = new SelectList(projects, "Id", "Name", worker.Project.Id);

                return View(worker);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(WorkerDto workerDto)
        {
            using (var client = new HttpClient())
            {
                var workersUrl = $"{baseUrl}api/workers/{workerDto.Id}";
                var workerDtoString = JsonConvert.SerializeObject(workerDto);
                var workersResponse = await client.PutAsync(workersUrl,
                    new StringContent(workerDtoString, Encoding.UTF8, "application/json"));

                if (workersResponse.IsSuccessStatusCode)
                {
                    workerDto =
                        JsonConvert.DeserializeObject<WorkerDto>(
                            await workersResponse.Content.ReadAsStringAsync());
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Update failed");
                }

                return View(workerDto);
            }
        }


    }
}