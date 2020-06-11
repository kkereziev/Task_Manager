using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task.Manager.Entities
{
    public class ProjectWorker
    {
        public int ProjectId { get; set; }

        public int WorkerId { get; set; }

        public Project Project { get; set; }

        public Worker Worker { get; set; }

    }
}
