using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Natural_API.Resources;
using Natural_Core.Models;
using Natural_Core.IServices;
using System.Runtime.ExceptionServices;
using Swashbuckle.AspNetCore.Swagger;

#nullable disable

namespace Natural_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly IMapper _mapper;


        public LoginController(ILoginService loginService, IMapper mapper)
        {
            _mapper = mapper;
            _loginService = loginService;
        }

        [HttpPost]
        public async Task<ActionResult<LoginResponse>> ValidUser([FromBody] LoginResource loginModel)
        {
            var credentials = _mapper.Map<LoginResource, Login>(loginModel);
            var user = await _loginService.LoginAsync(credentials);
            
                return StatusCode(user.StatusCode,user);

        }
    }
}
