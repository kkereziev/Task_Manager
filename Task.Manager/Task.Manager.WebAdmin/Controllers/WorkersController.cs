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

                var workers = JsonConvert.DeserializeObject<List<WorkerDto>>(workersResponse);

                return View(workers);
            }
        }

        public async Task<IActionResult> Get(int id)
        {
            using (var client = new HttpClient())
            {
                //Workers
                var workersUrl = $"{baseUrl}api/workers/{id}";
                var workersResponse = await client.GetStringAsync(workersUrl);
                var worker = JsonConvert.DeserializeObject<WorkerDto>(workersResponse);
                
                return View(worker);
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
                var projects = deserializeProjectDtos(client);
                ViewBag.Projects = new SelectList(projects.Result, "ProjectId", "Name", worker.Project.ProjectId);

                //Assignments
                var assignments
                    = deserializeAssignmentDtos(client);
                ViewBag.Assignments = new MultiSelectList(assignments.Result, "AssignmentId", "Title",worker.Assignments.Select(x=>x.AssignmentId));

                //Roles
                var roles=deserializeRoleDtos(client);
                ViewBag.Roles = new SelectList(roles.Result, "RoleId", "Name", worker.Role.RoleId);
                
                return View(worker);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(WorkerDto workerDto)
        {
            
            using (var client = new HttpClient())
            {
                //var projects=deserializeProjectDtos(client);
                //var roles = deserializeRoleDtos(client);
                //var assignments = deserializeAssignmentDtos(client);

                //var projId = workerDto.Project.ProjectId;
                //workerDto.Project = projects.Result.SingleOrDefault(x => x.ProjectId == projId);

                //var roleId = workerDto.Role.RoleId;
                //workerDto.Role = roles.Result.SingleOrDefault(x => x.RoleId == roleId);
                
                //workerDto.Assignments = null;
                //if (workerDto.AssignmentsId!=null)
                //{
                //    workerDto.Assignments=new List<AssignmentDto>();
                //    foreach (var id in workerDto.AssignmentsId)
                //    {
                //        var assignment = assignments.Result.FirstOrDefault(x => x.AssignmentId== id);
                //        workerDto.Assignments.Add(assignment);
                //    }
                //}

                var workersUrl = $"{baseUrl}api/workers/{workerDto.WorkerId}";
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

                return RedirectToAction("Index",Index());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            using (var client = new HttpClient())
            {
                //Roles
                var roles = deserializeRoleDtos(client);
                ViewBag.Roles = new SelectList(roles.Result, "RoleId", "Name");

                //Projects
                var projects = deserializeProjectDtos(client);
                ViewBag.Projects = new SelectList(projects.Result, "ProjectId", "Name");

                return View(new WorkerDto());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(WorkerDto workerDto)
        {
            using (var client = new HttpClient())
            {
                var workerUrl = $"{baseUrl}api/workers/";
                var workerDtoString = JsonConvert.SerializeObject(workerDto);
                var workerResponse = await client.PostAsync(workerUrl,
                    new StringContent(workerDtoString, Encoding.UTF8, "application/json"));
                if (workerResponse.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", Index());
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Update failed");
                    return View(workerDto);
                }
            }
        }

        private async Task<List<ProjectDto>> deserializeProjectDtos(HttpClient client)
        {
            var projectsUrl = $"{baseUrl}api/projects";
            var projectsResponse = await client.GetStringAsync(projectsUrl);
            var projects = JsonConvert.DeserializeObject<List<ProjectDto>>(projectsResponse);
            return projects;
        }

        private async Task<List<RoleDto>> deserializeRoleDtos(HttpClient client)
        {
            var rolesUrl = $"{baseUrl}api/roles";
            var rolesResponse = await client.GetStringAsync(rolesUrl);
            var roles = JsonConvert.DeserializeObject<List<RoleDto>>(rolesResponse);
            return roles;
        }

        private async Task<List<AssignmentDto>> deserializeAssignmentDtos(HttpClient client)
        {
            var assignmentsUrl = $"{baseUrl}api/assignments";
            var assignmentsResponse = await client.GetStringAsync(assignmentsUrl);
            var assignments = JsonConvert.DeserializeObject<List<AssignmentDto>>(assignmentsResponse);
            return assignments;
        }
    }
}