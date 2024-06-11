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
        private readonly ILogger<RetailorController> _logger;


        public LoginController(ILoginService loginService, IMapper mapper, ILogger<RetailorController> logger)
        {
            _mapper = mapper;
            _loginService = loginService;
            _logger = logger;
        }

        /// <summary>
        /// ADMIN LOGIN 
        /// </summary>
        
        [HttpPost]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginResource loginModel)
        {
            try
            {


                var credentials = _mapper.Map<LoginResource, Login>(loginModel);
                var user = await _loginService.LoginAsync(credentials);

                return StatusCode(user.StatusCode, user);
            }
            catch (Exception ex)
            {
                _logger.LogError(" LoginControlle" + "Login" + ex.Message);
                return StatusCode(500, "An error occured while processing your request");
            }

        }
    }
}
