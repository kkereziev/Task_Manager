using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace Task.Manager.Entities
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(250)]
        public string Description { get; set; }

        public ICollection<Worker> Workers { get; set; }

        public ICollection<Assignment> Assignments { get; set; }
    }
}
