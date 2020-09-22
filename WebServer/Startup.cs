using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebServer.Services.Hubs;

namespace WebServer {
    public partial class Startup {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSignalR(options => {
                options.ClientTimeoutInterval = TimeSpan.FromSeconds(60);
                //options.HandshakeTimeout = TimeSpan.FromSeconds(120);
                options.KeepAliveInterval = TimeSpan.FromSeconds(30);
            });
            services.AddSingleton<IUserIdProvider, UserHubIdProvider>();

            AddServices(services);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc(routes => {
                routes.MapRoute("default", "{controller=Users}/{action=GetAllUsers}");
            });

            app.UseSignalR(routes => {
                routes.MapHub<HubProtocolDH>("/protocoldh");
            });
        }
    }
}
