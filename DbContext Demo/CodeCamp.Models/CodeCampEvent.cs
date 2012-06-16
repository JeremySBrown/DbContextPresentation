using System;
using System.Collections.Generic;

namespace CodeCamp.Models
{
    public class CodeCampEvent
    {
        public CodeCampEvent()
        {
            Topics = new HashSet<EventSession>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Location Location { get; set; }

        public ICollection<EventSession> Topics { get; set; }
    }
}