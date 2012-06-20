using System;
using System.Data;
using System.Data.Entity;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using CodeCamp.Datasource;
using CodeCamp.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeCamp.Tests
{
    /// <summary>
    /// Summary description for CodeCampContextTests
    /// </summary>
    [TestClass]
    public class CodeCampContextTests
    {
        public CodeCampContextTests()
        {
            // Initialize Database and Context
            Database.SetInitializer(new DropCreateDatabaseForTestingWithSeedData());
        }
        
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void Speaker_Seed_Data_Created_Properly()
        {
            using (var context = new CodeCampContext(TestHelpers.TestDatabaseName))
            {
                context.Speakers.Load();

                Assert.IsTrue(context.Speakers.Local.Any());
            }
        }

        [TestMethod]
        public void Using_Local_Data()
        {
            using (var context = new CodeCampContext(TestHelpers.TestDatabaseName))
            {
                // Local data is empty
                Assert.IsFalse(context.Speakers.Local.Any());

                // querying database two speaker records are added to context
                var speakers = context.Speakers.Where(s => s.Id < 3).ToList();

                // speaker records are now available 
                Assert.IsTrue(context.Speakers.Local.Count == 2);

                // another query will add additional records
                var speaker = context.Speakers.SingleOrDefault(s => s.Id == 3);

                Assert.IsTrue(context.Speakers.Local.Count() == 3);
            }
        }

        [TestMethod]
        public void Local_Data_Allows_Queries_Against_New_Records_Not_Saved_To_Database()
        {
            using(var context = new CodeCampContext(TestHelpers.TestDatabaseName))
            {
                var speaker = new Speaker
                                  {
                                      FirstName = "Test",
                                      LastName = "User01",
                                      Email = "testuser01@notarealemail.com"
                                  };
                
                context.Speakers.Add(speaker);

                // query to database will result in a null record
                var query1 = context.Speakers.SingleOrDefault(s => s.Email == "testuser01@notarealemail.com");
                Assert.IsNull(query1);

                // query of local data will return new record
                var query2 = context.Speakers.Local.SingleOrDefault(s => s.Email == "testuser01@notarealemail.com");
                Assert.IsNotNull(query2);
               
            }
        }

        [TestMethod]
        public void Lazy_Loading()
        {
            using(var context = new CodeCampContext(TestHelpers.TestDatabaseName))
            {
                // The following two configuration will disable Lazy Loading
                //context.Configuration.LazyLoadingEnabled = false;
                //context.Configuration.ProxyCreationEnabled = false;

                // lazy loading can only work with dynamic proxies
                // dynamic proxies are only generated for navigation properties
                // that are virtual and public (not sealed)

                // CodeCampEvent Topics collection supports Lazy Loading
                var codeCamp = context.CodeCampEvents.FirstOrDefault();
                var topics = codeCamp.Topics.ToList();

                Assert.IsTrue(topics.Any());
            }
        }

        [TestMethod]
        public void Lazy_Loading_Fails_On_Non_Virtual_Naviation_Properties()
        {
            using (var context = new CodeCampContext(TestHelpers.TestDatabaseName))
            {
                // lazy loading can only work with dynamic proxies
                // dynamic proxies are only generated for navigation properties
                // that are virtual and public (not sealed)

                // Speaker Presentation collection does NOT support Lazy Loading
                var speaker = context.Speakers.FirstOrDefault();
                var presentation = speaker.Presentations.ToList();

                Assert.IsFalse(presentation.Any());
            }
        }

        [TestMethod]
        public void Eager_Loading()
        {
            // Note:
            // Includes can be chained.
            // For collections all records are returned. No filtering.
            using(var context = new CodeCampContext(TestHelpers.TestDatabaseName))
            {
                // To fully hydrate the Code Camp Event object
                var codeCamp =
                    context.CodeCampEvents.Include(c => c.Topics.Select(t => t.Presentation.Speaker)).FirstOrDefault();

                // Topics where returned
                Assert.IsTrue(codeCamp.Topics.Any());

                // Presentation was returned for Topic
                Assert.IsNotNull(codeCamp.Topics.FirstOrDefault().Presentation);

                // Speaker was returned for Presentation
                Assert.IsNotNull(codeCamp.Topics.FirstOrDefault().Presentation.Speaker);

            }
        }

        [TestMethod]
        public void Explicit_Loading()
        {
            // Note:
            // Includes can be chained.
            // For collections all records are returned. No filtering.
            using (var context = new CodeCampContext(TestHelpers.TestDatabaseName))
            {
                // To fully hydrate the Code Camp Event object
                var codeCamp = context.CodeCampEvents.FirstOrDefault();

                // you can check to see if a collection or reference has been loaded
                if (!context.Entry(codeCamp).Collection(c => c.Topics).IsLoaded)
                {
                    context.Entry(codeCamp)
                        .Collection(c => c.Topics).Load();
                }

                // Topics where returned
                Assert.IsTrue(codeCamp.Topics.Any());

                // loading a reference
                var presentation = context.Presentations.FirstOrDefault();
                context.Entry(presentation).Reference(p=>p.Speaker).Load();

                Assert.IsNotNull(presentation.Speaker);
            }
        }

        [TestMethod]
        public void Explicit_Loading_A_Subset()
        {
            // Note:
            // Includes can be chained.
            // For collections all records are returned. No filtering.
            using (var context = new CodeCampContext(TestHelpers.TestDatabaseName))
            {
                // To fully hydrate the Code Camp Event object
                var codeCamp = context.CodeCampEvents.FirstOrDefault();

                var query = context.Entry(codeCamp).Collection(c => c.Topics).Query();

                var generalTrackTopics = from t in query
                                         where t.Track == "General Track"
                                         select t;

                Assert.AreEqual(2, generalTrackTopics.Count());
            }
        }

        [TestMethod]
        public void CRUD()
        {
            // Note: The reason for the seperate using blocks is to simulate seperate calls to database.

            int speakerID;
            
            // Create new speaker
            using(var context = new CodeCampContext(TestHelpers.TestDatabaseName))
            {
                var speaker = new Speaker
                                  {
                                      FirstName = "Test",
                                      LastName = "Speaker"
                                  };

                context.Speakers.Add(speaker);
                context.SaveChanges();
                speakerID = speaker.Id;

                Assert.IsTrue(speakerID != 0);
            }

            // Read and Update speaker
            using(var context = new CodeCampContext(TestHelpers.TestDatabaseName))
            {
                var query = from s in context.Speakers
                            where s.Id == speakerID
                            select s;

                var speaker = query.FirstOrDefault(); 

                Assert.IsNotNull(speaker);

                speaker.Email = "mynewemail@testing.com";
                context.SaveChanges();

                var checkSpeaker = query.FirstOrDefault();

                Assert.AreEqual(speaker.Email, checkSpeaker.Email);
            }

            // Delete: with out pulling record from database first
            using (var context = new CodeCampContext(TestHelpers.TestDatabaseName))
            {
                var speaker = new Speaker {Id = speakerID};
                context.Speakers.Attach(speaker);
                context.Speakers.Remove(speaker);

                // can also be expressed as a single statement
                // context.Entry(speaker).State = EntityState.Deleted;
                
                context.SaveChanges();

                var query = from s in context.Speakers
                            where s.Id == speakerID
                            select s;

                var checkSpeaker = query.FirstOrDefault();

                Assert.IsNull(checkSpeaker);
            }
        }

        [TestMethod]
        public void CRUD_With_Relationships()
        {
            // Add a new presentation through a Speaker's presentation collection.
            using (var context = new CodeCampContext(TestHelpers.TestDatabaseName))
            {
                Presentation presentation = new Presentation
                                                {
                                                    Title = "Another presentation about .NET"
                                                };

                Speaker speaker = context.Speakers.FirstOrDefault(s => s.LastName == "Pugh");

                speaker.Presentations.Add(presentation);

                context.SaveChanges();

                var query = from p in context.Presentations
                            where p.Speaker.LastName == "Pugh"
                            select p;

                Assert.AreEqual(2, query.Count());
            }

            // Add a speaker to a new presentation.
            using (var context = new CodeCampContext(TestHelpers.TestDatabaseName))
            {
                var query = from s in context.Speakers
                            where s.Id == 1
                            select s;

                Speaker speaker = query.FirstOrDefault();

                Presentation presentation = new Presentation
                                                {
                                                    Title = "More about REST",
                                                    Speaker = speaker
                                                };

                context.Presentations.Add(presentation);
                context.SaveChanges();

                // Requery Speaker, this time get presentations.
                speaker = query.Include(s => s.Presentations).FirstOrDefault();

                Assert.AreEqual(2, speaker.Presentations.Count);

                Console.WriteLine("New Presentation added to existing Speaker");
                foreach (var presentation1 in context.Presentations.Local)
                {
                    Console.WriteLine("Speaker: {0}, Presentation: {1}", presentation1.Speaker.FirstName, presentation1.Title);
                }
                Console.WriteLine();

                // Change speaker of presentation
                var speaker2 = context.Speakers.Include(s => s.Presentations).FirstOrDefault(s => s.Id == 8);
                presentation.Speaker = speaker2;

                context.SaveChanges();

                Console.WriteLine("Presenation moved to a different speaker"); 
                foreach (var presentation1 in context.Presentations.Local.OrderBy(p=>p.Speaker.LastName))
                {
                    Console.WriteLine("Speaker: {0}, Presentation: {1}", presentation1.Speaker.FirstName, presentation1.Title);
                }
                Console.WriteLine();

                // Remove Presentation by setting Speaker to null in Presentation record.
                presentation.Speaker = null; // This only works because Speaker is not required. If it was required an exception would be thrown on save.
                
                // Remove Presentation by removing it from the Presentation navigation property in Speaker;
                Presentation presentation2 = speaker2.Presentations.FirstOrDefault();
                speaker2.Presentations.Remove(presentation2);
                
                context.SaveChanges();

                Console.WriteLine("Presenation removed. No longer assigned to any speaker.");
                foreach (var presentation1 in context.Presentations.Local)
                {
                    Console.WriteLine("Speaker: {0}, Presentation: {1}", 
                        presentation1.Speaker != null ? presentation1.Speaker.FirstName : "No Speaker Assigned", 
                        presentation1.Title);
                }
            }
        }

        [TestMethod]
        public void Checking_State()
        {
            using (var context = new CodeCampContext(TestHelpers.TestDatabaseName))
            {
                var speaker = context.Speakers.Find(1);
                
                Console.WriteLine("Before Edit");
                Console.WriteLine("Speaker Email: {0}, State: {1}",speaker.Email, context.Entry(speaker).State);
                Console.WriteLine("After Edit:");
                speaker.Email = "new@email.com";
                Console.WriteLine("Speaker Email: {0}, State: {1}", speaker.Email, context.Entry(speaker).State);
            }
        }

        [TestMethod]
        public void Working_With_Current_Orginal_DatabaseValues()
        {
            using (var context = new CodeCampContext(TestHelpers.TestDatabaseName))
            {
                var speaker = context.Speakers.Find(1);
                var entity = context.Entry(speaker);

                // create a second DbContext to simulate change from another user.
                using (var context2 = new CodeCampContext(TestHelpers.TestDatabaseName))
                {
                    var changeSpeaker = context2.Speakers.Find(1);
                    changeSpeaker.Bio = "Bio has changed while you were out...";
                    context2.SaveChanges();
                }

                Console.WriteLine("Current Values before edit");
                TestHelpers.WritePropertyValues(entity.CurrentValues);

                speaker.Email = "new@email.com";
                speaker.TwitterAlias = "@steve";

                Console.WriteLine("Current Values after edit");
                TestHelpers.WritePropertyValues(entity.CurrentValues);

                Console.WriteLine("Orginal Values");
                TestHelpers.WritePropertyValues(entity.OriginalValues);

                Console.WriteLine("Database Values (trip to database)");
                TestHelpers.WritePropertyValues(entity.GetDatabaseValues());
            }
        }

        [TestMethod]
        public void ComlexTypes_With_Current_Orginal_DatabaseValues()
        {
            using (var context = new CodeCampContext(TestHelpers.TestDatabaseName))
            {
                var codeCamp = context.CodeCampEvents.FirstOrDefault();
                var entity = context.Entry(codeCamp);

                // create a second DbContext to simulate change from another user.
                using (var context2 = new CodeCampContext(TestHelpers.TestDatabaseName))
                {
                    var codeCamp2 = context2.CodeCampEvents.FirstOrDefault();
                    codeCamp2.Location.StreetAddress2 = "Street 2 Changed";
                    context2.SaveChanges();
                }

                Console.WriteLine("Current Values before edit");
                TestHelpers.WritePropertyValues(entity.CurrentValues);

                codeCamp.Location.StreetAddress2 = "Suite 101";
                

                Console.WriteLine("Current Values after edit");
                TestHelpers.WritePropertyValues(entity.CurrentValues);

                Console.WriteLine("Orginal Values");
                TestHelpers.WritePropertyValues(entity.OriginalValues);

                Console.WriteLine("Database Values (trip to database)");
                TestHelpers.WritePropertyValues(entity.GetDatabaseValues());
            }
        }

        [TestMethod]
        public void Attaching_Disconnected_Entities()
        {
            Speaker speaker;

            // Get speaker from one context.
            using (var context = new CodeCampContext(TestHelpers.TestDatabaseName))
            {
                speaker = context.Speakers.FirstOrDefault(s => s.FirstName == "Chip");
                Console.WriteLine("State before edit: {0}", context.Entry(speaker).State);

            }

            // Change speaker out side of context
            speaker.Bio = "Bio has changed";

            //Save changes using a second context.
            using (var context = new CodeCampContext(TestHelpers.TestDatabaseName))
            {
                // Attach speaker
                context.Speakers.Attach(speaker); // speaker will be unchanged becuase it was just added to the context
                Console.WriteLine("State after edit (attached only): {0}", context.Entry(speaker).State);
                
                // must manually change state
                context.Entry(speaker).State = EntityState.Modified; // This line alone will attach and set the state. No need to call Attach
                
                Console.WriteLine("State after edit (after setting state): {0}", context.Entry(speaker).State);
            }
        }

    }
}
