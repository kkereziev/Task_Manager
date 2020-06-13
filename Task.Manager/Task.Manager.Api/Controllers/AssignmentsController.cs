using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public AssignmentsController(ManagerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Assignments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssignmentDto>>> GetAssignments()
        {
            var assignments = await _context.Assignments
                .Include(p=>p.Project)
                .Include(c=>c.Comments)
                .ToListAsync();

            var assignemntsDto = _mapper.Map<List<AssignmentDto>>(assignments);
            
            return assignemntsDto;
        }

        // GET: api/Assignments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AssignmentDto>> GetAssignment(int id)
        {
            var assignment = await _context.Assignments
                .Include(p => p.Project)
                .Include(c => c.Comments)
                .Include(w=>w.Worker)
                .SingleOrDefaultAsync(x=>x.Id==id);

            if (assignment == null)
            {
                return NotFound();
            }

            var assignemntDto = _mapper.Map<AssignmentDto>(assignment);

            return assignemntDto;
        }

        // PUT: api/Assignments/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAssignment(int id, AssignmentDto assignmentDto)
        {
            var assignment = _mapper.Map<Assignment>(assignmentDto);
            
            if (id != assignment.Id)
            {
                return BadRequest();
            }

            assignment.ProjectId = assignment.Project.Id;
            assignment.WorkerId = assignment.Worker.Id;
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
        public async Task<ActionResult<Assignment>> PostAssignment(AssignmentDto assignmentDto)
        {
            var assignment = _mapper.Map<Assignment>(assignmentDto);
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
