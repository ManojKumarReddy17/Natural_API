using Natural_Core;
using Natural_Core.IServices;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Natural_Core.Models;
using System.Net.Http.Headers;
using System;
using System.Security.Claims;
using Microsoft.Extensions.Logging;


#nullable disable

namespace Natural_Services
{
    public class LoginService : ILoginService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<LoginService> _logger;

        public LoginService(IUnitOfWork unitOfWork, ILogger<LoginService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;

        }
      
        public async Task<LoginResponse> LoginAsync(Login credentials)
        {
            LoginResponse response = new LoginResponse();
            try
            {
                try
                {
                    var user = await _unitOfWork.Login.GetAllAsync();

                    var authenticatedUser = user.FirstOrDefault(u => u.UserName == credentials.UserName && u.Password == credentials.Password);


                    if (authenticatedUser != null)
                    {

                        response.FirstName = authenticatedUser.FirstName;
                        response.LastName = authenticatedUser.LastName;
                        response.StatusCode = 200;
                        response.Message = "LOGIN SUCCESSFUL";
                        return response;

                    }

                    else
                    {
                        response.StatusCode = 401;
                        response.Message = "INVALID CREDENTIALS";
                        return response;

                    }


                }

                catch (Exception)
                {
                    response.Message = "INTERNAL SERVER ERROR";
                    response.StatusCode = 500;
                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(" LoginService-LoginAsync", ex.Message);
                return null;

            }


        }
    }

           
}
