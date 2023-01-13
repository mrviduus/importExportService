using DataAccess.Entity.Enums;

namespace DataAccess.Entity;

public class Job : BaseEntity
{
	public string Code { get; set; }

	public JobStatus Status { get; set; }

	public DateTime CreatedOn { get; set; }

	public DateTime? StartedOn { get; set; }

	public DateTime? CompletedOn { get; set; }

	public DateTime? CancelledOn { get; set; }

	public StoredFile File { get; set; }

	public ParseResult ParseResult { get; set; }
}