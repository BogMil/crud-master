using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace crudMasterApi.Entities
{
    public class City
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(30)]
        public string Name { get; set; }
        [StringLength(6)]
        public string PostalCode { get; set; }
        public virtual ICollection<School> Schools { get; set; }
    }

    public static class CityCreator
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>().HasKey(e => e.Id);
            modelBuilder.Entity<City>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<City>().HasMany(e => e.Schools).WithOne(x=>x.City).HasForeignKey(x=>x.CityId);
                
            //.WithMany(s => s.Cities).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<City>().HasData(
                new City { Name = "Kovin", PostalCode = "26220", Id = 1 },
                new City { Name = "Beograd", PostalCode = "11000", Id = 2 },
                new City { Name = "Pančevo", PostalCode = "26000", Id = 3 },
                new City { Name = "Novi Sad", PostalCode = "21000", Id = 4},
                new City { Name = "Beograd", PostalCode = "11000", Id = 5}
            );
        }
    }
}
