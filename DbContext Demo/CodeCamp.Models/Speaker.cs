using System;
using System.Collections.Generic;

namespace CodeCamp.Models
{
    public class Speaker
    {
        public Speaker()
        {
            Presentations = new HashSet<Presentation>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string TwitterAlias { get; set; }
        public string BlogUrl { get; set; }
        public string Bio { get; set; }

        public ICollection<Presentation> Presentations { get; set; }
    }
}