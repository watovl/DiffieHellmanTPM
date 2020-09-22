using System.Collections.Generic;
using System.Threading.Tasks;
using WebServer.Models;

namespace WebServer.Services.ProxyDb {
    public interface IRolesDataBase {
        Task<IEnumerable<Role>> GetAllAsync();
        Task<Role> GetRoleAsync(string name);
        Task<Role> GetAdminAsync();
        Task<Role> GetUserAsync();
    }
}
