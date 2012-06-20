using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using CodeCamp.Datasource;

namespace CodeCamp.Tests
{
    public static class TestHelpers
    {
        public const string TestDatabaseName = "CodeCampTestDatabase";

        public static void WritePropertyValues(DbPropertyValues values, int indent = 1)
        {
            foreach (string propertyName in values.PropertyNames)
            {
                var value = values[propertyName];
                if (value is DbPropertyValues)
                {
                    Console.WriteLine("{0} - Complex Property: {1}",
                                      string.Empty.PadLeft(indent),
                                      propertyName);
                    WritePropertyValues((DbPropertyValues)value, indent + 1);
                }
                else
                {
                    Console.WriteLine("{0} - {1}: {2}",
                        string.Empty.PadLeft(indent),
                        propertyName,values[propertyName]);
                }
            }
        }

        public static void WriteValiationResults(DbContext context)
        {
            foreach (DbEntityValidationResult dbEntityValidationResult in context.GetValidationErrors())
            {
                WriteValiationResults(dbEntityValidationResult);
                Console.WriteLine();
            }
        }

        public static void WriteValiationResults(DbContext context, object model)
        {
            WriteValiationResults(context.Entry(model).GetValidationResult());
        }

        public static void WriteValiationResults(DbEntityValidationResult result)
        {
            Console.WriteLine("Type: {0}", result.Entry.Entity.GetType().Name);
            Console.WriteLine("Passed Validation: {0}",result.IsValid);

            foreach (DbValidationError dbValidationError in result.ValidationErrors)
            {
                Console.WriteLine("{0}: {1}", dbValidationError.PropertyName, dbValidationError.ErrorMessage);
            }
        }

        public static void WritePropertyValidationResults(DbContext context, object model, params string[] propertyNames)
        {
            var entity = context.Entry(model);
            Console.WriteLine("Type: {0}", model.GetType().Name);
            foreach (string propertyName in propertyNames)
            {
                Console.WriteLine("\nProperty: {0}", propertyName);
                Console.WriteLine("Value: {0}", entity.Property(propertyName).CurrentValue);
                var results = entity.Property(propertyName).GetValidationErrors();
                Console.WriteLine("Passed Validation: {0}",!results.Any());
                foreach (DbValidationError dbValidationError in results)
                {
                    Console.WriteLine("  - {0}", dbValidationError.ErrorMessage);
                }
            }
        }

    }
}