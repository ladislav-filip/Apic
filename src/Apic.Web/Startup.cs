using System;
using Apic.DependencyInjection;
using Apic.Web.Extensions;
using Apic.Web.Middlewares;
using BeatPulse.UI;
using Castle.Windsor;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Apic.Web
{
    public class Startup
    {
	    private readonly IHostingEnvironment hostingEnvironment;
	    private readonly IConfiguration configuration;
        private readonly ILogger<Startup> logger;

		public Startup(IHostingEnvironment hostingEnvironment, IConfiguration configuration, ILogger<Startup> logger)
		{
			this.hostingEnvironment = hostingEnvironment;
			this.configuration = configuration;
            this.logger = logger;
        }

		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
		    services.AddCors();
		    services.AddCustomizedOptions(configuration);
            services.AddAuthentication();
            services.AddCustomizedAzureStorage();
            services.AddCustomizedAutomapper();
            services.AddApplicationInsightsTelemetry(configuration);
			services.AddCustomizedBeatPulse(configuration);
		    services.AddResponseCaching();
		    services.AddCustomizedCors();
			services.AddCustomizedSwagger(configuration);
		    services.AddCustomizedDbContext(configuration);
		    services.AddHttpsRedirection(x => x.HttpsPort = 443);
            services.AddCustomizedMvc();

			WindsorContainer container = new WindsorContainer();
			container = container.RegisterServices(hostingEnvironment, configuration);

            IServiceProvider provider = WindsorRegistrationHelper.CreateServiceProvider(container, services);

            return provider;
		}

        public void Configure(IApplicationBuilder app)
        {
	        app.UseBeatPulseUI();
	        app.UseHsts();
            app.UseCors();
            app.UseResponseCaching();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
	        app.UseCustomizedExceptionHandling();
            app.UseAuthentication();
            app.UseThrottlingMiddleware();
	        app.UseCustomizedSwagger();
            app.UseCustomizedOptionsMethodMiddleware();
            app.UseCustomizedCors();
            app.UseMvc();
        }
    }
}
