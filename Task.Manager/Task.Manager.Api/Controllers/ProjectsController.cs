using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Task.Manager.Api.Data;
using Task.Manager.DTO;
using Task.Manager.Entities;


namespace Task.Manager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ManagerDbContext _context;
        private readonly IMapper _mapper;

        public ProjectsController(ManagerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjects()
        {
            var projects=await _context.Projects
                .Include(w=>w.Workers)
                .Include(a=>a.Assignments)
                .ToListAsync();

            var projectsDto = _mapper.Map<List<ProjectDto>>(projects);

            return projectsDto;
        }

        // GET: api/Projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDto>> GetProject(int id)
        {
            var project = await _context.Projects
                .Include(w=>w.Workers)
                .ThenInclude(r=>r.Role)
                .Include(a=>a.Assignments)
                .SingleOrDefaultAsync(x=>x.ProjectId==id);

            if (project == null)
            {
                return NotFound();
            }
            var projectDto = _mapper.Map<ProjectDto>(project);

            return projectDto;
        }

        // PUT: api/Projects/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(int id, ProjectDto project)
        {
            using (var client=new HttpClient())
            {
                //Assignments
                var assignmentsUrl = $"https://localhost:44358/api/assignments";
                var assignmentsResponse = await client.GetStringAsync(assignmentsUrl);
                var assignments = JsonConvert.DeserializeObject<List<AssignmentDto>>(assignmentsResponse);

                //Workers
                var workersUrl = $"https://localhost:44358/api/workers";
                var workersResponse = await client.GetStringAsync(workersUrl);
                var workers = JsonConvert.DeserializeObject<List<WorkerDto>>(workersResponse);

                project.Assignments = new List<AssignmentDto>();
                project.Workers = new List<WorkerDto>();
                foreach (var assignmentId in project.AssignmentIds)
                {
                    project.Assignments.Add(assignments.Find(x=>x.AssignmentId==assignmentId));
                }

                foreach (var workerId in project.WorkerIds)
                {
                    project.Workers.Add(workers.Find(x => x.WorkerId == workerId));
                }
            }
            
            if (id != project.ProjectId)
            {
                return BadRequest();
            }
            
            _context.Entry(project).State = EntityState.Modified;

           
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Projects
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Project>> PostProject(ProjectDto projectDto)
        {
            var project = _mapper.Map<Project>(projectDto);
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

          
            return CreatedAtAction("GetProject", new { id = project.ProjectId}, project);
        }

        // DELETE: api/Projects/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Project>> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return project;
        }

        

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.ProjectId== id);
        }
    }
}
