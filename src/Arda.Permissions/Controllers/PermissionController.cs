using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net;
using System.Collections.Generic;
using Arda.Permissions.Models;
using Arda.Permissions.Models.Repositories;
using Arda.Permissions.ViewModels;
using Newtonsoft.Json;

namespace Arda.Permissions.Controllers
{
    [Route("api/[controller]")]
    public class PermissionController : Controller
    {
        private IPermissionRepository _permission;

        public PermissionController(IPermissionRepository permission)
        {
            _permission = permission;
        }


        [HttpPost]
        [Route("setuserpermissionsandcode")]
        public IActionResult SetUserPermissionsAndCode([FromQuery]string name)
        {
            var uniqueName = HttpContext.Request.Headers["unique_name"].ToString();
            var code = HttpContext.Request.Headers["code"].ToString();

            if (uniqueName != null && name != null && code != null)
            {
                User responseUser = null;
                bool responseEmail = false;

                bool UserExists = _permission.VerifyIfUserIsInUserPermissionsDatabase(uniqueName);
                if (!UserExists)
                {
                    responseUser = _permission.CreateNewUserAndSetInitialPermissions(uniqueName, name);
                    responseEmail = _permission.SendNotificationOfNewUserByEmail(uniqueName);
                    if (responseUser == null || responseEmail == false)
                    {
                        return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
                    }
                    else
                    {
                        bool response = _permission.SetUserPermissionsAndCode(uniqueName, code);
                        if (response)
                        {
                            return new StatusCodeResult((int)HttpStatusCode.OK);
                        }
                        else
                        {
                            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
                        }
                    }
                }
                else
                {
                    bool response = _permission.SetUserPermissionsAndCode(uniqueName, code);
                    if (response)
                    {
                        return new StatusCodeResult((int)HttpStatusCode.OK);
                    }
                    else
                    {
                        return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
                    }
                }
            }
            else
            {
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);
            }
        }

        [HttpPut]
        [Route("updateuserpermissions")]
        public HttpResponseMessage UpdateUserPermissions([FromQuery]string uniqueName)
        {
            var reader = new System.IO.StreamReader(HttpContext.Request.Body);
            var userPermissionsSerialized = reader.ReadToEnd();
            var userPermissions = JsonConvert.DeserializeObject<PermissionsViewModel>(userPermissionsSerialized);

            if (uniqueName != null && userPermissions != null)
            {
                if (_permission.UpdateUserPermissions(uniqueName, userPermissions))
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                else
                {
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        [HttpDelete]
        [Route("deleteuserpermissions")]
        public HttpResponseMessage DeleteUserPermissions()
        {
            var uniqueName = HttpContext.Request.Headers["unique_name"].ToString();

            if (uniqueName != null)
            {
                _permission.DeleteUserPermissions(uniqueName);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        [HttpDelete]
        [Route("deleteuser")]
        public HttpResponseMessage DeleteUser([FromQuery]string uniqueName)
        {
            if (uniqueName != null)
            {
                _permission.DeleteUser(uniqueName);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        [HttpPut]
        [Route("updateuserphoto")]
        public HttpResponseMessage UpdateUserPhoto([FromQuery]string uniqueName)
        {
            System.IO.StreamReader reader = new System.IO.StreamReader(HttpContext.Request.Body);
            string requestFromPost = reader.ReadToEnd();
            var userPhoto = JsonConvert.DeserializeObject<string>(requestFromPost);

            if (uniqueName != null && userPhoto != null)
            {
                _permission.UpdateUserPhoto(uniqueName, userPhoto);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }
                
        [HttpGet]
        [Route("getuserphotofromcache")]
        public string GetUserPhotoFromCache([FromQuery]string uniqueName)
        {
            string photo = _permission.GetUserPhotoFromCache(uniqueName);

            return JsonConvert.SerializeObject( _permission.GetUserPhotoFromCache(uniqueName) );
        }

        [HttpPut]
        [Route("saveuserphotooncache")]
        public HttpResponseMessage SaveUserPhotoOnCache([FromQuery]string uniqueName)
        {
            if (uniqueName != null)
            {
                if (_permission.SaveUserPhotoOnCache(uniqueName))
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                else
                {
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        [HttpPut]
        [Route("updateuser")]
        public HttpResponseMessage UpdateUser([FromQuery]string uniqueName)
        {
            System.IO.StreamReader reader = new System.IO.StreamReader(HttpContext.Request.Body);
            string requestFromPost = reader.ReadToEnd();
            var userProfile = JsonConvert.DeserializeObject<UserMainViewModel>(requestFromPost);

            if (uniqueName != null && userProfile != null)
            {
                _permission.UpdateUser(uniqueName, userProfile);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        [Route("verifyuseraccesstoresource")]
        public HttpResponseMessage VerifyUserAccessToResource([FromQuery]string uniqueName, [FromQuery]string module, [FromQuery]string resource)
        {
            if (uniqueName != null && module != null && resource != null)
            {
                if (_permission.VerifyUserAccessToResource(uniqueName, module, resource))
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                else
                {
                    return new HttpResponseMessage(HttpStatusCode.Forbidden);
                }
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        [Route("getallpermissions")]
        public IEnumerable<ResourcesViewModel> GetAllPermissions()
        {
            return _permission.GetAllPermissions();
        }
    }
}