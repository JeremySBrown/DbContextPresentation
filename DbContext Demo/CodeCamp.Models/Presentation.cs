using System;
using System.ComponentModel.DataAnnotations;

namespace CodeCamp.Models
{
    public class Presentation
    {
        public int ID { get; set; }

        [Required]
        public string Title { get; set; }
        public string Abstract { get; set; }
        public string SlideDeckUrl { get; set; }
        public string CodeSampleUrl { get; set; }

        public Speaker Speaker { get; set; }

    }
}