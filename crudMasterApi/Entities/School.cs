using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrudMasterApi.Entities
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

        public int NekiInt { get; set; }
        public long NekiLong{ get; set; }
        public bool NekiBool { get; set; }
        public decimal NekiDecimal{ get; set; }
        public double NekiDouble{ get; set; }
        public float NekiFloat { get; set; }
        public DateTime NekiDatum { get; set; }

        public virtual ICollection<Module> Modules{ get; set; }
    }

    public class SchoolModelCreator : IEntityTypeConfiguration<School>
    {
        public void Configure(EntityTypeBuilder<School> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            //builder.HasOne<City>();


            builder.HasData(
                new School
                {
                    Id = 1,
                    Name = "Gimnazija i ekonomska škola Branko Radičević",
                    CityId = 1,
                    Mail = "gimeko@yahoo.com",
                    NekiInt = 1,
                    NekiLong = 1111111111,
                    NekiBool = true,
                    NekiDouble = 0.11,
                    NekiFloat= (float)0.1,
                    NekiDecimal = (decimal)1.1,
                    NekiDatum=new DateTime(2019,1,1)
                },
                new School
                {
                    Id = 2,
                    Name = "Gimnazija i ekonomska škola Branko Radičević",
                    CityId = 1,
                    Mail = "gimeko@yahoo.com",
                    NekiInt = 2,
                    NekiLong = 2222222222,
                    NekiBool = false,
                    NekiDouble = 0.22,
                    NekiFloat = (float)0.2,
                    NekiDecimal = (decimal)2.2,
                    NekiDatum = new DateTime(2018, 1, 2)

                },
                new School
                {
                    Id = 3,
                    Name = "Gimnazija i ekonomska škola Branko Radičević",
                    CityId = 2,
                    Mail = "gimeko@yahoo.com",
                    NekiInt = 3,
                    NekiLong = 3333333333,
                    NekiBool = true,
                    NekiDouble = 0.33,
                    NekiFloat = (float)0.3,
                    NekiDecimal = (decimal)3.3,
                    NekiDatum = new DateTime(2017, 11, 2)
                },
                new School
                {
                    Id = 4,
                    Name = "Gimnazija i ekonomska škola Branko Radičević",
                    CityId = 3,
                    Mail = "gimeko@yahoo.com",
                    NekiInt = 4,
                    NekiLong = 4444444444,
                    NekiBool = true,
                    NekiDouble = 0.44,
                    NekiFloat = (float)0.4,
                    NekiDecimal = (decimal)4.4,
                    NekiDatum = new DateTime(2019, 7, 25)
                },
                new School
                {
                    Id = 5,
                    Name = "Srednja stručna škola Mihajlo Pupin",
                    CityId = 1,
                    NekiInt = 5,
                    NekiLong = 5555555555,
                    NekiBool = true,
                    NekiDouble = 0.55,
                    NekiFloat = (float)0.5,
                    NekiDecimal = (decimal)5.5,
                    NekiDatum = new DateTime(2019, 5, 25)
                }
            );
        }
    }
}
