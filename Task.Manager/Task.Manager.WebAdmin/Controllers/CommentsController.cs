using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Task.Manager.DTO;

namespace Task.Manager.WebAdmin.Controllers
{
    public class CommentsController : Controller
    {
        private string baseUrl = "https://localhost:44358/";

        [HttpGet]
        public async Task<IActionResult> Edit(int id, int assignmentId, int workerId)
        {
            using (var client = new HttpClient())
            {
                //Assignment
                var commentsUrl = $"{baseUrl}api/comments/{id}";
                var commentsResponse = await client.GetStringAsync(commentsUrl);
                var comments = JsonConvert.DeserializeObject<CommentDto>(commentsResponse);


                var assignments = deserializeAssignmentDtos(client);
                var assignment = new List<AssignmentDto>();
                assignment.Add(assignments.Result.FirstOrDefault(x => x.AssignmentId == assignmentId));
                ViewBag.Assignments = new SelectList(assignment, "AssignmentId", "Title");

                var workers = deserializeWorkerDtos(client);
                var worker = new List<WorkerDto>();
                worker.Add(workers.Result.FirstOrDefault(x => x.WorkerId == workerId));
                ViewBag.Workers = new SelectList(worker, "WorkerId", "Name");

                return View(comments);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CommentDto commentDto)
        {
            using (var client = new HttpClient())
            {
                commentDto.AddedOn=DateTime.Now;
                var commentsUrl = $"{baseUrl}api/comments/{commentDto.CommentId}";
                var commentDtoString = JsonConvert.SerializeObject(commentDto);
                var commentsResponse = await client.PutAsync(commentsUrl, new StringContent(commentDtoString, Encoding.UTF8, "application/json"));

                if (commentsResponse.IsSuccessStatusCode)
                {
                    commentDto =
                        JsonConvert.DeserializeObject<CommentDto>(
                            await commentsResponse.Content.ReadAsStringAsync());
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Update failed");
                }

                return RedirectToAction("Index","Projects");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create(int assignmentId, int projectId)
        {
            using (var client = new HttpClient())
            {
                var assignments = deserializeAssignmentDtos(client);
                var assignment = new List<AssignmentDto>();
                assignment.Add(assignments.Result.FirstOrDefault(x=>x.AssignmentId== assignmentId));
                ViewBag.Assignments = new SelectList(assignment, "AssignmentId", "Title");
                
                var projects = deserializeProjectDtos(client);
                var workers=new List<WorkerDto>();
                var project = projects.Result.FirstOrDefault(x => x.ProjectId == projectId);
                workers.AddRange(project.Workers);
                ViewBag.Workers = new SelectList(workers, "WorkerId", "Name");
                
                return View(new CommentDto());
            }
        }

        [HttpPost]
        //POST: Comments
        public async Task<IActionResult> Create(CommentDto commentDto)
        {
            using (var client = new HttpClient())
            {
                commentDto.AddedOn=DateTime.Now;
                var commentUrl = $"{baseUrl}api/comments/";
                var commentDtoString = JsonConvert.SerializeObject(commentDto);
                var commentResponse = await client.PostAsync(commentUrl,
                    new StringContent(commentDtoString, Encoding.UTF8, "application/json"));
                var id = commentDto.AssignmentId;
                if (commentResponse.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Projects");

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Update failed");
                    return View(commentDto);
                }
            }
        }
        public async Task<IActionResult> Delete(int id)
        {
            using (var client = new HttpClient())
            {
                var commentUrl = $"{baseUrl}api/comments/{id}";
                var commentsResponse = await client.DeleteAsync(commentUrl);

                var result = commentsResponse;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index","Projects");
                }
            }

            return RedirectToAction("Index", "Projects");
        }

        private async Task<List<AssignmentDto>> deserializeAssignmentDtos(HttpClient client)
        {
            var assignmentsUrl = $"{baseUrl}api/assignments";
            var assignmentsResponse = await client.GetStringAsync(assignmentsUrl);
            var assignments = JsonConvert.DeserializeObject<List<AssignmentDto>>(assignmentsResponse);
            
            return assignments;
        }

        private async Task<List<WorkerDto>> deserializeWorkerDtos(HttpClient client)
        {
            var workersUrl = $"{baseUrl}api/workers";
            var workersResponse = await client.GetStringAsync(workersUrl);
            var workers = JsonConvert.DeserializeObject<List<WorkerDto>>(workersResponse);
            
            return workers;
        }

        private async Task<List<ProjectDto>> deserializeProjectDtos(HttpClient client)
        {
            var projectsUrl = $"{baseUrl}api/projects";
            var projectsResponse = await client.GetStringAsync(projectsUrl);
            var projects = JsonConvert.DeserializeObject<List<ProjectDto>>(projectsResponse);
            return projects;
        }
    }
}