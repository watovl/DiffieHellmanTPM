using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using WebServer.Services.Authentication;
using WebServer.Services.Hubs;
using WebServer.Services.ProxyDb;
using WebServer.Models;

namespace WebServer {
    public partial class Startup{
        private void AddServices(IServiceCollection services) {
            /* Подключение базы данных */
            // добавляем контекст UsersContext в качестве сервиса в приложение
            services.AddDbContext<DataBaseContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                    sqlServerOptionsAction: sqlOptions => {
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                    });
            });
            services.AddTransient<IUsersDataBase, UsersDataBase>();
            services.AddTransient<IRolesDataBase, RolesDataBase>();

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
        }
    }
}
