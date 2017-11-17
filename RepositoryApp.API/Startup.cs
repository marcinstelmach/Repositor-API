using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using RepositoryApp.Data.DAL;
using RepositoryApp.Service.Providers;
using RepositoryApp.Service.Services.Implementations;
using RepositoryApp.Service.Services.Interfaces;
using Swashbuckle.AspNetCore.Swagger;

namespace RepositoryApp.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ConnectionString")));

            services.AddAuthentication(cfg =>
                {
                    cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    cfg.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = true;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidAudience = Configuration["Tokens:Issuer"],
                        ValidIssuer = Configuration["Tokens:Issuer"],
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"]))
                    };
                });

            services.AddMvc(
                    setupAction =>
                    {
                        setupAction.ReturnHttpNotAcceptable = true;
                        setupAction.OutputFormatters.Add(new XmlSerializerOutputFormatter());
                        setupAction.InputFormatters.Add(new XmlDataContractSerializerInputFormatter());
                    })
                .AddJsonOptions(option =>
                    option.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());

            services.AddAutoMapper();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info {Title = "RepositoryApp", Version = "v1"}); });
            services.AddSingleton(Configuration);
            services.AddTransient<IRepositoryService, RepositoryService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IDirectoryService, DirectoryService>();
            services.AddTransient<IVersionService, VersionService>();
            services.AddTransient<IFileService, FileService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseAuthentication();
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
            });
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Repository App"); });
            app.UseMvc();


            //DbInitializeProvider.InitializeWithDefaults(dbContext);
        }
    }
}