using System;
using System.Data.Entity.Infrastructure;

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
    }
}