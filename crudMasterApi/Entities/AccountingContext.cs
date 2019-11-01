using Microsoft.EntityFrameworkCore;

namespace CrudMasterApi.Entities
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
            modelBuilder.Entity<City>().HasMany(e => e.Schools).WithOne(x => x.City).HasForeignKey(x => x.CityId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<School>().HasMany(e => e.Modules).WithOne(x => x.School).HasForeignKey(x => x.SchoolId).OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<ModuleSubject>().HasKey(ms => new { ms.IdModule, ms.IdSubject });
            modelBuilder.Entity<ModuleSubject>().HasOne(x=>x.Subject).WithMany(s => s.ModulesOfSubject).HasForeignKey(ms => ms.IdSubject);
            modelBuilder.Entity<ModuleSubject>().HasOne(s=>s.Module).WithMany(s => s.SubjectsOfModule).HasForeignKey(ms => ms.IdModule);


            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        }
    }
}