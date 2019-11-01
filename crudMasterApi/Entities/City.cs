using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrudMasterApi.Entities
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

    public class CityModelCreator : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.HasData(
                new City { Name = "Kovin", PostalCode = "26220", Id = 1 },
                new City { Name = "Beograd", PostalCode = "11000", Id = 2 },
                new City { Name = "Pančevo", PostalCode = "26000", Id = 3 },
                new City { Name = "Novi Sad", PostalCode = "21000", Id = 4 },
                new City { Name = "Beograd", PostalCode = "11000", Id = 5 }
            );
        }
    }
}
