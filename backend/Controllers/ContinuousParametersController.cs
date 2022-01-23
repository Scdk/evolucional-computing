using Microsoft.AspNetCore.Mvc;
using backend.Services;
using System.Text.Json;
using backend.Models.Configurations;

namespace backend.Controllers;

[ApiController]
[Route("continuous-parameters")]
public class ContinuousParametersController : Controller
{
    private ContinuousParameters continuousParameters = new ContinuousParameters();

    [HttpPost]
    public JsonResult Maximization([FromBody]ContinuousParametersConfiguration configuration)
    {
        return Json(continuousParameters.main(JsonSerializer.Serialize(configuration)));
    }
}