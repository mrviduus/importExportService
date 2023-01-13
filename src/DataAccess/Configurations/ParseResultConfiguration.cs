using DataAccess.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations;

public class ParseResultConfiguration : IEntityTypeConfiguration<ParseResult>
{
	public void Configure(EntityTypeBuilder<ParseResult> builder)
	{
		builder.HasKey(_ => _.Id);

		builder.HasIndex(x => x.JobId)
			.IsUnique();

		builder.HasOne(parseResult => parseResult.Job)
			.WithOne(job => job.ParseResult)
			.HasForeignKey<ParseResult>(parseResult => parseResult.JobId);

		builder.Property(_ => _.ErrorsJson)
			.HasColumnType("NVARCHAR");
	}
}