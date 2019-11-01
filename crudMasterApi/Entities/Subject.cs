using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrudMasterApi.Entities
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

    public class SubjectModelCreator : IEntityTypeConfiguration<Subject>
    {
        public void Configure(EntityTypeBuilder<Subject> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.HasData(
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
