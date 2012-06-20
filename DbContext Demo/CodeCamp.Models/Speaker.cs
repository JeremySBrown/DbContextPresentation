using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CodeCamp.Models
{
    public class Speaker
    {
        public Speaker()
        {
            Presentations = new HashSet<Presentation>();
        }

        public int Id { get; set; }

        [StringLength(50)]
        [Required]
        public string FirstName { get; set; }

        [StringLength(50)]
        [Required]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?",
            ErrorMessage = "Please provide a valid email address.")]
        public string Email { get; set; }
        public string TwitterAlias { get; set; }
        public string BlogUrl { get; set; }
        public string Bio { get; set; }

        public ICollection<Presentation> Presentations { get; set; }
    }
}