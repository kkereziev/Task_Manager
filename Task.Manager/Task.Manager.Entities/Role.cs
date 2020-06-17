using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Task.Manager.Entities
{
    public class Role
    {
        public int RoleId { get; set; }
        [Required]
        public string Name { get; set; }

        public ICollection<Worker> Workers { get; set; }
    }
}
