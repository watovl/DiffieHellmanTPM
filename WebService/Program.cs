using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace WebService {
    public class Program {
        public static void Main(string[] args) {
            try {
                CreateWebHostBuilder(args).Build().Run();
            }
            catch(Exception ex) {
                Debug.WriteLine(ex.Message);
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging(logging => {
                    //logging.AddFilter("Microsoft.AspNetCore.SignalR", LogLevel.Debug);
                    //logging.AddFilter("Microsoft.AspNetCore.Http.Connections", LogLevel.Debug);
                })
                .UseStartup<Startup>();
    }
}
