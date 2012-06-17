using System;
using System.Data.Entity;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using CodeCamp.Datasource;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeCamp.Tests
{
    /// <summary>
    /// Summary description for LocalDataTests
    /// </summary>
    [TestClass]
    public class LocalDataTests
    {
        public LocalDataTests()
        {
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
        public void Using_Local_Data()
        {
            using(CodeCampContext context = new CodeCampContext())
            {
                Assert.IsFalse(context.Speakers.Local.Any());

                var speakers = context.Speakers.Where(s => s.Id < 3).ToList();

                Assert.IsTrue(context.Speakers.Local.Count == 2);
            }
        }
    }
}
