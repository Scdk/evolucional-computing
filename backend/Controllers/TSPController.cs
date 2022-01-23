using Microsoft.AspNetCore.Mvc;
using backend.Services;
using System.Text.Json;
using backend.Models.Configurations;

namespace backend.Controllers;

[ApiController]
[Route("travelling-salesman")]
public class TSPController : Controller
{
    private TSP travellingSalesman = new TSP();

    [HttpPost]
    public JsonResult Maximization([FromBody]TSPConfiguration configuration)
    {
        return Json(travellingSalesman.main(JsonSerializer.Serialize(configuration)));
    }

    [HttpPost("test")]
    public void Test([FromBody]TSPConfiguration configuration)
    {
        travellingSalesman.test(JsonSerializer.Serialize(configuration));
    }
}