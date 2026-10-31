using Microsoft.EntityFrameworkCore;
using App.Model;
namespace App.Repository;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
  public DbSet<Employee> Employees => Set<Employee>();
  public DbSet<Appointment> Appointments => Set<Appointment>();
  protected override void OnModelCreating(ModelBuilder b)
  {
    b.Entity<Employee>(e => { e.ToTable("employees"); e.HasKey(x => x.Id); e.Property(x => x.Id).HasColumnName("id"); e.Property(x => x.CreatedAt).HasColumnName("created_at"); e.Property(x => x.Name).HasColumnName("name").HasMaxLength(80).IsRequired(); e.Property(x => x.Title).HasColumnName("title").HasMaxLength(80).IsRequired(); e.Property(x => x.Color).HasColumnName("color").HasMaxLength(20).IsRequired(); });
    b.Entity<Appointment>(e => { e.ToTable("appointments"); e.HasKey(x => x.Id); e.Property(x => x.Id).HasColumnName("id"); e.Property(x => x.CreatedAt).HasColumnName("created_at"); e.Property(x => x.EmployeeId).HasColumnName("employee_id"); e.Property(x => x.Title).HasColumnName("title").HasMaxLength(120).IsRequired(); e.Property(x => x.ClientName).HasColumnName("client_name").HasMaxLength(100).IsRequired(); e.Property(x => x.ClientEmail).HasColumnName("client_email").HasMaxLength(200).IsRequired(); e.Property(x => x.Note).HasColumnName("note").HasMaxLength(1500).IsRequired(); e.Property(x => x.StartAt).HasColumnName("start_at"); e.Property(x => x.EndAt).HasColumnName("end_at"); e.Property(x => x.Status).HasColumnName("status").HasMaxLength(20).IsRequired(); e.Property(x => x.Version).HasColumnName("version"); });
    b.Entity<Appointment>().HasOne<Employee>().WithMany().HasForeignKey(x => x.EmployeeId).OnDelete(DeleteBehavior.Restrict);
  }
}
