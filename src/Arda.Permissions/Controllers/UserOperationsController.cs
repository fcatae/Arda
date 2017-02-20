using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Arda.Common.Interfaces.Permissions;
using Arda.Common.ViewModels.Main;

namespace Arda.Permissions.Controllers
{
    [Route("api/[controller]")]
    public class UserOperationsController : Controller
    {

        private IPermissionRepository _permission;

        public UserOperationsController(IPermissionRepository permission)
        {
            _permission = permission;
        }


        [HttpGet]
        [Route("getusermenu")]
        public string GetUserMenu()
        {
            // TODO: REQUIRES REVIEWING
            var uniqueName = HttpContext.Request.Headers["unique_name"].ToString();

            try
            {
                if (uniqueName != null)
                {
                    var menu = _permission.GetUserMenuSerialized(uniqueName);
                    return menu;
                }
                return null;
            }
            catch (Exception ex)
            {
                //Cache doesn't exists
                //TODO: Returns a message requiring to login again
                var menu = _permission.GetUserMenuSerialized(uniqueName);
                return menu;

                // WARNING: NEVER REACH THIS POINT
                throw ex;
            }
        }

        [HttpGet]
        [Route("getuserstatus")]
        public int GetUserStatus()
        {
            var uniqueName = HttpContext.Request.Headers["unique_name"].ToString();

            if (uniqueName != null)
            {
                return (int)_permission.GetUserStatus(uniqueName);
            }
            return -1;
        }

        [HttpGet]
        [Route("verifyifuseradmin")]
        public bool VerifyIfUserAdmin()
        {
            var uniqueName = HttpContext.Request.Headers["unique_name"].ToString();

            if (uniqueName != null)
            {
                return _permission.VerifyIfUserAdmin(uniqueName);
            }
            return false;
        }

        [HttpGet]
        [Route("getnumberofuserstoapprove")]
        public int GetNumberOfUsersToApprove()
        {
            return _permission.GetNumberOfUsersToApprove();
        }

        [HttpGet]
        [Route("getpendingusers")]
        public IEnumerable<UserMainViewModel> GetPendingUsers()
        {
            return _permission.GetPendingUsers();
        }

        [HttpGet]
        [Route("getuserpermissions")]
        public PermissionsViewModel GetUserPermissions([FromQuery]string uniqueName)
        {
            return _permission.GetUserPermissions(uniqueName);
        }

        [HttpGet]
        [Route("getusers")]
        public IEnumerable<UserMainViewModel> GetUsers()
        {
            return _permission.GetUsers();
        }

        [HttpGet]
        [Route("getuser")]
        public UserMainViewModel GetUser([FromQuery]string uniqueName)
        {
            return _permission.GetUser(uniqueName);
        }

    }
}
