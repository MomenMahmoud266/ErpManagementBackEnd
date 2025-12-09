namespace ErpManagement.Domain.Models.Logging;

[Table("Com_Logs")]
public class ComLog : BaseEntity
{
    [Key]
    public int Id { get; set; }
    public LogType LogType { get; set; } = LogType.Bug;
    public string? StackTrace { get; set; }
    public string? ExceptionPath { get; set; }
    public string? ExceptionInnerPath { get; set; }
    public string? IsSuccess { get; set; }
    public string? ObjJson { get; set; }
    public string? InnerException { get; set; }
}
public enum LogType
{
    Bug,
    Integrationbug
}
