using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using CodeCamp.Datasource;
using CodeCamp.Models;

namespace CodeCamp.Tests
{
    //public class DropCreateDatabaseForTestingWithSeedData : DropCreateDatabaseIfModelChanges<CodeCampContext> 
    public class DropCreateDatabaseForTestingWithSeedData : DropCreateDatabaseAlways<CodeCampContext>
    {
        protected override void Seed(CodeCampContext context)
        {
            //base.Seed(context);

            // Speakers with Presentations
            var steveSuing =
                new Speaker
                    {
                        FirstName = "Steve",
                        LastName = "Suing",
                        Presentations = new Collection<Presentation>(new[]
                                                                         {
                                                                             new Presentation {Title = "Rest API's"}
                                                                         })
                    };

            var mattDuffied =
                new Speaker
                    {
                        FirstName = "Matt",
                        LastName = "Duffield",
                        Presentations = new Collection<Presentation>(new[]
                                                                         {
                                                                             new Presentation
                                                                                 {
                                                                                     Title =
                                                                                         "Building Windows Phone 7 apps with JSON"
                                                                                 }
                                                                         })
                    };

            var kevinHennessy =
                new Speaker
                    {
                        FirstName = "Kevin",
                        LastName = "Hennessy",
                        Presentations = new Collection<Presentation>(new[]
                                                                         {
                                                                             new Presentation
                                                                                 {
                                                                                     Title = "Using Knockout JS"
                                                                                 }
                                                                         })
                    };

            var robZelt =
                new Speaker
                {
                    FirstName = "Rob",
                    LastName = "Zelt",
                    Presentations = new Collection<Presentation>(new[]
                                                                         {
                                                                             new Presentation
                                                                                 {
                                                                                     Title = "WinRT - What Art Thou? A Developers look at Windows 8"
                                                                                 }
                                                                         })
                };

            var jamesDixon =
                new Speaker
                    {
                        FirstName = "James",
                        LastName = "Dixon",
                        Presentations = new Collection<Presentation>(new[]
                                                                         {
                                                                             new Presentation
                                                                                 {
                                                                                     Title =
                                                                                         "Unit testing your voice activated toaster"
                                                                                 }
                                                                         })
                    };

            var chipBurris =
                new Speaker
                    {
                        FirstName = "Chip",
                        LastName = "Burris",
                        Presentations = new Collection<Presentation>(new[]
                                                                         {
                                                                             new Presentation
                                                                                 {
                                                                                     Title =
                                                                                         "Mobile Web APP support in MVC 4"
                                                                                 }
                                                                         })
                    };

            var dianeWilson =
                new Speaker
                    {
                        FirstName = "Diane",
                        LastName = "Wilson",
                        Presentations = new Collection<Presentation>(new[]
                                                                         {
                                                                             new Presentation
                                                                                 {
                                                                                     Title = "Remedial LING"
                                                                                 }
                                                                         })
                    };

            var chrisEargle =
                new Speaker
                    {
                        FirstName = "Chris",
                        LastName = "Eargle",
                        Presentations = new Collection<Presentation>(new[]
                                                                         {
                                                                             new Presentation
                                                                                 {
                                                                                     Title =
                                                                                         "Secrets of a .NET Ninja"
                                                                                 },
                                                                             new Presentation
                                                                                 {
                                                                                     Title =
                                                                                         "The Legend of Lamda"
                                                                                 }
                                                                         })
                    };

            var gregPugh =
                new Speaker
                    {
                        FirstName = "Greg",
                        LastName = "Pugh",
                        Presentations = new Collection<Presentation>(new[]
                                                                         {
                                                                             new Presentation
                                                                                 {
                                                                                     Title = "What ever he dreamed up the night before."
                                                                                 }
                                                                         })
                    };

            context.Speakers.Add(steveSuing);
            context.Speakers.Add(mattDuffied);
            context.Speakers.Add(kevinHennessy);
            context.Speakers.Add(jamesDixon);
            context.Speakers.Add(robZelt);
            context.Speakers.Add(chipBurris);
            context.Speakers.Add(dianeWilson);
            context.Speakers.Add(chrisEargle);
            context.Speakers.Add(gregPugh);
            
            // Code Camp
            var codeCamp =
                new CodeCampEvent
                    {
                        Title = "RDU Code Camp",
                        Location = new Location
                                       {
                                           Name = "ECPI College of Technology",
                                           StreetAddress1 = "4101 Doie Cope Road",
                                           City = "Raleigh",
                                           State = "NC"
                                       },
                        StartDate = new DateTime(2012, 11, 3, 8, 0, 0),
                        EndDate = new DateTime(2012, 11, 3, 17, 30, 0),
                        Topics = new Collection<EventSession>(new[]
                                                                  {
                                                                      new EventSession
                                                                          {
                                                                              StartTime =
                                                                                  new DateTime(2012, 11, 3, 9, 30, 0),
                                                                              Room = "206",
                                                                              Track = "Web Development",
                                                                              SessionType = SessionType.Presentation,
                                                                              Presentation =
                                                                                  steveSuing.Presentations.
                                                                                  ElementAtOrDefault(0)
                                                                          },
                                                                      new EventSession
                                                                          {
                                                                              StartTime =
                                                                                  new DateTime(2012, 11, 3, 9, 30, 0),
                                                                              Room = "223",
                                                                              Track = "General Track",
                                                                              SessionType = SessionType.Presentation,
                                                                              Presentation =
                                                                                  chrisEargle.Presentations.
                                                                                  ElementAtOrDefault(0)
                                                                          },
                                                                      new EventSession
                                                                          {
                                                                              StartTime =
                                                                                  new DateTime(2012, 11, 3, 14, 30, 0),
                                                                              Room = "230",
                                                                              Track = "General Track",
                                                                              SessionType = SessionType.Presentation,
                                                                              Presentation =
                                                                                  chrisEargle.Presentations.
                                                                                  ElementAtOrDefault(1)
                                                                          },
                                                                  })

                    };

            context.CodeCampEvents.Add(codeCamp);


        }
    }
}