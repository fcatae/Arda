using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Arda.Permissions.Models.Repositories;
using Arda.Permissions.ViewModels;

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

            if (uniqueName == null)
                throw new InvalidOperationException("Request.Header[unique_name] is null");

            var menu = _permission.GetUserMenuSerialized(uniqueName);
            return menu;

            // In the past, this code had to be called twice when data was not cached
            // However, GetUserMenuSerialized should never fail due to caching
            // I am removing this code now - let's see if the problem happens again

            //try
            //{
            //    if (uniqueName != null)
            //    {
            //        var menu = _permission.GetUserMenuSerialized(uniqueName);
            //        return menu;
            //    }
            //    return null;
            //}
            //catch (Exception ex)
            //{
            //    //Cache doesn't exists
            //    //TODO: Returns a message requiring to login again
            //    var menu = _permission.GetUserMenuSerialized(uniqueName);
            //    return menu;
            //    throw ex;
            //}
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
        public bool VerifyIfUserAdmin([FromQuery]string optionalUsername)
        {
            var uniqueName = optionalUsername;
            
            if( uniqueName == null || uniqueName == "" )
            {
                HttpContext.Request.Headers["unique_name"].ToString();
            }

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
