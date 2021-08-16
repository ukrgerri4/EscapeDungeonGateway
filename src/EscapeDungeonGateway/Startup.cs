using EscapeDungeonGateway.Extensions;
using EscapeDungeonGateway.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net;

namespace EscapeDungeonGateway
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly IHostEnvironment env;

        public Startup(IConfiguration configuration, IHostEnvironment env)
        {
            this.configuration = configuration;
            this.env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            #region Settings
            services.AddSettings(configuration);
            #endregion

            #region HttpClients
            services.AddHttpContextAccessor();
            services.AddHttpClient<ITokenService, TokenService>((provider, client) => {
                var idsSettigs = provider.GetRequiredService<IIdentityServerSettings>();
                client.BaseAddress = new Uri(idsSettigs.AuthorityUrl);
            });
            #endregion

            #region Cors
            services
                .AddCors(options =>
                {
                    var origins = configuration.GetSection("Cors:Origins").Get<string[]>() ?? new string[0];
                    options.AddPolicy("CorsPolicy", builder =>
                    {
                        builder
                        .WithOrigins(origins)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                    });
                });
            #endregion

            #region Authentication
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;

                    // base-address of your identityserver
                    options.Authority = configuration["IdentityServer:AuthorityUrl"];

                    // if you are using API resources, you can specify the name here
                    //options.Audience = "resource1";
                    options.TokenValidationParameters.ValidateAudience = false;

                    // IdentityServer emits a typ header by default, recommended extra check
                    options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
                });
            //.AddAuthentication(options =>
            //{
            //    options.DefaultScheme = "cookie";
            //    options.DefaultChallengeScheme = "oidc";
            //})
            //.AddCookie("cookie")
            //.AddOpenIdConnect("oidc", options =>
            //{
            //    options.Authority = configuration["IdentityServer:AuthorityUrl"];
            //    options.ClientId = configuration["IdentityServer:InteractiveClient:ClientId"];
            //    options.ClientSecret = configuration["IdentityServer:InteractiveClient:ClientSecret"];

            //    var scopes = configuration.GetSection("IdentityServer:InteractiveClient:Scopes").Get<List<string>>();
            //    scopes.ForEach(scope => options.Scope.Add(scope));

            //    options.ResponseType = OpenIdConnectResponseType.Code;
            //    options.UsePkce = true;
            //    options.ResponseMode = OpenIdConnectResponseMode.Query;
            //    options.SaveTokens = true;
            //});
            #endregion

            #region Services
            //services.AddScoped<ITokenService, TokenService>();
            #endregion

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseCors("CorsPolicy");
            // app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapHealthChecks("/health");
                endpoints.MapControllers();
            });
        }
    }
}
