using System.Collections.Generic;
using Arda.Permissions.ViewModels;

namespace Arda.Permissions.Models.Repositories
{
    public interface IPermissionRepository
    {
        // Save the permissions and code at the cache.
        bool SetUserPermissionsAndCode(string uniqueName, string code);

        // Update an existing user permissions.
        bool UpdateUserPermissions(string uniqueName, PermissionsViewModel userPermission);

        // Update an existing user photo.
        bool UpdateUserPhoto(string uniqueName, string photo);

        // Update an existing user permissions.
        bool UpdateUser(string uniqueName, UserMainViewModel user);

        // Delete an existing user permissions from the cache.
        void DeleteUserPermissions(string uniqueName);

        void DeleteUser(string uniqueName);

        // Verify if user has authorization to specific resource.
        bool VerifyUserAccessToResource(string uniqueName, string module, string resource);

        // Verify if user exists in UserPermissions table.
        bool VerifyIfUserIsInUserPermissionsDatabase(string uniqueName);

        // Send a notification about new user to administrator.
        bool SendNotificationOfNewUserByEmail(string uniqueName);

        // Set basic permissions to new users.
        User CreateNewUserAndSetInitialPermissions(string uniqueName, string name);

        // Create the user menu based on his/her permissions.
        string GetUserMenuSerialized(string uniqueName);

        PermissionStatus GetUserStatus(string uniqueName);

        bool VerifyIfUserAdmin(string uniqueName);

        int GetNumberOfUsersToApprove();

        IEnumerable<UserMainViewModel> GetPendingUsers();

        IEnumerable<ResourcesViewModel> GetAllPermissions();

        PermissionsViewModel GetUserPermissions(string uniqueName);

        IEnumerable<UserMainViewModel> GetUsers();

        UserMainViewModel GetUser(string uniqueName);

        void CacheUserPhoto(string uniqueName, string PhotoBase64);

        bool SaveUserPhotoOnCache(string uniqueName);

        string GetUserPhotoFromCache(string uniqueName);
    }
}