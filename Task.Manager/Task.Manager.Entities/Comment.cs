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
        public int CommentId { get; set; }
        
        [StringLength(250)]
        public string Description { get; set; }

        public DateTime AddedOn { get; set; }=DateTime.Now;

        public int? AssignmentId { get; set; }

        public int? WorkerId { get; set; }

        public virtual Assignment Assignment { get; set; }
        
        public virtual Worker Worker { get; set; }
    }
}
