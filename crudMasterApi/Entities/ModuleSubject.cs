using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrudMasterApi.Entities
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

    public class ModuleSubjectModelCreator : IEntityTypeConfiguration<ModuleSubject>
    {
        public void Configure(EntityTypeBuilder<ModuleSubject> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.HasData(
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
                    Id = 3,
                    IdSubject = 1,
                    IdModule = 3
                },
                new ModuleSubject
                {
                    Id = 4,
                    IdSubject = 3,
                    IdModule = 3
                }
            );
        }
    }
}
