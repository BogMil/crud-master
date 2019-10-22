using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace crudMasterApi.Entities
{
    public class School
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(200)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Mail { get; set; }
        public int CityId { get; set; }
        public virtual City City { get; set; }
    }

    public static class SchoolCreator
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<School>().HasKey(e => e.Id);
            modelBuilder.Entity<School>().Property(e => e.Id).ValueGeneratedOnAdd();
            //modelBuilder.Entity<School>().HasOne<City>();


            modelBuilder.Entity<School>().HasData(
                new School
                {
                    Id = 1,
                    Name = "Gimnazija i ekonomska škola Branko Radičević",
                    CityId = 1,
                    Mail = "gimeko@yahoo.com",
                },
                new School
                {
                    Id = 2,
                    Name = "Gimnazija i ekonomska škola Branko Radičević",
                    CityId = 1,
                    Mail = "gimeko@yahoo.com",
                },
                new School
                {
                    Id = 3,
                    Name = "Gimnazija i ekonomska škola Branko Radičević",
                    CityId = 2,
                    Mail = "gimeko@yahoo.com",
                },
                new School
                {
                    Id = 4,
                    Name = "Gimnazija i ekonomska škola Branko Radičević",
                    CityId = 3,
                    Mail = "gimeko@yahoo.com",
                },
                new School
                {
                    Id = 5,
                    Name = "Srednja stručna škola Mihajlo Pupin",
                    CityId = 1,
                }
            );
        }
    }
}
