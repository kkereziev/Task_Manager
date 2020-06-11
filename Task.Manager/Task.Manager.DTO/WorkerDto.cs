﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Task.Manager.DTO
{
    public class WorkerDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Email { get; set; }

        [Required]
        public string Status { get; set; }

        public RoleDto Role { get; set; }

        public ICollection<ProjectWorkerDto> Projects { get; set; }

        public ICollection<AssignmentDto> Assignments { get; set; }
    }
}