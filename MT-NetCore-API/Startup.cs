using System;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using MT_NetCore_API.Extensions;
using MT_NetCore_API.Helpers;
using MT_NetCore_API.Interfaces;
using MT_NetCore_API.Models.AuthModels;
using MT_NetCore_API.Services;
using MT_NetCore_Data.CatalogDB;
using MT_NetCore_Data.IdentityDB;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.Swagger;


/*=================================================
 *          Essential Commands - Ignore & Perish
 * ================================================
 * Migrations are Added Per DB Context
 * 
 * Sample Micgration Command: 
 * `dotnet ef migrations add "Initial Migration" -s ../MT-NetCore-API --context AuthenticationDbContext`
 * -------------------------------------------------
 * Database is Updated Per DB Context
 * 
 * Sample Update Command:
 * `dotnet ef database update -s ../MT-NetCore-API --context AuthenticationDbContext`
 */

namespace MT_NetCore_API
{
    public class Startup
    {
        private const string SecretKey = "iNivDmHLpUA223sqsfhqGbMRdRj1PVkH"; // todo: get this from somewhere secure
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ReadAppConfig(configuration);
        }

        public IConfiguration Configuration { get; }
        public static DatabaseConfig DatabaseConfig { get; set; }
        public static CatalogConfig CatalogConfig { get; set; }
        public static TenantServerConfig TenantServerConfig { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;
            });

            // api user claim policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiUser", policy => policy.RequireClaim(Constants.Strings.JwtClaimIdentifiers.Rol, Constants.Strings.JwtClaims.ApiAccess));
            });

            services.AddOptions();

            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);


            //Database Contexts
            services.AddDbContext<AuthenticationDbContext>(options => options.UseSqlServer(GetCatalogConnectionString(CatalogConfig, DatabaseConfig),
                sqlServerOptions =>
                {
                    sqlServerOptions.MigrationsAssembly("MT-NetCore-Data");
                }
            ));

            services.AddDbContext<CatalogDbContext>(options => options.UseSqlServer(GetCatalogConnectionString(CatalogConfig, DatabaseConfig),
                sqlServerOptions =>
                {
                    sqlServerOptions.MigrationsAssembly("MT-NetCore-Data");
                }
            ));

            // add identity
            var builder = services.AddIdentityCore<ApplicationUser>(o =>
            {
                // configure identity options
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
            });
            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), builder.Services);
            builder.AddEntityFrameworkStores<AuthenticationDbContext>().AddDefaultTokenProviders();



            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "MobileForms API", Version = "v1" });
                options.AddSecurityDefinition("oauth2", new ApiKeyScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                    In = "header",
                    Name = "Authorization",
                    Type = "apiKey"
                });

                options.OperationFilter<SecurityRequirementsOperationFilter>();

            });

            services.AddMvc();
            services.TryAddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IJwtFactory, JwtFactory>();
        }

      
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseExceptionHandler(
                builder =>
                {
                    builder.Run(
                        async context =>
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                            var error = context.Features.Get<IExceptionHandlerFeature>(); 
                            if (error != null)
                            {
                                //context.Response.AddApplicationError(error.Error.Message);
                                await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
                                }
                            });
                         }
               );

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseCors(x => x
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MobileForms API");
                c.RoutePrefix = string.Empty;
            });
            app.UseAuth();
            app.UseMvc();
        }

        private string GetCatalogConnectionString(CatalogConfig catalogConfig, DatabaseConfig databaseConfig)
        {
            return $"Server=tcp:{catalogConfig.CatalogServer},1433;Database={catalogConfig.CatalogDatabase};User ID={databaseConfig.DatabaseUser};Password={databaseConfig.DatabasePassword};Trusted_Connection=False;Encrypt=False;";
        }

        private void ReadAppConfig(IConfiguration configuration)
        {

            /*
             * 
             *  },
  "DatabaseOptions": {
    "ConnectionTimeOut": "100",
    "CatalogDatabase": "mfacatalog",
    "CatalogServer": "127.0.0.1",
    "DatabasePassword": "Dev@12345",
    "DatabaseServerPort": "1433",
    "DatabaseUser": "SA",
    "ServicePlan": "Standard",
    "TenantServer": "127.0.0.1",
    "TenantDatabase": "mfatenants"
             * 
             */
            DatabaseConfig = new DatabaseConfig
            {
                DatabasePassword = Configuration["DatabaseOptions:DatabasePassword"],
                DatabaseUser = Configuration["DatabaseOptions:DatabaseUser"],
                DatabaseServerPort = Int32.Parse(Configuration["DatabaseOptions:DatabaseServerPort"]),
                SqlProtocol = SqlProtocol.Tcp,
                ConnectionTimeOut = Int32.Parse(Configuration["DatabaseOptions:ConnectionTimeOut"]),
            };

            CatalogConfig = new CatalogConfig
            {
                ServicePlan = Configuration["DatabaseOptions:ServicePlan"],
                CatalogDatabase = Configuration["DatabaseOptions:CatalogDatabase"],
                CatalogServer = Configuration["DatabaseOptions:CatalogServer"], // + ".database.windows.net"
            };

            TenantServerConfig = new TenantServerConfig
            {
                TenantServer = Configuration["DatabaseOptions:CatalogServer"],// + ".database.windows.net",
                TenantDatabase = Configuration["DatabaseOptions:TenantDatabase"],

            };
        }

    }
}
