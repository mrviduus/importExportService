using DataAccess.Entity.Enums;

namespace DataAccess.Entity;

public abstract class BaseParseEntity : BaseEntity
{
    public int ParseResultId { get; set; }

    public ParseResult ParseResult { get; set; }

    public int RowNumber { get; set; }

    public JobStatus Status { get; set; }

    public string ErrorsJson { get; set; }
    
}