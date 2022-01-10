using Microsoft.AspNetCore.Mvc;
using backend.Services;
using System.Text.Json;
using backend.Models.Configurations;

namespace backend.Controllers;

[ApiController]
[Route("maximization-of-potency")]
public class MaxPotController : Controller
{
    private MaxPot maximizationOfPotency = new MaxPot();

    [HttpPost]
    public JsonResult Maximization([FromBody]BasicConfiguration configuration)
    {
        return Json(maximizationOfPotency.main(JsonSerializer.Serialize(configuration)));
    }
}