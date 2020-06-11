using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task.Manager.Entities;
using Task.Manager.Api.Data;
using Task.Manager.DTO;

namespace Task.Manager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentsController : ControllerBase
    {
        private readonly ManagerDbContext _context;

        public AssignmentsController(ManagerDbContext context)
        {
            _context = context;
        }

        // GET: api/Assignments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Assignment>>> GetAssignments()
        {
            return await _context.Assignments
                .Include(p=>p.Project)
                .Include(c=>c.Comments)
                .ToListAsync();
        }

        // GET: api/Assignments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AssignmentDto>> GetAssignment(int id)
        {
            var assignment = await _context.Assignments
                .Include(p => p.Project)
                .Include(c => c.Comments)
                .SingleOrDefaultAsync(x=>x.Id==id);

            if (assignment == null)
            {
                return NotFound();
            }

            var assignemntDto = new AssignmentDto()
            {
                Id = assignment.Id,
                Title = assignment.Title,
                Status = assignment.Status,
                Description = assignment.Description,
                CreatedOn = assignment.CreatedOn,
                UpdatedOn = assignment.UpdatedOn,
                Project = new ProjectDto()
                {
                    Id = assignment.Project.Id,
                    Name = assignment.Project.Name,
                },
                Worker = new WorkerDto()
                {
                    Id = assignment.Worker.Id,
                    Name = assignment.Worker.Name,
                    Email = assignment.Worker.Email,
                    Status = assignment.Worker.Status
                },
                Comments = assignment.Comments.Select(x=>new CommentDto()
                {
                    Description = x.Description,
                    Assignment =  new AssignmentDto()
                    {
                        Id = x.Assignment.Id,
                        Description = x.Assignment.Description,
                        Title = x.Assignment.Title,
                        Status = x.Assignment.Status
                    }
                }).ToList(),

            };

            return assignemntDto;
        }

        // PUT: api/Assignments/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAssignment(int id, Assignment assignment)
        {
            if (id != assignment.Id)
            {
                return BadRequest();
            }

            _context.Entry(assignment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssignmentExists(id))
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

        // POST: api/Assignments
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Assignment>> PostAssignment(Assignment assignment)
        {
            _context.Assignments.Add(assignment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAssignment", new { id = assignment.Id }, assignment);
        }

        // DELETE: api/Assignments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Assignment>> DeleteAssignment(int id)
        {
            var assignment = await _context.Assignments.FindAsync(id);
            if (assignment == null)
            {
                return NotFound();
            }

            _context.Assignments.Remove(assignment);
            await _context.SaveChangesAsync();

            return assignment;
        }

        private bool AssignmentExists(int id)
        {
            return _context.Assignments.Any(e => e.Id == id);
        }
    }
}
