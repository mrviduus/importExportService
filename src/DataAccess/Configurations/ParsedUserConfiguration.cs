using DataAccess.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations;

public class ParsedUserConfiguration : IEntityTypeConfiguration<ParsedUser>
{
	public void Configure(EntityTypeBuilder<ParsedUser> builder)
	{
		builder.ToTable("ParsedUser");

		builder.HasKey(_ => _.Id);
		builder.Property(_ => _.Id)
			.HasColumnName("ID");

		builder.Property(_ => _.ErrorsJson)
			.HasColumnType("NVARCHAR");

		builder.Property(_ => _.Name)
			.HasMaxLength(100);

		builder.Property(_ => _.JobTitle)
			.HasMaxLength(100);

		builder.Property(_ => _.Email)
			.HasMaxLength(100);

		builder.Property(_ => _.PhoneNumber)
			.HasMaxLength(20);

		builder.Property(_ => _.Email)
			.HasMaxLength(100);

		builder.Property(_ => _.BirthDate)
			.HasMaxLength(100);

	}
}