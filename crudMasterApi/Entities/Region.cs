using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrudMasterApi.Entities
{
    public class Region
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public int TestInt { get; set; }
        public virtual ICollection<City> Cities { get; set; }
    }

    public class RegionModelCreator : IEntityTypeConfiguration<Region>
    {
        public void Configure(EntityTypeBuilder<Region> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.HasData(
                new Region{ Name = "Srbija", TestInt = 111, Id = 1 },
                new Region{ Name = "Bosna", TestInt = 222, Id = 2 },
                new Region{ Name = "Angola", TestInt = 333, Id = 3 }
            );
        }
    }
}
