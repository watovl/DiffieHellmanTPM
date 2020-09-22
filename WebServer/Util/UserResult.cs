using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebServer.Models;

namespace WebServer.Util {
    public class UserResult : IActionResult {
        private readonly User User;

        public UserResult(User user) {
            User = user;
        }

        public async Task ExecuteResultAsync(ActionContext context) {
            UserResultModel userResult = new UserResultModel { Id = User.Id, Name = User.Name, RoleName = User.Role.Name };
            var objectResult = new ObjectResult(userResult);
            await objectResult.ExecuteResultAsync(context);
        }
    }
}
