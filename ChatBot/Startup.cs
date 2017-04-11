using ChatBot.Data;
using ChatBot.Data.Infrastructure;
using ChatBot.Data.Respositories;
using ChatBot.Infrastructure;
using ChatBot.Infrastructure.Mappings;
using ChatBot.Model.Models;
using ChatBot.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace ChatBot
{
    public class Startup
    {
        private readonly SecurityKey _securityKey;

        private static string _applicationPath = string.Empty;
        private static string _contentRootPath = string.Empty;

        public Startup(IHostingEnvironment env)
        {
            _applicationPath = env.WebRootPath;
            _contentRootPath = env.ContentRootPath;
            // Setup configuration sources.

            var builder = new ConfigurationBuilder()
                .SetBasePath(_contentRootPath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // This reads the configuration keys from the secret store.
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                //builder.AddUserSecrets();
                builder.AddUserSecrets<Startup>();
            }
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();

            var cert = new X509Certificate2(Path.Combine(env.ContentRootPath, "people.pfx"));
            _securityKey = new X509SecurityKey(cert);
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            string sqlConnectionString = Configuration["ConnectionStrings:DefaultConnection"];
            bool useInMemoryProvider = bool.Parse(Configuration["Data:ChatBotDBConnection:InMemoryProvider"]);

          //  services.AddDbContext<ChatBotDbContext>(options => options.UseSqlServer(@"Data Source=DESKTOP-SD7L5A2\PC;Initial Catalog=ChatBotDB;Integrated Security=False;User Id=sa;Password=123456;MultipleActiveResultSets=True;"));
            services.AddDbContext<ChatBotDbContext>(options => options.UseSqlServer(@"Data Source=27.0.12.24;Initial Catalog=DEFACEWEBSITE;Integrated Security=False;User Id=deface;Password=123456;MultipleActiveResultSets=True;"));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
               // options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
               // options.Password.RequireNonLetterOrDigit = true;
            //    options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
            })
          .AddEntityFrameworkStores<ChatBotDbContext>()
          .AddDefaultTokenProviders();

            // Repositories
            //services.AddScoped<IUserRepository, UserRepository>();
            //services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            //services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ILoggingRepository, LoggingRepository>();
        //    services.AddScoped<IBotDomainRepository, BotDomainRepository>();

            //Services

          //  services.AddScoped<IMembershipService, MembershipService>();
            //services.AddScoped<IEncryptionService, EncryptionService>();
            //services.AddScoped<IBotDomainService, BotDomainService>();
            services.AddAuthentication();

            //    services.AddCors();
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
            //  Polices
            //services.AddAuthorization(options =>
            //{
            //    // inline policies
            //    options.AddPolicy("AdminOnly", policy =>
            //    {
            //        policy.RequireClaim(ClaimTypes.Role, "Admin");
            //    });

            //});

            // Add MVC services to the services container.
            services.AddMvc()
            .AddJsonOptions(opt =>
            {
                var resolver = opt.SerializerSettings.ContractResolver;
                if (resolver != null)
                {
                    var res = resolver as DefaultContractResolver;
                    res.NamingStrategy = null;
                }
            });

            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(_securityKey, SecurityAlgorithms.RsaSha256);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // this will serve up wwwroot
            app.UseStaticFiles();
            // app.UseCors("CorsPolicy");
            app.UseCors(builder =>
              builder.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());

            //app.UseExceptionHandler(
            // builder =>
            // {
            //     builder.Run(
            //       async context =>
            //       {
            //           context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //           context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

            //           var error = context.Features.Get<IExceptionHandlerFeature>();
            //           if (error != null)
            //           {
            //               context.Response.AddApplicationError(error.Error.Message);
            //               await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
            //           }
            //       });
            // });

            AutoMapperConfiguration.Configure();

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true
            });

            app.UseIdentity();
            // Custom authentication middleware
            // app.UseMiddleware<AuthMiddleware>();

            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],
                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _securityKey,
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = tokenValidationParameters
            });

            // Add MVC to the request pipeline.
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                //routes.MapRoute(
                //    name: "default1",
                //    template: "{controller=Home}/{action=Index}/{username?}");
                // Uncomment the following line to add a route for porting Web API 2 controllers.
                //routes.MapWebApiRoute("DefaultApi", "api/{controller}/{id?}");
            });

            DbInitializer.Initialize(app.ApplicationServices, _applicationPath);
        }

        // Entry point for the application.
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
              .UseKestrel()
              .UseContentRoot(Directory.GetCurrentDirectory())
              .UseIISIntegration()
              .UseStartup<Startup>()
              .Build();
            host.Run();
        }
    }
}