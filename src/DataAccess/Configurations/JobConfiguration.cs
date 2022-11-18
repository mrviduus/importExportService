using DataAccess.Entity;
using DataAccess.Entity.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataAccess.Configurations;

public class JobConfiguration: IEntityTypeConfiguration<Job>
{
    public void Configure(EntityTypeBuilder<Job> builder)
    {
        builder.HasKey(_ => _.Id);
        
        builder.Property(_ => _.Status)
            .IsRequired()
            .IsUnicode(false)
            .HasMaxLength(20)
            .HasConversion(new EnumToStringConverter<JobStatus>());

        builder.Property(_ => _.StartedOn)
            .IsRequired(false);

        builder.Property(_ => _.CompletedOn)
            .IsRequired(false);

        builder.Property(_ => _.CancelledOn)
            .IsRequired(false);

        builder.HasOne(job => job.File)
            .WithOne(file => file.Job);
    }
}