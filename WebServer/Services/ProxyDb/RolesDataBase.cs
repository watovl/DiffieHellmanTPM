using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServer.Models;

namespace WebServer.Services.ProxyDb {
    public class RolesDataBase : IRolesDataBase {
        private DataBaseContext Context;

        public RolesDataBase(DataBaseContext context) {
            Context = context;
        }

        public Task<IEnumerable<Role>> GetAllAsync() {
            return Task.FromResult(Context.Roles.Include(r => r.Users).AsEnumerable());
        }

        public async Task<Role> GetRoleAsync(string name) {
            return await Context.Roles.Include(r => r.Users).FirstOrDefaultAsync(role => role.Name == name);
        }

        public async Task<Role> GetAdminAsync() {
            return await GetRoleAsync("admin");
        }

        public async Task<Role> GetUserAsync() {
            return await GetRoleAsync("user");
        }
    }
}
