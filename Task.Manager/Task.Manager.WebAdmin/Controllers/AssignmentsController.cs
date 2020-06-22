using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Task.Manager.DTO;
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

                var assignments = JsonConvert.DeserializeObject<List<AssignmentDto>>(assignmentsResponse);

                return View(assignments);
            }
        }

        public async Task<IActionResult> Get(int id)
        {
            using (var client=new HttpClient())
            {
                var assignmentsUrl = $"{baseUrl}api/assignments/{id}";
                var assignmentsResponse = await client.GetStringAsync(assignmentsUrl);

                var assignments = JsonConvert.DeserializeObject<AssignmentDto>(assignmentsResponse);

                return View(assignments);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id,int projectId)
        {
            using (var client = new HttpClient())
            {
                //Assignment
                var assignmentsUrl = $"{baseUrl}api/assignments/{id}";
                var assignmentsResponse = await client.GetStringAsync(assignmentsUrl);
                var assignment = JsonConvert.DeserializeObject<AssignmentDto>(assignmentsResponse);
                
                //Projects
                var projects = deserializeProjectDto(client);
                var project = new List<ProjectDto>();
                project.Add(projects.Result.FirstOrDefault(x=>x.ProjectId== projectId));
                ViewBag.Projects = new SelectList(project, "ProjectId", "Name");

                //Workers
                var workers = deserializeWorkerDtos(client);
                var worker = new List<WorkerDto>();
                worker.AddRange(project[0].Workers);
                ViewBag.Workers = new SelectList(worker, "WorkerId", "Name");

                return View(assignment);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AssignmentDto assignmentDto)
        {
            using (var client = new HttpClient())
            {
                if (assignmentDto.Status=="Inactive" || assignmentDto.Status == "Completed")
                {
                    assignmentDto.WorkerId = null;
                }
                var assignmentsUrl = $"{baseUrl}api/assignments/{assignmentDto.AssignmentId}";
                var assignmentDtoString = JsonConvert.SerializeObject(assignmentDto);
                var assignmentsResponse = await client.PutAsync(assignmentsUrl, new StringContent(assignmentDtoString,Encoding.UTF8, "application/json"));

                if (assignmentsResponse.IsSuccessStatusCode)
                {
                    assignmentDto =
                        JsonConvert.DeserializeObject<AssignmentDto>(
                            await assignmentsResponse.Content.ReadAsStringAsync());
                }
                else
                {
                    ModelState.AddModelError(string.Empty,"Update failed");
                }
               
                return RedirectToAction("Index","Projects");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create(int id)
        {
            using (var client = new HttpClient())
            {
                var projects = deserializeProjectDto(client);
                var project = new List<ProjectDto>(projects.Result);
                ViewBag.Projects = new SelectList(project,"ProjectId","Name");
                var workers = new List<WorkerDto>();
                workers.AddRange(project[0].Workers);
                ViewBag.Workers = new SelectList(workers, "WorkerId", "Name");
                return View(new AssignmentDto());
            }
        }

        [HttpPost]
        //POST: Assignment
        public async Task<IActionResult> Create(AssignmentDto assignmentDto)
        {
            using (var client = new HttpClient())
            {
                if (assignmentDto.WorkerId==0)
                {
                    assignmentDto.WorkerId = null;
                }
                var assignmentUrl = $"{baseUrl}api/assignments/";
                var assignmentDtoString = JsonConvert.SerializeObject(assignmentDto);
                var assignmentResponse = await client.PostAsync(assignmentUrl,
                    new StringContent(assignmentDtoString, Encoding.UTF8, "application/json"));
                if (assignmentResponse.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Projects");

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Update failed");
                    return View(assignmentDto);
                }
            }
        }

        private async Task<List<ProjectDto>> deserializeProjectDto(HttpClient client)
        {
            var projectsUrl = $"{baseUrl}api/projects/";
            var projectsResponse = await client.GetStringAsync(projectsUrl);
            var projects = JsonConvert.DeserializeObject<List<ProjectDto>>(projectsResponse);
           
            return projects;
        }

        private async Task<List<WorkerDto>> deserializeWorkerDtos(HttpClient client)
        {
            var workersUrl = $"{baseUrl}api/workers";
            var workersResponse = await client.GetStringAsync(workersUrl);
            var workers = JsonConvert.DeserializeObject<List<WorkerDto>>(workersResponse);

            return workers;
        }
    }
}