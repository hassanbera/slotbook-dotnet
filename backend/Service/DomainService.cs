using App.Model;
using App.Repository;
using App.Common;
using static App.Common.Input;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
namespace App.Service;

public class DomainService(IDomainRepository repo)
{

  public async Task<object> Employees() => await repo.Query<Employee>().OrderBy(x => x.Name).ToListAsync();
  public async Task<object> Employee(JsonElement j) { var e = new Employee { Name = Text(j, "name", 80, true), Title = Text(j, "title", 80), Color = Text(j, "color", 20, true) }; if (!System.Text.RegularExpressions.Regex.IsMatch(e.Color, @"^#[a-fA-F0-9]{6}$")) throw new ApiException(400, "Renk geçersiz."); repo.Add(e); await repo.Save(); return e; }
  public async Task<object> Appointments(string from, string to) { if (!DateTimeOffset.TryParse(from, out var f) || !DateTimeOffset.TryParse(to, out var t) || t <= f || t - f > TimeSpan.FromDays(31)) throw new ApiException(400, "Aralık 1–31 gün olmalı."); f = f.ToUniversalTime(); t = t.ToUniversalTime(); return await repo.Query<Appointment>().Where(a => a.StartAt < t && a.EndAt > f).OrderBy(a => a.StartAt).ToListAsync(); }
  public async Task<object> Create(JsonElement j) { var a = new Appointment(); await Apply(a, j); repo.Add(a); await repo.Save(); return a; }
  public async Task<object> Update(Guid id, JsonElement j) { await using var tx = await repo.Begin(); var a = await repo.Lock<Appointment>(id); Check(a, j); await Apply(a, j); a.Version++; await repo.Save(); await tx.CommitAsync(); return a; }
  public async Task<object> Status(Guid id, JsonElement j) { await using var tx = await repo.Begin(); var a = await repo.Lock<Appointment>(id); Check(a, j); a.Status = Choice(j, "status", "COMPLETED", "CANCELLED"); a.Version++; await repo.Save(); await tx.CommitAsync(); return a; }
  private void Check(Appointment a, JsonElement j) { if (a.Version != Number(j, "version", 1, int.MaxValue)) throw new ApiException(409, "Kayıt değişmiş; yeniden yükleyin."); if (a.Status != "SCHEDULED") throw new ApiException(409, "Randevu artık değiştirilemez."); }
  private async Task Apply(Appointment a, JsonElement j) { a.Title = Text(j, "title", 120, true); a.ClientName = Text(j, "clientName", 100, true); a.ClientEmail = Email(j); a.Note = Text(j, "note", 1500); a.EmployeeId = Id(j, "employeeId"); await repo.Find<Employee>(a.EmployeeId); a.StartAt = Instant(j, "startAt"); a.EndAt = Instant(j, "endAt"); if (a.EndAt <= a.StartAt || a.EndAt - a.StartAt > TimeSpan.FromHours(12)) throw new ApiException(400, "Randevu süresi 0–12 saat olmalı."); }

}
