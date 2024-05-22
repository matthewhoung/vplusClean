using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Tasks
{
    public class WorkDay
    {
        public int WorkDayId { get; set; }
        public int TaskId { get; set; }
        public int? SubTaskId { get; set; }
        public DateTime WorkDate { get; set; }
    }
}
