using Arda.Common.Models.Permissions;
using Microsoft.EntityFrameworkCore;

namespace Arda.Kanban.Models
{
    public class PermissionsContext : DbContext
    {
        public PermissionsContext(DbContextOptions<PermissionsContext> options)
            : base(options)
        { }


        public DbSet<Module> Modules { get; set; }

        public DbSet<Resource> Resources { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UsersPermission> UsersPermissions { get; set; }

    }
}
