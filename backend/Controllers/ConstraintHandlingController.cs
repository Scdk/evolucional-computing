using Microsoft.AspNetCore.Mvc;
using backend.Services;
using System.Text.Json;
using backend.Models.Configurations;

namespace backend.Controllers;

[ApiController]
[Route("constraint-handling")]
public class ConstraintHandlingController : Controller
{
    private ConstraintHandling constraintHandling = new ConstraintHandling();

    [HttpPost]
    public JsonResult Maximization([FromBody]ConstraintHandlingConfiguration configuration)
    {
        return Json(constraintHandling.main(JsonSerializer.Serialize(configuration)));
    }

    [HttpPost("challenge")]
    public JsonResult Challenge([FromBody]ConstraintHandlingConfiguration configuration)
    {
        return Json(constraintHandling.challenge(JsonSerializer.Serialize(configuration)));
    }
}