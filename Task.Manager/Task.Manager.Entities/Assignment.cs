using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Task.Manager.Entities
{
    public class Assignment
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

        public int ProjectId { get; set; }

        public int WorkerId { get; set; }

        public Project Project { get; set; }

        public Worker Worker { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
