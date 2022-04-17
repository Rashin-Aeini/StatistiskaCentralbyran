using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using StatistiskaCentralbyran.Models.Data;
using StatistiskaCentralbyran.Models.Interfaces;
using StatistiskaCentralbyran.Models.Repositories;
using StatistiskaCentralbyran.Models.Settings;
using StatistiskaCentralbyran.Models.Workers;

namespace StatistiskaCentralbyran
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDatabase>(
                options => options.UseInMemoryDatabase("SCB")
                );

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddTransient<Centralbyran>(
                provider => Configuration
                .GetSection(nameof(Centralbyran))
                .Get<Centralbyran>()
            );

            services.AddControllers();

            services.AddScoped<FetchingDataWorker>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "StatistiskaCentralbyran", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "StatistiskaCentralbyran v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
