using System;

namespace CodeCamp.Models
{
    public enum SessionType
    {
        Presentation,
        Registration,
        Keynote,
        Lunch,
        Closing
    }

    public class EventSession
    {
        public EventSession()
        {
            Duration = new TimeSpan(0,1,15,0);
            SessionType = SessionType.Presentation;
        }

        public int Id { get; set; }
        public SessionType SessionType { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public string Track { get; set; }
        public string Room { get; set; }
        public Presentation Presentation { get; set; }
    }
}