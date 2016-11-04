using System;
using Microsoft.AspNetCore.Mvc;
using Arda.Common.Interfaces.Permissions;
using Arda.Common.Models.Permissions;
using System.Net.Http;
using System.Net;
using Arda.Common.ViewModels.Main;
using System.Collections.Generic;
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
        public IActionResult SetUserPermissionsAndCode(string name)
        {
            var uniqueName = HttpContext.Request.Headers["unique_name"].ToString();
            var code = HttpContext.Request.Headers["code"].ToString();

            try
            {
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
            catch (Exception)
            {
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut]
        [Route("updateuserpermissions")]
        public HttpResponseMessage UpdateUserPermissions(string uniqueName)
        {
            try
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
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete]
        [Route("deleteuserpermissions")]
        public HttpResponseMessage DeleteUserPermissions()
        {
            try
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
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete]
        [Route("deleteuser")]
        public HttpResponseMessage DeleteUser(string uniqueName)
        {
            try
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
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut]
        [Route("updateuserphoto")]
        public HttpResponseMessage UpdateUserPhoto(string uniqueName)
        {
            try
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
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut]
        [Route("saveuserphotooncache")]
        public HttpResponseMessage SaveUserPhotoOnCache(string uniqueName)
        {
            try
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
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut]
        [Route("updateuser")]
        public HttpResponseMessage UpdateUser(string uniqueName)
        {
            try
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
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("verifyuseraccesstoresource")]
        public HttpResponseMessage VerifyUserAccessToResource(string uniqueName, string module, string resource)
        {
            try
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
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("getallpermissions")]
        public IEnumerable<ResourcesViewModel> GetAllPermissions()
        {
            try
            {
                return _permission.GetAllPermissions();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}