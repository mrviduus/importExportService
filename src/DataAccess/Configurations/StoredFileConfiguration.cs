using DataAccess.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations;

public class StoredFileConfiguration : IEntityTypeConfiguration<StoredFile>
{
    public void Configure(EntityTypeBuilder<StoredFile> builder)
    {
        builder.ToTable("StoredFile");

        builder.HasKey(_ => _.Id);
        builder.Property(_ => _.Id)
            .HasColumnName("ID")
            .ValueGeneratedNever();

        builder.Property(_ => _.FileName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(_ => _.Content)
            .IsRequired()
            .HasColumnType("VARBINARY");

        builder.HasOne(file => file.Job)
            .WithOne(job => job.File)
            .HasForeignKey<StoredFile>(file => file.Id);
    }
}