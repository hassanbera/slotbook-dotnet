using Microsoft.AspNetCore.Mvc;
using App.Service;
using App.Repository;
using System.Text.Json;
namespace App.Controller;

[ApiController]
[Route("api")]
public class ApiController(DomainService service, IDomainRepository repo) : ControllerBase
{
  [HttpGet("health")] public async Task<object> Health() { if (!await repo.Healthy()) throw new Exception("Database unavailable"); return new { data = new { status = "ok" } }; }
  [HttpGet("employees")] public async Task<object> Action0() { return new { data = await service.Employees() }; }
  [HttpPost("employees")] public async Task<object> Action1([FromBody] JsonElement j) { return new { data = await service.Employee(j) }; }
  [HttpGet("appointments")] public async Task<object> Action2([FromQuery] string from, [FromQuery] string to) { return new { data = await service.Appointments(from, to) }; }
  [HttpPost("appointments")] public async Task<object> Action3([FromBody] JsonElement j) { return new { data = await service.Create(j) }; }
  [HttpPut("appointments/{id}")] public async Task<object> Action4(Guid id, [FromBody] JsonElement j) { return new { data = await service.Update(id, j) }; }
  [HttpPatch("appointments/{id}/status")] public async Task<object> Action5(Guid id, [FromBody] JsonElement j) { return new { data = await service.Status(id, j) }; }
}
