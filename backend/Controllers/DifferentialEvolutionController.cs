using Microsoft.AspNetCore.Mvc;
using backend.Services;
using System.Text.Json;
using backend.Models.Configurations;

namespace backend.Controllers;

[ApiController]
[Route("differential-evolution")]
public class DifferentialEvolutionController : Controller
{
    private DifferentialEvolutionService differentialEvolution = new DifferentialEvolutionService();

    [HttpPost]
    public JsonResult Minimization([FromBody]DifferentialEvolutionConfiguration configuration)
    {
        return Json(differentialEvolution.main(JsonSerializer.Serialize(configuration)));
    }

    [HttpPost("test")]
    public void Test([FromBody]DifferentialEvolutionConfiguration configuration)
    {
        differentialEvolution.test(JsonSerializer.Serialize(configuration));
    }
}