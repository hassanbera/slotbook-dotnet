namespace App.Model;

public class Employee
{
  public Guid Id { get; set; } = Guid.NewGuid();
  public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
  public string Name { get; set; } = "";
  public string Title { get; set; } = "";
  public string Color { get; set; } = "#4263eb";
}
