using Apic.Common.Attributes;
using Apic.Common.Exceptions;
using Apic.Data.Repositories;
using Apic.Facades.Customers;
using Apic.Facades.Mappers.Resolvers;
using Apic.Services;
using Apic.Services.Throttling;
using Apic.Web.Cors;
using Apic.Web.Extensions;
using Apic.Web.Middlewares;
using BeatPulse.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Apic.Web
{
    public class Startup
    {
	    private readonly IWebHostEnvironment hostingEnvironment;
	    private readonly IConfiguration configuration;

		public Startup(IWebHostEnvironment hostingEnvironment, IConfiguration configuration)
		{
			this.hostingEnvironment = hostingEnvironment;
			this.configuration = configuration;
        }

		public void ConfigureServices(IServiceCollection services)
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
            services.AddCustomizedControllers();

            services.Scan(selector => selector
	            .FromAssemblyOf<IApiException>()
	            .AddClasses(x => x.AssignableTo<IApiException>())
	            .AsImplementedInterfaces()
	            .WithTransientLifetime());
            
            services.Scan(selector => selector
	            .FromAssemblyOf<CustomerFacade>()
	            .AddClasses(x => x.WithAttribute<ScopedServiceAttribute>())
	            .AsMatchingInterface()
	            .WithScopedLifetime());
            
            services.Scan(selector => selector
	            .FromAssemblyOf<CustomerRepository>()
	            .AddClasses(x => x.WithAttribute<ScopedServiceAttribute>())
	            .AsMatchingInterface()
	            .WithScopedLifetime());

            services.AddScoped<ModelStateAccessor>();
            services.AddTransient<BlobUrlValueResolver>();
            services.AddSingleton<ThrottlingDemoService>();
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
	        
	        app.UseStaticFiles();
	        
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
	        
	        app.UseRouting();
	        app.UseCors(CorsPolicies.Default);
            app.UseResponseCaching();
            app.UseAuthentication();
            app.UseMiddleware<ThrottlingMiddleware>();
            app.UseEndpoints(builder =>
            {
	            builder.MapControllers();
            });
        }
    }
}
