using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

                throw ex;
            }
        }

        [HttpGet]
        [Route("getuserstatus")]
        public int GetUserStatus()
        {
            var uniqueName = HttpContext.Request.Headers["unique_name"].ToString();

            try
            {
                if (uniqueName != null)
                {
                    return (int)_permission.GetUserStatus(uniqueName);
                }
                return -1;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        [HttpGet]
        [Route("verifyifuseradmin")]
        public bool VerifyIfUserAdmin()
        {
            var uniqueName = HttpContext.Request.Headers["unique_name"].ToString();

            try
            {
                if (uniqueName != null)
                {
                    return _permission.VerifyIfUserAdmin(uniqueName);
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("getnumberofuserstoapprove")]
        public int GetNumberOfUsersToApprove()
        {
            try
            {
                return _permission.GetNumberOfUsersToApprove();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("getpendingusers")]
        public IEnumerable<UserMainViewModel> GetPendingUsers()
        {
            try
            {
                return _permission.GetPendingUsers();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("getuserpermissions")]
        public PermissionsViewModel GetUserPermissions(string uniqueName)
        {
            try
            {
                return _permission.GetUserPermissions(uniqueName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("getusers")]
        public IEnumerable<UserMainViewModel> GetUsers()
        {
            try
            {
                return _permission.GetUsers();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("getuser")]
        public UserMainViewModel GetUser(string uniqueName)
        {
            try
            {
                return _permission.GetUser(uniqueName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
