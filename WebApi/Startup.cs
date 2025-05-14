using Domain.Interfaces.Services;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) 
        {
            #region "Pre-Configuration"
            services.AddMvcCore( opt =>
            {
                opt.EnableEndpointRouting = false;
            });
            services.AddDistributedMemoryCache();
            services.AddHttpContextAccessor();
            #endregion

            //Applications Injections

            //Services
            services.AddSingleton<ILoginService, LoginService>();

            //End Application Injections

            #region "Pos-Configuration"
            services.AddMvc().ConfigureApiBehaviorOptions(opt =>
            {
                opt.InvalidModelStateResponseFactory = (context => new BadRequestObjectResult("Invalid call!"));
            });
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Populate;
                options.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            services.AddCors(o =>
            {
                o.AddPolicy("LocalCorsPolicy", builder =>
                {
                    builder.AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .AllowAnyMethod();
                });
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Dale API", Version = "v0"});
            });
            #endregion

            ConfigureCors(services);
        }

        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("LocalCorsPolicy");

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("v1/swagger.json", "API"); });
            app.UseRewriter(new RewriteOptions().AddRedirect("^", "/swagger"));
        }

        private static void ConfigureCors(IServiceCollection services)
        {
            services.AddCors( o =>
            {
                o.AddPolicy("CorsPolicy", builder =>
                {
                    builder.AllowAnyMethod()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .AllowAnyHeader();
                });
                o.AddPolicy("LocalCorsPolicy", builder =>
                {
                    builder.AllowAnyMethod()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .AllowAnyHeader();
                });
            });
        }
    }
}
