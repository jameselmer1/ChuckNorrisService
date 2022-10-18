using ChuckNorrisApi.Models;
using ChuckNorrisApi.Repository;
using ChuckNorrisApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace ChuckNorrisApi
{
    public class Startup
    {
        private const string CorsApiPolicyName = "CorsCantBlockChuckNorris";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: CorsApiPolicyName,
                    policy => policy.WithOrigins("http://localhost:4200")
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            services.AddHttpClient("cnClient", client =>
             {
                 client.Timeout = TimeSpan.FromSeconds(30);
             });

            services.AddControllers();
            services.Configure<ChuckNorrisApiSettings>(Configuration.GetSection("ChuckNorrisApiSettings"));
            services.AddOptions();

            services.AddScoped<IChuckNorrisRepository, ChuckNorrisRepository>();
            services.AddScoped<IChuckNorrisService, ChuckNorrisService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(CorsApiPolicyName);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
