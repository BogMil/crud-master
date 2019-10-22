using Microsoft.EntityFrameworkCore;

namespace crudMasterApi.Entities
{
    public class AccountingContext : DbContext
    {
        public AccountingContext()
        {
        }

        public AccountingContext(DbContextOptions<AccountingContext> options)
        : base(options)
        { }

        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<School> Schools { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<ModuleSubject> ModuleSubjects{ get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=AccountingDB;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //var properties = this.GetType().GetProperties();



            CityCreator.OnModelCreating(modelBuilder);
            SchoolCreator.OnModelCreating(modelBuilder);
        }
    }
}
