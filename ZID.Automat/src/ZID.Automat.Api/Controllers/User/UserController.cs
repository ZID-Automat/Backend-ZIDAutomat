using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ZID.Automat.Api.Controllers.User
{
    [Authorize(Roles = "User")]
    [Route("[controller]")]
    [ApiController]
    public class UserController
    {
        [HttpPost("Borrow")]
        public bool borrow()
        {
            return true;
        }
    }
}
