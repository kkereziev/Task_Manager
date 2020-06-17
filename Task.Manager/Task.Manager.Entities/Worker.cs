using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Task.Manager.Entities
{
    public class Worker
    {
        public int WorkerId { get; set; }
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        
        [StringLength(250)]
        public string Email { get; set; }
        
        [Required]
        public string Status { get; set; }
        
        public int? RoleId { get; set; }

        public int? ProjectId { get; set; }

        public virtual Role Role { get; set; }

        public virtual Project Project { get; set; }

        public ICollection<Assignment> Assignments { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
