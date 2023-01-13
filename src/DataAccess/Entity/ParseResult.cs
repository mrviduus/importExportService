namespace DataAccess.Entity;

public class ParseResult : BaseEntity
{
	public int JobId { get; set; }

	public Job Job { get; set; }

	public bool IsSuccessful { get; set; }

	public string ErrorsJson { get; set; }
}