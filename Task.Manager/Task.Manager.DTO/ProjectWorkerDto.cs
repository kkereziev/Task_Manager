using System;
using System.Collections.Generic;
using System.Text;

namespace Task.Manager.DTO
{
    public class ProjectWorkerDto
    {
        public ProjectDto Project { get; set; }

        public WorkerDto Worker { get; set; }
    }
}
