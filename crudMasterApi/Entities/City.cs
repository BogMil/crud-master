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
        public int RegionId { get; set; }
        public virtual Region Region { get; set; }
        public virtual ICollection<School> Schools { get; set; }
    }

    public class CityModelCreator : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.HasData(
                new City { Name = "Kovin", PostalCode = "26220", Id = 1 ,RegionId = 1},
                new City { Name = "Beograd", PostalCode = "11000", Id = 2, RegionId = 1 },
                new City { Name = "Banja Luka", PostalCode = "26000", Id = 3, RegionId = 2 },
                new City { Name = "Bihac", PostalCode = "21000", Id = 4, RegionId = 2 },
                new City { Name = "Luanda", PostalCode = "11000", Id = 5, RegionId = 3}
            );
        }
    }
}
