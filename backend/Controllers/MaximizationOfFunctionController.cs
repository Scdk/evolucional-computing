using Microsoft.AspNetCore.Mvc;
using backend.Services;
using System.Text.Json;
using backend.Models.Configurations;

namespace backend.Controllers;

[ApiController]
[Route("maximization-of-function")]
public class MaximizationOfFunctionController : Controller
{
    private MaximizationOfFunction maximizationOfFunction = new MaximizationOfFunction();

    [HttpPost]
    public JsonResult Maximization([FromBody]BasicConfiguration configuration)
    {
        return Json(maximizationOfFunction.main(JsonSerializer.Serialize(configuration)));
    }
}