using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Task.Manager.Entities
{
    public class Worker
    {
        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        
        [StringLength(250)]
        public string Email { get; set; }
        
        [Required]
        public string Status { get; set; }
        
        public int RoleId { get; set; }

        public Role Role { get; set; }

        public ICollection<ProjectWorker> Projects { get; set; }

        public ICollection<Assignment> Assignments { get; set; }
    }
}
