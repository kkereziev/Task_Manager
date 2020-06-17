using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task.Manager.Api.Data;
using Task.Manager.DTO;
using Task.Manager.Entities;

namespace Task.Manager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkersController : ControllerBase
    {
        private readonly ManagerDbContext _context;
        private readonly IMapper _mapper;

        public WorkersController(ManagerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Workers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkerDto>>> GetWorkers()
        {
            var workers=await _context.Workers
                .Include(p=>p.Project)
                .Include(a=>a.Assignments)
                .Include(x=>x.Role)
                .ToListAsync();

            var workersDto = _mapper.Map<List<WorkerDto>>(workers);
            return workersDto;
        }

        // GET: api/Workers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkerDto>> GetWorker(int id)
        {
            var worker = await _context.Workers
                .Include(p => p.Project)
                .Include(a => a.Assignments)
                .Include(x => x.Role)
                .SingleOrDefaultAsync(x=>x.WorkerId==id);

            if (worker == null)
            {
                return NotFound();
            }

            var workerDto = _mapper.Map<WorkerDto>(worker);
            return workerDto;
        }

        // PUT: api/Workers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWorker(int id, WorkerDto workerDto)
        {
            var worker = _mapper.Map<Worker>(workerDto);
            
            if (id != worker.WorkerId)
            {
                return BadRequest();
            }

            _context.Entry(worker).State = EntityState.Modified;
            
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkerExists(id))
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

        // POST: api/Workers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Worker>> PostWorker(WorkerDto workerDto)
        {
            var worker = _mapper.Map<Worker>(workerDto);
            _context.Workers.Add(worker);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWorker", new { id = worker.WorkerId }, worker);
        }

        // DELETE: api/Workers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<WorkerDto>> DeleteWorker(int id)
        {
            var worker = await _context.Workers
                .Include(x => x.Assignments)
                .Include(p => p.Project)
                .Include(r => r.Role)
                .FirstOrDefaultAsync(x => x.WorkerId == id);
            var workersDto = _mapper.Map<WorkerDto>(worker);
            if (worker == null)
            {
                return NotFound();
            }

            _context.Workers.Remove(worker);
            await _context.SaveChangesAsync();
            return workersDto;
        }

        private bool WorkerExists(int id)
        {
            return _context.Workers.Any(e => e.WorkerId == id);
        }
    }
}
