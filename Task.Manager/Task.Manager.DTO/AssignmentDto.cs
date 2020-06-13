using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Task.Manager.DTO
{
    public class AssignmentDto
    {
        public int Id { get; set; }

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

        public ProjectDto Project { get; set; }

        public ICollection<CommentDto> Comments { get; set; }
    }
}
