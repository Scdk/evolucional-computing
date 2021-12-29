using Microsoft.AspNetCore.Mvc;
using SimpleExempleService;
using System.Text.Json;
using Models.SimpleExempleConfiguration;

namespace backend.Controllers;

[ApiController]
[Route("simple-exemple")]
public class SimpleExempleController : Controller
{
    private SimpleExemple simpleExemple = new SimpleExemple();

    [HttpGet]
    public JsonResult SimpleExemple([FromBody]SimpleExempleConfiguration configuration)
    {
        return Json(simpleExemple.main(JsonSerializer.Serialize(configuration)));
    }
}