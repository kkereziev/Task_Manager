using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Task.Manager.DTO
{
    public class RoleDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public ICollection<WorkerDto> Workers { get; set; }
    }
}
