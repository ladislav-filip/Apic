using System;
using Apic.DependencyInjection;
using Apic.Web.Extensions;
using Apic.Web.Middlewares;
using BeatPulse.UI;
using Castle.Windsor;
using Castle.Windsor.MsDependencyInjection;
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

		public Startup(IHostingEnvironment hostingEnvironment, IConfiguration configuration)
		{
			this.hostingEnvironment = hostingEnvironment;
			this.configuration = configuration;
		}

		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
		    services.AddCustomizedAutomapper();
            services.AddApplicationInsightsTelemetry(configuration);
			services.AddBeatPulseUI();
			services.AddCustomizedBeatPulseHealthCheck(configuration);
		    services.AddResponseCaching();
		    services.AddCustomizedCors();
			services.AddCustomizedSwagger(configuration);
		    services.AddCustomizedDbContext(configuration);
		    services.AddCustomizedOptions(configuration);
			services.AddCustomizedMvc();

			WindsorContainer container = new WindsorContainer();
			container = container.RegisterServices(hostingEnvironment, configuration);
			return WindsorRegistrationHelper.CreateServiceProvider(container, services);
		}

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
	        loggerFactory.AddApplicationInsights(app.ApplicationServices);

	        app.UseBeatPulseUI();
	        app.UseResponseCaching();
	        app.UseCustomizedExceptionHandling();
	        app.UseCustomizedSwagger();
	        app.UseCustomizedCors();
			app.UseMvc();
        }
    }
}
