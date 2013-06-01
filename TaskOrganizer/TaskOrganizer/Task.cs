using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskOrganizer
{
    class Task
    {
        public String name;
        public String description;
        public String dateStarted;
        public String dateDue;
        public String status;
        public String priority;
        public String details;

        public Task() { }

        public Task(String name, String description, String dateStarted, String dateDue, String status, String priority, String details)
        {
            this.name = name;
            this.description = description;
            this.dateStarted = dateStarted;
            this.dateDue = dateDue;
            this.status = status;
            this.priority = priority;
            this.details = details;
        }
    }
}
