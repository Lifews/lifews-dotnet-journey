using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Demo2.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        public LoginResponse Login(LoginRequest request)
        {
            if (request.Username == "admin" && request.Password == "123456")
            {
                var items = System
                    .Diagnostics.Process.GetProcesses()
                    .Select(p => new ProcessInfo(p.Id, p.ProcessName, p.WorkingSet64));
                return new LoginResponse(true, items.ToArray());
            }
            else
            {
                return new LoginResponse(false, null);
            }
        }
    }

    public record LoginRequest(string Username, string Password);

    public record ProcessInfo(long Id, string Name, long WorkingSet);

    public record LoginResponse(bool OK, ProcessInfo[] ProcessInfos);
}
