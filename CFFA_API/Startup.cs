using Algorithm;
using CFFA_API.Controllers.Helpers;
using CFFA_API.Controllers.Helpers.EmailSender;
using CFFA_API.Logic.Implementations;
using CFFA_API.Logic.Interfaces;
using CFFA_API.Models;
using CFFA_API.Repository.Implementations;
using CFFA_API.Repository.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Text;

namespace CFFA_API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {

            Configuration = configuration;
            AppSettings.Secret = Configuration["AppSettings:Secret"];
            AppSettings.EmailSenderAddress = Configuration["AppSettings:EmailSenderAddress"];
            AppSettings.EmailSenderPassword = Configuration["AppSettings:EmailSenderPassword"];
            AppSettings.SQLConnectionString = Configuration["ConnectionStrings:DefaultConnection"];
        }

        public IConfiguration Configuration { get; }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //var appSettings = new AppSettings();
            //Configuration.Bind(nameof(appSettings), appSettings);
            //services.AddSingleton<IAppSettings, AppSettings>();


            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseLazyLoadingProxies().UseSqlServer(AppSettings.SQLConnectionString));//toDo: APPSETTINGS
            services.AddDbContext<DbContext, ApplicationDbContext>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //services.AddIdentity<ApplicationUser, IdentityRole>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>()
            //    .AddDefaultTokenProviders();
            services.AddTransient<IPostRepository, PostRepository>();
            services.AddTransient<ICommentRepository, CommentRepository>();
            services.AddTransient<ITagRepository, TagRepository>();
            services.AddTransient<IUserRepository, UserRepository>();

            services.AddTransient<IPostBehaviour, PostBehaviour>();
            services.AddTransient<ICommentBehaviour, CommentBehaviour>();
            services.AddTransient<IUserBehaviour, UserBehaviour>();

            services.AddTransient<IPhotoManager, PhotoManager>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IUsage, Usage>();

            //TODO: add dependencyies
            //services.AddCors(options =>
            //{ //http://localhost:56107
            //    options.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials().Build());
            //});


            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().Build();
                    });
            });

            services.AddControllers();
            services.AddIdentityCore<ApplicationUser>(config =>
            {
                config.SignIn.RequireConfirmedEmail = true;
                config.User.RequireUniqueEmail = true;
                config.Password.RequireDigit = false;
                config.Password.RequireLowercase = false;
                config.Password.RequiredUniqueChars = 0;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
            }).AddDefaultTokenProviders()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddTokenProvider("Default", typeof(DataProtectorTokenProvider<ApplicationUser>));
            //services.AddIdentityCore<identityuser> services.AddIdentityCore<IdentityUser>(cfg =>
            //{
            //    cfg.User.RequireUniqueEmail = true;
            //}).AddEntityFrameworkStores<StoreContext>();


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    SaveSigninToken = false,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = AppSettings.Issuer,
                    ValidAudience = AppSettings.Audition,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AppSettings.Secret)),
                    RequireExpirationTime = false
                };
            }).AddCookie("JwtBearerDefaults.AuthenticationScheme");

            services.AddLogging();
            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "API",
                    Version = "v1"
                });

                c.AddSecurityDefinition("Bearer", //Name the security scheme
                    new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme.",
                        Type = SecuritySchemeType.Http, //We set the scheme type to http since we're using bearer authentication
                        Scheme = "bearer" //The name of the HTTP Authorization scheme to be used in the Authorization header. In this case "bearer".
                    });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement{
                    {
                        new OpenApiSecurityScheme{
                            Reference = new OpenApiReference{
                                Id = "Bearer", //The name of the previously defined security scheme.
                                Type = ReferenceType.SecurityScheme
                            }
                        },new List<string>()
                    }
                });
            });
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            } else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();

            app.UseCors(options => options.AllowAnyOrigin());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test API V1");
            });

            app.Run(async (context) => {
                await context.Response.WriteAsync("Could not find anything");
            });
        }
    }
}
