using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Task.Manager.DTO;
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

                var projects = JsonConvert.DeserializeObject<List<ProjectDto>>(projectsResponse);

                return View(projects);
            }
        }

        public async Task<IActionResult> Get(int id)
        {
            using (var client = new HttpClient())
            {
                //Project
                var projectUrl = $"{baseUrl}api/projects/{id}";
                var projectResponse = await client.GetStringAsync(projectUrl);
                var project = JsonConvert.DeserializeObject<ProjectDto>(projectResponse);

                //Workers
                var workersUrl = $"{baseUrl}api/workers";
                var workersResponse = await client.GetStringAsync(workersUrl);
                var workers = JsonConvert.DeserializeObject<List<WorkerDto>>(workersResponse);
                ViewBag.Workers = new SelectList(workers, "Id", "Name", project.Workers.Select(x=>x.Id));
                
                //Assignements
                var assignmentsUrl = $"{baseUrl}api/assignments";
                var assignmentsResponse = await client.GetStringAsync(assignmentsUrl);
                var assignments = JsonConvert.DeserializeObject<List<AssignmentDto>>(assignmentsResponse);
                ViewBag.Assignments = new SelectList(assignments, "Id", "Name", project.Assignments.Select(x => x.Id));

                return View(project);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            return View();
        }
    }
}