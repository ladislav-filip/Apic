using System;
using Apic.DependencyInjection;
using Apic.Web.Extensions;
using Apic.Web.Middlewares;
using Autofac;
using Autofac.Extensions.DependencyInjection;
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
	        services.AddAutofac();
	        services.AddApplicationInsightsTelemetry(configuration);
	        services.AddResponseCaching();
	        services.AddCustomizedCors();
			services.AddCustomizedSwagger(configuration);
	        services.AddCustomizedDbContext(configuration);
	        services.AddCustomizedOptions(configuration);
			services.AddCustomizedApiBehaviorOptions();
			services.AddCustomizedMvc();

			ContainerBuilder builder = new ContainerBuilder();
	        builder.RegisterServices(hostingEnvironment, configuration);
	        builder.Populate(services);

	        return new AutofacServiceProvider(builder.Build());
		}

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
	        loggerFactory.AddApplicationInsights(app.ApplicationServices);

	        app.UseResponseCaching();
	        app.UseCustomizedExceptionHandling();
	        app.UseCustomizedHealthMonitor("/hc");
	        app.UseCustomizedSwagger();
	        app.UseCustomizedCors();
			app.UseMvc();
        }
    }
}