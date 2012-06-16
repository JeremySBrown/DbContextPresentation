using System;

namespace CodeCamp.Models
{
    public class Presentation
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Abstract { get; set; }
        public string SlideDeckUrl { get; set; }
        public string CodeSampleUrl { get; set; }

        public Speaker Speaker { get; set; }

    }
}