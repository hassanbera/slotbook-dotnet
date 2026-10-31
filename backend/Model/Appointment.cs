namespace App.Model;

public class Appointment
{
  public Guid Id { get; set; } = Guid.NewGuid();
  public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
  public Guid EmployeeId { get; set; } = Guid.Empty;
  public string Title { get; set; } = "";
  public string ClientName { get; set; } = "";
  public string ClientEmail { get; set; } = "";
  public string Note { get; set; } = "";
  public DateTimeOffset StartAt { get; set; } = DateTimeOffset.UtcNow;
  public DateTimeOffset EndAt { get; set; } = DateTimeOffset.UtcNow;
  public string Status { get; set; } = "SCHEDULED";
  public int Version { get; set; } = 1;
}
