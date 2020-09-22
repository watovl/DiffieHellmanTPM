using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using WebService.Services.Authentication;
using WebService.Services.Hubs;
using WebService.Services.Hashing;
using WebService.Services;
using WebService.Models;

namespace WebService {
    public class Startup {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services) 
        {
            /* Подключение базы данных */
            // добавляем контекст UsersContext в качестве сервиса в приложение
            services.AddDbContext<UsersContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                    });
                });
            services.AddTransient<IUsersDataBase, UsersDataBase>();

            /* Добавление листа запросов на синхронизацию */
            ListRequests SyncRequestsList = new ListRequests();
            services.AddSingleton(SyncRequestsList);
            // фоновая очистка списка запросов на синхронизацию
            services.AddHostedService<ClearingRequests>();

            /* Настройка jWT (токенов) */
            //string signingSecurityKey = Convert.ToString(Salt.Create());
            var signingKey = new SigningSymmetricKey(AuthOptions.KEY);
            services.AddSingleton<IJwtSigningEncodingKey>(signingKey);

            var signingDecodingKey = (IJwtSigningDecodingKey)signingKey;
            const string jwtSchemeName = "JwtBearer";
            services
                .AddAuthentication(options => {
                    options.DefaultAuthenticateScheme = jwtSchemeName;
                    options.DefaultChallengeScheme = jwtSchemeName;
                })
                .AddJwtBearer(jwtSchemeName, jwtBearerOptions => {
                    // настройка записи токена в строку запроса (для работы с сокетами)
                    jwtBearerOptions.Events = new JwtBearerEvents {
                        OnMessageReceived = context => {
                            var accessToken = context.Request.Query["access_token"];
                            // если запрос направлен хабу
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/protocoldh"))) {
                                // получаем токен из строки запроса
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                    // настройка параметров токена
                    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = signingDecodingKey.GetKey(),

                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.ISSUER,

                        ValidateAudience = true,
                        ValidAudience = AuthOptions.AUDIENCE,

                        ValidateLifetime = true,

                        ClockSkew = TimeSpan.FromSeconds(5)
                    };
                });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSignalR(options => {
                options.ClientTimeoutInterval = TimeSpan.FromSeconds(600);
                //options.HandshakeTimeout = TimeSpan.FromSeconds(120);
                options.KeepAliveInterval = TimeSpan.FromSeconds(60);
            });
            services.AddSingleton<IUserIdProvider, UserHubIdProvider>();
        }
                
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Users}/{action=GetAllUsers}");
            });

            app.UseSignalR(routes => {
                routes.MapHub<HubProtocolDH>("/protocoldh");
            });
        }
    }
}
