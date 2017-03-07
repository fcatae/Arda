using System;
using System.Linq;
using Microsoft.Extensions.Caching.Distributed;
using Arda.Common.Utils;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http;
using Arda.Permissions.Models;
using Arda.Permissions.Models.Repositories;
using Arda.Permissions.ViewModels;

namespace Arda.Permissions.Repositories
{
    //TODO: Splits in User and Permission Repository
    public class PermissionRepository : IPermissionRepository
    {
        private PermissionsContext _context;
        private IDistributedCache _cache;

        public PermissionRepository(PermissionsContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }


        CacheViewModel GetUserPermissionsAndCode(string uniqueName, string code)
        {
            var userPermissions = (from u in _context.Users
                                   join up in _context.UsersPermissions on u.UniqueName equals up.UniqueName
                                   join r in _context.Resources on up.ResourceID equals r.ResourceID
                                   join m in _context.Modules on r.ModuleID equals m.ModuleID
                                   where up.UniqueName == uniqueName && r.ResourceSequence > 0
                                   orderby r.CategorySequence, r.ResourceSequence
                                   select new PermissionsToBeCachedViewModel
                                   {
                                       Endpoint = m.Endpoint,
                                       Module = m.ModuleName,
                                       Resource = r.ResourceName,
                                       Category = r.Category,
                                       DisplayName = r.DisplayName
                                   }).ToList();

            return new CacheViewModel(code, userPermissions);
        }

        public bool SetUserPermissionsAndCode(string uniqueName, string code)
        {
            var userPermissions = (from u in _context.Users
                                   join up in _context.UsersPermissions on u.UniqueName equals up.UniqueName
                                   join r in _context.Resources on up.ResourceID equals r.ResourceID
                                   join m in _context.Modules on r.ModuleID equals m.ModuleID
                                   where up.UniqueName == uniqueName && r.ResourceSequence > 0
                                   orderby r.CategorySequence, r.ResourceSequence
                                   select new PermissionsToBeCachedViewModel
                                   {
                                       Endpoint = m.Endpoint,
                                       Module = m.ModuleName,
                                       Resource = r.ResourceName,
                                       Category = r.Category,
                                       DisplayName = r.DisplayName
                                   }).ToList();

            var propertiesToCache = new CacheViewModel(code, userPermissions);
            _cache.Set(uniqueName, Util.GetBytes(propertiesToCache.ToString()));

            return true;
        }

        //Updates permissions on database and cache
        public bool UpdateUserPermissions(string uniqueName, PermissionsViewModel newUserPermissions)
        {
            //Delete old permissions from the database:
            var oldPermissions = _context.UsersPermissions.Where(up => up.UniqueName == uniqueName);
            _context.UsersPermissions.RemoveRange(oldPermissions);
            //_context.SaveChanges();

            //Update the database
            //Permissions:
            foreach (var permissionToQuery in newUserPermissions.permissions)
            {
                var resourceReturned = _context.Resources.First(r => r.Category == permissionToQuery.category && r.DisplayName == permissionToQuery.resource);

                _context.UsersPermissions.Add(new UsersPermission()
                {
                    UniqueName = uniqueName,
                    ResourceID = resourceReturned.ResourceID
                });
            }
            //User:
            var user = _context.Users.First(u => u.UniqueName == uniqueName);
            if (newUserPermissions.permissions.Count > 0)
            {
                user.Status = PermissionStatus.Permissions_Granted;
            }
            else
            {
                user.Status = PermissionStatus.Permissions_Denied;
            }

            _context.SaveChanges();

            //Update the cache
            CacheViewModel propertiesToCache = new CacheViewModel();
            byte[] arrPropertiesSerializedCached = null;

            try
            {
                // may raise exception
                arrPropertiesSerializedCached = _cache.Get(uniqueName);

                // if data is not cached
                if(arrPropertiesSerializedCached != null)
                {
                    string propertiesSerializedCached = Util.GetString(arrPropertiesSerializedCached);
                    propertiesToCache = new CacheViewModel(propertiesSerializedCached);
                }
            }
            catch (StackExchange.Redis.RedisConnectionException)
            {
                // Ignore transient network errors
            }            

            var userPermissions = (from up in _context.UsersPermissions
                                   join r in _context.Resources on up.ResourceID equals r.ResourceID
                                   join m in _context.Modules on r.ModuleID equals m.ModuleID
                                   where up.UniqueName == uniqueName && r.ResourceSequence > 0
                                   orderby r.CategorySequence, r.ResourceSequence
                                   select new PermissionsToBeCachedViewModel
                                   {
                                       Endpoint = m.Endpoint,
                                       Module = m.ModuleName,
                                       Resource = r.ResourceName,
                                       Category = r.Category,
                                       DisplayName = r.DisplayName
                                   }).ToList();

            if (userPermissions != null)
            {
                propertiesToCache.Permissions = userPermissions;
                _cache.Set(uniqueName, Util.GetBytes(propertiesToCache.ToString()));
                return true;
            }
            else
            {
                return false;
            }
        }
        
        //Updates user photo
        public bool UpdateUserPhoto(string uniqueName, string photo)
        {
            var user = _context.Users.First(u => u.UniqueName == uniqueName);

            if (user != null && photo != null)
            {
                //Save on database:
                user.PhotoBase64 = photo;
                _context.SaveChanges();
                //Cache it:
                CacheUserPhoto(uniqueName, photo);
                return true;
            }
            else
            {
                return false;
            }
        }

        //Cache User Photo:
        public void CacheUserPhoto(string uniqueName, string PhotoBase64)
        {
            var key = "photo_" + uniqueName;
            _cache.Set(key, Util.GetBytes(PhotoBase64.ToString()));
        }

        //Updates user info
        public bool UpdateUser(string uniqueName, UserMainViewModel userToBeUpdated)
        {
            var user = _context.Users.First(u => u.UniqueName == uniqueName);

            if (user != null)
            {
                user.Name = userToBeUpdated.Name;
                user.GivenName = userToBeUpdated.GivenName;
                user.Surname = userToBeUpdated.Surname;
                user.JobTitle = userToBeUpdated.JobTitle;
                user.ManagerUniqueName = userToBeUpdated.ManagerUniqueName;

                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void DeleteUserPermissions(string uniqueName)
        {
            _cache.Remove(uniqueName);
        }

        public void DeleteUser(string uniqueName)
        {
            //From Cache:
            _cache.Remove(uniqueName);

            //From Context:
            var userPermissions = (from up in _context.UsersPermissions
                                   where up.UniqueName == uniqueName
                                   select up).ToList();

            var user = (from u in _context.Users
                        where u.UniqueName == uniqueName
                        select u).First();

            _context.UsersPermissions.RemoveRange(userPermissions);
            _context.Users.Remove(user);
            _context.SaveChanges();

            //From Kanban:
            var res = Util.ConnectToRemoteService(HttpMethod.Delete, Util.KanbanURL + "api/user/delete?userID=" + uniqueName, "kanban", "kanban").Result;

            if (!res.IsSuccessStatusCode)
            {
                throw new Exception();
            }
        }

        CacheViewModel GetUserPermissionsCached(string uniqueName)
        {
            CacheViewModel cachedView = null;
            byte[] arraySerializedCached = null;

            if (uniqueName == null)
                throw new ArgumentNullException("GetUserPermissionsCached(uniqueName: null)");

            try
            {
                arraySerializedCached = _cache.Get(uniqueName);
            }
            catch(StackExchange.Redis.RedisConnectionException)
            {
                // Ignore transient network issues
            }

            if( arraySerializedCached != null )
            {
                var propertiesSerializedCached = Util.GetString(arraySerializedCached);
                cachedView = new CacheViewModel(propertiesSerializedCached);
            }

            return cachedView;
        }

        public bool VerifyUserAccessToResource(string uniqueName, string module, string resource)
        {
            // THIS IMPLEMENTATION IS WRONG! 
            // If the cache is clean, then it will not properly verify the user access

            //try
            //{
            //    var propertiesSerializedCached = Util.GetString(_cache.Get(uniqueName));
            //    if (propertiesSerializedCached != null)
            //    {
            //        var permissions = new CacheViewModel(propertiesSerializedCached).Permissions;

            //        var perm = (from p in permissions
            //                    where p.Resource.ToLower().Equals(resource) && p.Module.ToLower().Equals(module)
            //                    select p).First();

            //        return (perm != null);
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    if (ex.Message == "Sequence contains no elements")
            //    {
            //        return false;
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            var cachedView = GetUserPermissionsCached(uniqueName);

            if (cachedView != null)
            {
                var permissions = cachedView.Permissions;

                var perm = (from p in permissions
                            where p.Resource.ToLower().Equals(resource) && p.Module.ToLower().Equals(module)
                            select p).First();

                return (perm != null);
            }

            return false;
        }

        public bool VerifyIfUserIsInUserPermissionsDatabase(string uniqueName)
        {
            var response = _context.Users.SingleOrDefault(u => u.UniqueName == uniqueName);

            if (response == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool SendNotificationOfNewUserByEmail(string uniqueName)
        {
            return true;

            //EmailLogic clientEmail = new EmailLogic();

            //// Mounting parameters and message.
            //var Configuration = new ConfigurationBuilder().AddJsonFile("secrets.json").Build();
            //var admin = Configuration["Email:Administrator"];

            //string ToName = "Arda Administrator";
            //string ToEmail = admin;
            //string Subject = "[ARDA] A new user has been logged @ Arda";

            //StringBuilder StructureModified = new StringBuilder();
            //StructureModified = EmailMessages.GetEmailMessageStructure();

            //// Replacing the generic title by the customized.
            //StructureModified.Replace("[MessageTitle]", "Hi " + ToName + ", someone requested access to the system");

            //// Replacing the generic subtitle by the customized.
            //StructureModified.Replace("[MessageSubtitle]", "Who did the request was <strong>" + uniqueName + "</strong>. If you deserve, can access 'Users' area and distribute the appropriated permissions.");

            //// Replacing the generic message body by the customized.
            //StructureModified.Replace("[MessageBody]", "Details about the request: </br></br><ul><li>Email: " + uniqueName + "</li><li>Date/time access: " + DateTime.Now + "</li></ul>");

            //// Replacing the generic callout box.
            //StructureModified.Replace("[MessageCallout]", "For more details about the request, send a message to <strong>arda@microsoft.com</strong>.");

            //// Creating a object that will send the message.
            //EmailLogic EmailObject = new EmailLogic();

            //try
            //{
            //    var EmailTask = EmailObject.SendEmailAsync(ToName, ToEmail, Subject, StructureModified.ToString());
            //    return true;
            //}
            //catch (Exception)
            //{
            //    return false;
            //}
        }

        // Generate initial and basic permissions set to new users.
        public User CreateNewUserAndSetInitialPermissions(string uniqueName, string name)
        {
            var user = new User()
            {
                UniqueName = uniqueName,
                Name = name,
                Status = PermissionStatus.Waiting_Review,
                UserPermissions = new List<UsersPermission>()
            };

            //Save on Permissions
            _context.Users.Add(user);
            _context.SaveChanges();

            //Save on Kanban
            var kanbanUser = new UserKanbanViewModel()
            {
                UniqueName = user.UniqueName,
                Name = user.Name
            };
            var res = Util.ConnectToRemoteService(HttpMethod.Post, Util.KanbanURL + "api/user/add", "kanban", "kanban", kanbanUser).Result;

            if (res.IsSuccessStatusCode)
            {
                return user;
            }
            else
            {
                return null;
            }
        }

        public string GetUserMenuSerialized(string uniqueName)
        {
            var menu = new List<Tuple<string, Tuple<string, string, string>>>();
            string propertiesSerializedCached = null;
            byte[] arraySerializedCached = null;

            try
            {
                arraySerializedCached = _cache.Get(uniqueName);
            }
            catch (StackExchange.Redis.RedisConnectionException)
            {
                // Ignore transient network failures
            }


            if (arraySerializedCached != null)
            {
                propertiesSerializedCached = Util.GetString(arraySerializedCached);
            }
            else
            {
                var cachedView = GetUserPermissionsAndCode(uniqueName, string.Empty);
                propertiesSerializedCached = cachedView.ToString();
            }

            try
            {
                _cache.Set(uniqueName, Util.GetBytes(propertiesSerializedCached));
            }
            catch (StackExchange.Redis.RedisConnectionException)
            {
                // Ignore transient network failures
            }

            //try
            //{
            //    propertiesSerializedCached = Util.GetString(_cache.Get(uniqueName));
            //}
            //catch (Exception)
            //{
            //    SetUserPermissionsAndCode(uniqueName, string.Empty);
            //    propertiesSerializedCached = Util.GetString(_cache.Get(uniqueName));
            //}

            var permissions = new CacheViewModel(propertiesSerializedCached).Permissions;

            foreach (var p in permissions)
            {
                if (!p.Endpoint.Contains("/api"))
                {
                    string category = p.Category;
                    string display = p.DisplayName;
                    string controller = p.Module;
                    string action = p.Resource;
                    //string url = p.Endpoint + "/" + p.Module + "/" + p.Resource;

                    menu.Add(Tuple.Create(category, Tuple.Create(display, controller, action)));
                }
            }

            var menuGrouped = (from m in menu
                               group m.Item2 by m.Item1 into g
                               select new
                               {
                                   Category = g.Key,
                                   Items = g.ToList()
                               }).ToList();


            return JsonConvert.SerializeObject(menuGrouped);
        }

        public PermissionStatus GetUserStatus(string uniqueName)
        {
            return _context.Users.First(u => u.UniqueName == uniqueName).Status;
        }

        public bool VerifyIfUserAdmin(string uniqueName)
        {
            var propertiesSerializedCached = Util.GetString(_cache.Get(uniqueName));
            var permissions = new CacheViewModel(propertiesSerializedCached).Permissions;

            var permToReview = permissions.FirstOrDefault(p => p.Module == "Users" && p.Resource == "Review");
            if (permToReview != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetNumberOfUsersToApprove()
        {
            return _context.Users.Where(u => u.Status == PermissionStatus.Waiting_Review).Count();
        }

        public IEnumerable<UserMainViewModel> GetPendingUsers()
        {
            var data = from users in _context.Users
                       where users.Status == PermissionStatus.Waiting_Review
                       select new UserMainViewModel
                       {
                           Name = users.Name,
                           Email = users.UniqueName,
                           Status = (int)users.Status
                       };

            return data.ToArray();
        }

        public IEnumerable<ResourcesViewModel> GetAllPermissions()
        {
            var data = (from r in _context.Resources
                        orderby r.CategorySequence, r.ResourceSequence
                        group r.DisplayName by r.Category into g
                        select new ResourcesViewModel
                        {
                            Category = g.Key,
                            Resources = g.ToList()
                        }).ToList();

            return data;
        }

        public PermissionsViewModel GetUserPermissions(string uniqueName)
        {
            var data = (from up in _context.UsersPermissions
                        join r in _context.Resources on up.ResourceID equals r.ResourceID
                        where up.UniqueName == uniqueName
                        orderby r.CategorySequence, r.ResourceSequence
                        select new Permission
                        {
                            category = r.Category,
                            resource = r.DisplayName
                        }).ToList();

            var permissions = new PermissionsViewModel()
            {
                permissions = data
            };

            return permissions;
        }

        public IEnumerable<UserMainViewModel> GetUsers()
        {
            var data = from users in _context.Users
                       select new UserMainViewModel
                       {
                           Name = users.Name,
                           Email = users.UniqueName,
                           Status = (int)users.Status
                       };

            return data.ToArray();
        }

        public UserMainViewModel GetUser(string uniqueName)
        {
            var data = (from user in _context.Users
                        where user.UniqueName == uniqueName
                        select new UserMainViewModel
                        {
                            Name = user.Name,
                            Email = user.UniqueName,
                            Status = (int)user.Status
                        }).First();

            return data;
        }

        public string GetUserPhotoFromCache(string uniqueName)
        {
            var key = "photo_" + uniqueName;
            byte[] arr = _cache.Get(key);

            return Util.GetString(arr);
        }

        public bool SaveUserPhotoOnCache(string uniqueName)
        {
            var user = _context.Users.First(u => u.UniqueName == uniqueName);

            if (user != null)
            {
                //Get photo from database:
                var photo = user.PhotoBase64;
                //Cache it:
                CacheUserPhoto(uniqueName, photo);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}