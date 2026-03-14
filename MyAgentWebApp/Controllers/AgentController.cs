using Microsoft.AspNetCore.Mvc;
using MyAgentWebApp.Services;

namespace MyAgentWebApp.Controllers;

[ApiController]
[Route("[controller]")]
public class AgentController(AgentServices agentServices) : ControllerBase
{
   [HttpGet]
   public async Task<IActionResult> Get([FromQuery] string menssage)
   {
        var result = await agentServices.Chat(menssage);
        return Ok(result);
    }
}
