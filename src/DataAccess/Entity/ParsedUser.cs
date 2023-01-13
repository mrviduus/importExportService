namespace DataAccess.Entity;

public class ParsedUser : BaseParseEntity
{
	public string Name { get; set; }

	public string Email { get; set; }

	public string PhoneNumber { get; set; }

	public string JobTitle { get; set; }

	public string BirthDate { get; set; }
}