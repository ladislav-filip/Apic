using System;
using Apic.Data.Repositories;
using Apic.DependencyInjection;
using Apic.Facades.Customers;
using Apic.Facades.Documents;
using Apic.Web.Cors;
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
using Swashbuckle.AspNetCore.SwaggerUI;

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
			services.AddCustomizedCors();
		    services.AddCustomizedOptions(configuration);
            services.AddAuthentication();
            services.AddCustomizedAzureStorage();
            services.AddCustomizedAutomapper();
            services.AddApplicationInsightsTelemetry(configuration);
			services.AddCustomizedBeatPulse(configuration);
		    services.AddResponseCaching();
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
	        app.UseMiddleware<ErrorHandlingMiddleware>();
	        
	        app.UseHsts();
	        app.UseHttpsRedirection();
	        
	        app.UseBeatPulse(options =>
	        {
		        options.ConfigurePath("hc")
			        .ConfigureTimeout(1500)
			        .ConfigureDetailedOutput(true, true);
	        });
	        
	        app.UseBeatPulseUI();
	        
	        app.UseSwagger();
	        app.UseSwaggerUI(x =>
	        {
		        x.SwaggerEndpoint("/swagger/v1/swagger.json", "API Documentation");
		        x.InjectStylesheet("css/swagger.css");
		        x.DisplayOperationId();
		        x.EnableFilter();
		        x.DocExpansion(DocExpansion.List);

		        x.RoutePrefix = string.Empty;
	        });
	        
	        app.UseCors(CorsPolicies.Default);
            app.UseResponseCaching();
            app.UseStaticFiles();	
            
            app.UseAuthentication();
            
            app.UseMiddleware<ThrottlingMiddleware>();
            
            app.UseMvc();
        }
    }
}
