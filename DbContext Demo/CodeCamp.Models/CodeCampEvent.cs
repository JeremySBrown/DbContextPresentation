using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CodeCamp.Models
{
    public class CodeCampEvent : IValidatableObject 
    {
        public CodeCampEvent()
        {
            Topics = new HashSet<EventSession>();
            Location = new Location();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Location Location { get; set; }

        public virtual ICollection<EventSession> Topics { get; set; }

        #region Implementation of IValidatableObject

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Start and End Dates
            if (StartDate >= EndDate)
            {
                yield return
                    new ValidationResult("Start Date must be ealier than the End Date.", new[] {"StartDate", "EndDate"})
                    ;
            }

            // NC Only Rule
            if (Location.State != "NC")
            {
                yield return new ValidationResult("Code Camps can only be held in North Carolina",new []{"Location.State"});
            }
        }

        #endregion
    }
}