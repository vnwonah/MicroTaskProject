using System;
using System.Data.SqlClient;
using System.IO;
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
using MT_NetCore_API.Swagger;
using MT_NetCore_Common.Interfaces;
using MT_NetCore_Common.Repositories;
using MT_NetCore_Common.Utilities;
using MT_NetCore_Data.CatalogDB;
using MT_NetCore_Data.IdentityDB;
using MT_NetCore_Data.TenantsDB;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.Swagger;
using static MT_NetCore_Common.Utilities.AppConfig;



/*=================================================
 *          Essential Commands
 * ================================================
 * Before running any of these commands set ASPNETCORE_ENVIRONMENT variable first.
 * This lets entity framework know which configuration files and connection string to use for the database operations
 * 
 * How to set:
 * Mac OS: export ASPNETCORE_ENVIRONMENT=<Environment Name> eg: export ASPNETCORE_ENVIRONMENT=Production
 * Windows CMD: set ASPNETCORE_ENVIRONMENT=<Environment Name> eg: set ASPNETCORE_ENVIRONMENT=Production
 * Windows CMD 2: setx ASPNETCORE_ENVIRONMENT=<Environment Name> eg: setx ASPNETCORE_ENVIRONMENT=Production.
 * Use second command if first fails. 
 * 
 * Info: change directory into API Project first with cd MT-NetCore-API before running Migrations and DB
 * Update Commands
 * 
 * Migrations are Added Per DB Context
 * 
 * Sample Migration Command: 
 * > dotnet ef migrations add "Initial Migration" --context TenantDbContext --project ../MT-NetCore-Data
 * -------------------------------------------------
 * Database is Updated Per DB Context
 * 
 * Sample Update Command:
 * > dotnet ef database update --context TenantDbContext --project ../MT-NetCore-Data
 * 
 * You can add the verbose flag to commands to see whats going on with --verbose
 * eg: > dotnet ef database update --context TenantDbContext --project ../MT-NetCore-Data --verbose
 */

namespace MT_NetCore_API
{
    public class Startup
    {
        private const string SecretKey = "iNivDmHLpUA223sqsfhqGbMRdRj1PVkH"; // todo: get this from somewhere secure
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            ReadAppConfig(configuration);
        }
        private static IUtilities _utilities;
        private static ICatalogRepository _catalogRepository;
        private static ITenantRepository _tenantRepository;


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

            //Uncomment Before Running Migrations on Tenant Context.
            services.AddDbContext<TenantDbContext>(options => options.UseSqlServer(GetTenantConnectionString(TenantServerConfig, DatabaseConfig),
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
                options.SwaggerDoc("v1", new Info
                {
                    Title = "MobileForms API",
                    Version = "v2.0.0",
                    Description = "Mobile Forms Africa REST API",
                    Contact = new Contact
                    {
                        Name = "MFA Developers",
                        Email = "dev@crowdforce.io",
                        Url = "https://crowdforce.io/help"
                    }
                });
                options.AddSecurityDefinition("oauth2", new ApiKeyScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                    In = "header",
                    Name = "Authorization",
                    Type = "apiKey"
                });

                var basePath = AppContext.BaseDirectory;
                var assemblyName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
                var fileName = Path.GetFileName(assemblyName + ".xml");
                options.IncludeXmlComments(System.IO.Path.Combine(basePath, fileName));

                options.SchemaFilter<SchemaFilter>();
                options.OperationFilter<HeaderFilter>();
                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            services.AddMvc();
            services.TryAddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IJwtFactory, JwtFactory>();
            services.AddScoped<IUserService, UserService>();
            //Add Application services
            services.AddTransient<ICatalogRepository, CatalogRepository>();
            services.AddTransient<ITenantRepository, TenantRepository>();
            services.AddSingleton<ITenantRepository>(p => new TenantRepository(GetBasicSqlConnectionString(), Configuration));
            services.AddSingleton(Configuration);
            services.AddSingleton<IRequestContext, RequestContextAdapter>();


            //create instance of utilities class
            services.AddTransient<IUtilities, Utilities>();
            var provider = services.BuildServiceProvider();
            _utilities = provider.GetService<IUtilities>();
            _catalogRepository = provider.GetService<ICatalogRepository>();
            _tenantRepository = provider.GetService<ITenantRepository>();
        }

      
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            if (env.IsDevelopment() || env.Equals("Local"))
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
            app.UseResponseWrapper();
            app.UseAuth();
            app.UseMvc();

            //shard management
            InitialiseShardMapManager();
            _utilities.RegisterTenantShard(TenantServerConfig, DatabaseConfig, CatalogConfig);
        }

        private string GetCatalogConnectionString(CatalogConfig catalogConfig, DatabaseConfig databaseConfig)
        {
            return $"Server=tcp:{catalogConfig.CatalogServer},1433;Database={catalogConfig.CatalogDatabase};User ID={databaseConfig.DatabaseUser};Password={databaseConfig.DatabasePassword};Trusted_Connection=False;Encrypt=False;";
        }

        private string GetTenantConnectionString(TenantServerConfig tenantServerConfig, DatabaseConfig databaseConfig)
        {
            return $"Server=tcp:{tenantServerConfig.TenantServer},1433;Database={tenantServerConfig.TenantDatabase};User ID={databaseConfig.DatabaseUser};Password={databaseConfig.DatabasePassword};Trusted_Connection=False;Encrypt=False;";
        }

        private void InitialiseShardMapManager()
        {
            var basicConnectionString = GetBasicSqlConnectionString();
            SqlConnectionStringBuilder connectionString = new SqlConnectionStringBuilder(basicConnectionString)
            {
                DataSource = DatabaseConfig.SqlProtocol + ":" + CatalogConfig.CatalogServer + "," + DatabaseConfig.DatabaseServerPort,
                InitialCatalog = CatalogConfig.CatalogDatabase
            };

            var sharding = new Sharding(CatalogConfig.CatalogDatabase, connectionString.ConnectionString, _catalogRepository, _tenantRepository, _utilities);
        }

        private string GetBasicSqlConnectionString()
        {
            var connStrBldr = new SqlConnectionStringBuilder
            {
                UserID = DatabaseConfig.DatabaseUser,
                Password = DatabaseConfig.DatabasePassword,
                ApplicationName = "EntityFramework",
                ConnectTimeout = DatabaseConfig.ConnectionTimeOut
            };

            return connStrBldr.ConnectionString;
        }

        private void ReadAppConfig(IConfiguration configuration)
        {
  
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
