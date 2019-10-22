using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace crudMasterApi.Entities
{
    public class Subject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(200)]
        public string Name { get; set; }

        public virtual ICollection<ModuleSubject> ModulesOfSubject { get; set; }
    }

    public static class SubjectCreator
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subject>().HasKey(e => e.Id);
            modelBuilder.Entity<Subject>().Property(e => e.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<Subject>().HasData(
                new Subject
                {
                    Id = 1,
                    Name = "Electrotehnics",
                },
                new Subject
                {
                    Id = 2,
                    Name = "Physics",
                },
                new Subject
                {
                    Id = 3,
                    Name = "English",
                },
                new Subject
                {
                    Id = 4,
                    Name = "Maths",
                }
            );
        }
    }
}
