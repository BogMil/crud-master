using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace crudMasterApi.Entities
{
    public class Module
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(200)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Principal { get; set; }
        public int SchoolId { get; set; }
        public virtual School School { get; set; }

        public virtual ICollection<ModuleSubject> SubjectsOfModule { get; set; }
    }

    public static class ModuleCreator
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Module>().HasKey(e => e.Id);
            modelBuilder.Entity<Module>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Module>().HasOne<School>();

            modelBuilder.Entity<Module>().HasData(
                new Module
                {
                    Id = 1,
                    Name = "Electrotehnical engineering",
                    Principal = "Jane Doe",
                    SchoolId = 1
                },
                new Module
                {
                    Id = 2,
                    Name = "Electrotehnical engineering",
                    Principal = "John Doe",
                    SchoolId = 2
                },
                new Module
                {
                    Id = 3,
                    Name = "Electrotehnical engineering",
                    Principal = "Marc Skimet",
                    SchoolId = 3
                },
                new Module
                {
                    Id = 4,
                    Name = "Electrotehnical engineering",
                    Principal = "Johny Noxvile",
                    SchoolId = 1
                },
                new Module
                {
                    Id = 5,
                    Name = "Electrotehnical engineering",
                    Principal = "Partic Star",
                    SchoolId = 1
                }
            );
        }
    }
}
