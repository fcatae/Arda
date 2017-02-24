using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Arda.Common.Interfaces.Permissions;
using Arda.Common.ViewModels.Main;

namespace Arda.Permissions.Controllers
{
    [Route("api/[controller]")]
    public class ProblemReproController : Controller
    {
        [HttpGet]
        [Route("getusers")]
        public IEnumerable<UserMainViewModel> GetUsers()
        {
            var ret = new List<UserMainViewModel>();

            throw new InvalidOperationException("This exception returns HTTP 200 instead of 500");

            return ret;
        }
    }
}
