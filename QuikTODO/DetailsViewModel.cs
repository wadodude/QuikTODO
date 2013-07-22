using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuikTODO
{
    public class DetailsViewModel
    {
        public Task Task { get; set; }
        public string OldTaskName { get; set; }
        public string Status
        {
            get { return Task.IsDone ? "Completed" : "Incomplete"; }
        }

        public DetailsViewModel(Task t)
        {
            Task = t;
            OldTaskName = Task.TaskName;
        }
    }
}
