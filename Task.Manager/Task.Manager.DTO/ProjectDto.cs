using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Task.Manager.DTO
{
    public class ProjectDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(250)]
        public string Description { get; set; }

        public int[] WorkersIds { get; set; }

        public ICollection<ProjectWorkerDto> Workers { get; set; }

        public ICollection<AssignmentDto> Assignments { get; set; }
    }
}
