using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using ZID.Automat.Infrastructure;

namespace ZID.Automat.Api.Controllers.User
{
    [AllowAnonymous]
    [Route("[controller]")]
    [ApiController]
    public class UserController
    {
        AutomatContext AC1;
        public UserController(AutomatContext AC)
        {
            AC1 = AC;
        }

        [HttpPost("Borrow")]
        public bool borrow()
        {
            return true;
        }
    }
}
