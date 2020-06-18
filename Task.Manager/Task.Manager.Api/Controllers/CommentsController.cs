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
    public class CommentsController : ControllerBase
    {
        private readonly ManagerDbContext _context;
        private readonly IMapper _mapper;

        public CommentsController(ManagerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Comments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetComments()
        {
            var comments = await _context.Comments
                .Include(p => p.Assignment)
                .Include(p => p.Worker)
                .ToListAsync();

            var commentsDto = _mapper.Map<List<CommentDto>>(comments);
            return commentsDto;
        }

        // GET: api/Comments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CommentDto>> GetComment(int id)
        {
            var comment = await _context.Comments
                .Include(x => x.Assignment)
                .Include(p => p.Worker)
                .SingleOrDefaultAsync(x => x.CommentId == id);

            if (comment == null)
            {
                return NotFound();
            }

            var commentDto = _mapper.Map<CommentDto>(comment);
            return commentDto;
        }

        [HttpPost]
        public async Task<ActionResult<Comment>> PostComment(CommentDto commentDto)
        {
            var comment = _mapper.Map<Comment>(commentDto);
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetComment", new { id = comment.CommentId }, comment);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CommentDto>> DeleteComment(int id)
        {
            var comment = await _context.Comments
                .FirstOrDefaultAsync(x=>x.CommentId==id);
            
            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            var commentDto = _mapper.Map<CommentDto>(comment);
            return commentDto;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment(int id, CommentDto commentDto)
        {
            var comment = _mapper.Map<Comment>(commentDto);

            if (id != comment.CommentId)
            {
                return BadRequest();
            }

            _context.Entry(comment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
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

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.CommentId == id);
        }
    }
}