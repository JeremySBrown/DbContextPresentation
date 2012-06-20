using System;
using System.Collections.ObjectModel;
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
    /// Summary description for ValidationTests
    /// </summary>
    [TestClass]
    public class ValidationTests
    {
        public ValidationTests()
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
        public void Vaidate_On_Demand_Fails()
        {
            using (var context = new CodeCampContext(TestHelpers.TestDatabaseName))
            {
                Speaker speaker = new Speaker();

                var validationResult = context.Entry(speaker).GetValidationResult().IsValid;
                
                TestHelpers.WriteValiationResults(context, speaker);
                //Console.WriteLine("Speaker is {0}",
                //                  validationResult ? "Valid" : "Invalid");

                Assert.IsFalse(validationResult);

            }
        }

        [TestMethod]
        public void Vaidate_On_Demand_Passes()
        {
            using (var context = new CodeCampContext(TestHelpers.TestDatabaseName))
            {
                Speaker speaker = new Speaker
                                      {
                                          FirstName = "Test",
                                          LastName = "Speaker",
                                          Email = "test@speaker.com"
                                      };

                var validationResult = context.Entry(speaker).GetValidationResult().IsValid;

                Console.WriteLine("Speaker is {0}",
                                  validationResult ? "Valid" : "Invalid");

                Assert.IsTrue(validationResult);
            }
        }

        [TestMethod]
        public void Validate_Invdividual_Properties()
        {
            using (var context = new CodeCampContext(TestHelpers.TestDatabaseName))
            {
                Speaker speaker = new Speaker();

                Console.WriteLine("Email validation check 1");
                TestHelpers.WritePropertyValidationResults(context,speaker,"Email");


                Console.WriteLine("\nEmail validation check 2");
                speaker.Email = "bademail.com";
                TestHelpers.WritePropertyValidationResults(context, speaker, "Email");

                Console.WriteLine("\nEmail validation check 3");
                speaker.Email = "goodemail@test.com";
                TestHelpers.WritePropertyValidationResults(context, speaker, "Email","FirstName");
            }
        }

        [TestMethod]
        public void Validate_With_IValidatableObject()
        {
            using (var context = new CodeCampContext(TestHelpers.TestDatabaseName))
            {
                // IValidatableObject Validate method is not called if validation attributes fail first.
                CodeCampEvent codeCamp = new CodeCampEvent();
                Console.WriteLine("Code Camp Valdation check 1");
                TestHelpers.WriteValiationResults(context, codeCamp);

                // IValidatableObject Validate method is called because validation attributes passed.
                Console.WriteLine("\nCode Camp Valdation check 2");
                codeCamp.Location.Name = "Event Name";
                TestHelpers.WriteValiationResults(context, codeCamp);

                Console.WriteLine("\nCode Camp Valdation check 3");
                codeCamp.StartDate = DateTime.Today;
                codeCamp.EndDate = DateTime.Today.AddDays(1);
                codeCamp.Location.State = "NC";
                TestHelpers.WriteValiationResults(context, codeCamp);

            }
        }

        [TestMethod]
        public void Validate_Multiple_Entities()
        {
            using (var context = new CodeCampContext(TestHelpers.TestDatabaseName))
            {
                Speaker speaker = new Speaker
                                      {
                                          FirstName = "Test",
                                          Presentations = new Collection<Presentation>(new[] {new Presentation()})
                                      };

                Presentation presentation = new Presentation {Speaker = new Speaker()};
                
                CodeCampEvent codeCamp = new CodeCampEvent
                                             {
                                                 Topics = new Collection<EventSession>(new []{new EventSession{Presentation = presentation} })
                                             };

                context.Speakers.Add(speaker);

                context.CodeCampEvents.Add(codeCamp);

                TestHelpers.WriteValiationResults(context);
            }
        }
    }
}
