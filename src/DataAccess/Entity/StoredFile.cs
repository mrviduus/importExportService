namespace DataAccess.Entity;

public class StoredFile : BaseEntity
{
	public string FileName { get; set; }

	public byte[] Content { get; set; }

	public Job Job { get; set; }
}