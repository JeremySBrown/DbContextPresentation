using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using CodeCamp.Models;

namespace CodeCamp.Datasource
{
    public class CodeCampContext : DbContext
    {
        public CodeCampContext():base()
        {
            
        }

        /// <summary>
        /// Constructor for CodeCampContext
        /// </summary>
        /// <param name="nameOrConnectionString">Database name or Connection string name. </param>
        /// <remarks>Use a prefix of name= to force context to look for a connection string by the supplied name</remarks>
        public CodeCampContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            
        }

        public ObjectContext ObjectContext
        {
            get { return ((IObjectContextAdapter)this).ObjectContext; }
        }


        public DbSet<Speaker> Speakers { get; set; }
        public DbSet<Presentation> Presentations { get; set; }
        public DbSet<CodeCampEvent> CodeCampEvents { get; set; }

    }
}