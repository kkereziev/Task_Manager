using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace Task.Manager.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        
        [StringLength(250)]
        public string Description { get; set; }

        public DateTime AddedOn { get; set; }

        public int? AssignmentId { get; set; }

        public Assignment Assignment { get; set; }
    }
}
