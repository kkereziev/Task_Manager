using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Task.Manager.DTO
{
    public class CommentDto
    {
        public int CommentId { get; set; }

        [StringLength(250)]
        public string Description { get; set; }
        
        public DateTime AddedOn { get; set; }

        public virtual AssignmentDto Assignment { get; set; }
    }
}
