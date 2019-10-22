using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace crudMasterApi.Entities
{
    public class ModuleSubject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(200)]
        public int IdSubject{ get; set; }
        public virtual Subject Subject { get; set; }
        public int IdModule { get; set; }
        public virtual Module Module { get; set; }
    }

    public static class ModuleSubjectCreator
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ModuleSubject>().HasKey(e => e.Id);
            modelBuilder.Entity<ModuleSubject>().Property(e => e.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<ModuleSubject>().HasKey(ms => new {ms.IdModule, ms.IdSubject});

            modelBuilder.Entity<ModuleSubject>().HasOne<Subject>().WithMany(s=>s.ModulesOfSubject).HasForeignKey(ms => ms.IdSubject);
            modelBuilder.Entity<ModuleSubject>().HasOne<Module>().WithMany(s=>s.SubjectsOfModule).HasForeignKey(ms=>ms.IdModule);

            modelBuilder.Entity<ModuleSubject>().HasData(
                new ModuleSubject
                {
                    Id = 1,
                    IdSubject = 1,
                    IdModule = 2
                },
                new ModuleSubject
                {
                    Id = 2,
                    IdSubject = 1,
                    IdModule = 1
                },
                new ModuleSubject
                {
                    IdSubject = 1,
                    IdModule = 3
                },
                new ModuleSubject
                {
                    IdSubject = 3,
                    IdModule = 3
                }
            );
        }
    }
}
