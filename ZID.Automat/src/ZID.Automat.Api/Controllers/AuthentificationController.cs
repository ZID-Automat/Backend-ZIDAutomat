﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using ZID.Automat.Infrastructure;
using ZID.Automat.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using ZID.Automat.Dto.Models;
using ZID.Automat.Application;

namespace ZID.Automat.Api.Controllers
{
    /// <summary>
    /// This APIController is used to do any related Account operations
    /// </summary>
    [AllowAnonymous]
    [Route("[controller]")]
    [ApiController]
    public class AuthentificationController : ControllerBase
    {
        private readonly IUserAuthService _userAuth;
        private readonly IControllerAuthService _controllerAuthService;
        private readonly IAdminAuthService _adminAuth;

        public AuthentificationController(IUserAuthService userAuth, IControllerAuthService controllerAuthService, IAdminAuthService adminAuth)
        {
            _userAuth = userAuth;
            _controllerAuthService = controllerAuthService;
            _adminAuth = adminAuth;

        }

        [HttpPost("UserLogin")]
        public string? UserLogin([FromBody] UserLoginDto UserLogin)
        {
            return _userAuth.AuthUser(UserLogin);
        }

        [HttpPost("AutomatLogin")]
        public string? ControllerPost([FromBody] ControllerLoginDto ControllerLogin)
        {
            return _controllerAuthService.AuthController(ControllerLogin);

        }

        [HttpPost("AdminLogin")]
        public string? AdminLogin([FromBody] AdminLoginDto AdminLogin)
        {
            return _adminAuth.AuthAdmin(AdminLogin);
        }
    }
}