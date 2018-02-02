using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CryptoBackend.Utils;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace CryptoBackend
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHangfire(config =>
                config.UsePostgreSqlStorage(Config.Default.ConnectionString));
            services.AddCors();
            services.AddMvc();
            services.AddSwaggerGen(config =>
                config.SwaggerDoc("v1", new Info { Title = "CoinArbitrageAPI", Version = "v1" }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            app.UseMvc();
            app.UseHangfireServer();
            app.UseHangfireDashboard();
            app.UseSwagger();
            app.UseSwaggerUI(config =>
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "CoinArbitrageAPI v1"));
        }
    }
}
