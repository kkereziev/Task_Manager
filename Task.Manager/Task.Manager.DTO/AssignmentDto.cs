using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Task.Manager.DTO
{
    public class AssignmentDto
    {
        public int AssignmentId { get; set; }

        [Required]
        [StringLength(250)]
        public string Title { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        [Required]
        [StringLength(250)]
        public string Status { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public DateTime UpdatedOn { get; set; } = DateTime.Now;

        public int ProjectId { get; set; }
        
        public int? WorkerId { get; set; }
        
        public virtual ProjectDto Project { get; set; }

        public virtual WorkerDto Worker { get; set; }

        public ICollection<CommentDto> Comments { get; set; }
    }
}
