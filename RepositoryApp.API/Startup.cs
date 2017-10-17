using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using RepositoryApp.Data.DAL;
using RepositoryApp.Data.Model;
using RepositoryApp.Service.Providers;
using RepositoryApp.Service.Services.Implementations;
using RepositoryApp.Service.Services.Interfaces;

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
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, ApplicationRole>(options => { options.User.RequireUniqueEmail = true; })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc(
                    setupAction =>
                    {
                        setupAction.ReturnHttpNotAcceptable = true;
                        setupAction.OutputFormatters.Add(new XmlSerializerOutputFormatter());
                        setupAction.InputFormatters.Add(new XmlDataContractSerializerInputFormatter());
                    })
                .AddJsonOptions(option =>
                    option.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());

            // Register no-op EmailSender used by account confirmation and password reset during development
            // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=532713
            services.AddSingleton<IEmailSender, EmailSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationDbContext dbContext)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseAuthentication();
            app.UseCors(builder => builder.AllowAnyOrigin());
            app.UseMvc();

            DbInitializeProvider.InitializeWithDefaults(dbContext);
        }
    }
}