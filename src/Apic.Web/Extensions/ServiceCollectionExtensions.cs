using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Apic.Common.Options;
using Apic.Contracts.Customers;
using Apic.Data.Context;
using Apic.Facades.Mappers;
using Apic.Services.AzureStorage;
using Apic.Web.Cors;
using Apic.Web.Filters.Action;
using Apic.Web.Filters.Exception;
using AutoMapper;
using BeatPulse;
using BeatPulse.UI;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;

namespace Apic.Web.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddCustomizedSwagger(this IServiceCollection services, IConfiguration configuration)
		{
			string XmlFileName(Type type) =>
				type.GetTypeInfo()
					.Module.Name.Replace(".dll", ".xml").Replace(".exe", ".xml");
			
			services.AddSwaggerGen(x =>
			{
				x.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "API Documentation",
					Version = "v1",
				});

				// zahrne XML soubory s dokumentací
				x.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, XmlFileName(typeof(Startup))));
				x.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, XmlFileName(typeof(ApplicationSettings))));
				x.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, XmlFileName(typeof(Customer))));
                
				// všechno camelcase
				x.DescribeAllParametersInCamelCase();
                
				x.EnableAnnotations();
			});

			return services;
		}

        public static IServiceCollection AddCustomizedAutomapper(this IServiceCollection services)
        {
            IAzureStorageService azureStorage = services.BuildServiceProvider().GetService<IAzureStorageService>();

            var mappingConfig = new MapperConfiguration(configuration =>
            {
                configuration.AddProfile(new DocumentMappingProfile(azureStorage));
                configuration.AddProfile<CustomerMappingProfile>();
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            return services;
        }

	    public static IServiceCollection AddCustomizedAzureStorage(this IServiceCollection services)
	    {
	        services.AddScoped<IAzureStorageService, AzureStorageService>();

	        return services;
	    }

        public static IServiceCollection AddCustomizedBeatPulse(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddBeatPulse(setup =>
			{
				setup.AddSqlServer(configuration.GetConnectionString("SqlServer"));
				setup.AddSqlite(configuration.GetConnectionString("SqlLite"));
			});

            services.AddBeatPulseUI();

			return services;
		}

		public static IServiceCollection AddCustomizedCors(this IServiceCollection services)
		{
			services.AddCors(x =>
			{
				x.AddPolicy(CorsPolicies.Default, builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
			});

			return services;
		}

		public static IServiceCollection AddCustomizedDbContext(this IServiceCollection services, IConfiguration configuration)
		{
			string connectionString = configuration.GetConnectionString("SqlLite");
			services.AddDbContext<UnitOfWork>(options => options.UseSqlite(connectionString));

			services.AddScoped<IUnitOfWork, UnitOfWork>();

			return services;
		}

		public static IServiceCollection AddCustomizedOptions(this IServiceCollection services,
			IConfiguration configuration)
		{
			services.Configure<AppSettings>(configuration);
			services.Configure<Throttling>(configuration.GetSection("Throttling"));
			services.Configure<ConsoleLifetimeOptions>(options =>
			{
				options.SuppressStatusMessages = true;
			});

			return services;
		}

		public static IServiceCollection AddCustomizedControllers(this IServiceCollection services)
		{
            services.AddScoped<ExceptionFilter>();
            services.AddScoped<ValidationFilter>();

            IMvcBuilder mvc = services.AddControllers(options =>
			{
				options.ReturnHttpNotAcceptable = true;
				options.RespectBrowserAcceptHeader = true;

				options.FormatterMappings.SetMediaTypeMappingForFormat("xml", MediaTypeHeaderValue.Parse("application/xml"));
				options.FormatterMappings.SetMediaTypeMappingForFormat("config", MediaTypeHeaderValue.Parse("application/xml"));
				options.FormatterMappings.SetMediaTypeMappingForFormat("js", MediaTypeHeaderValue.Parse("application/json"));
				options.FormatterMappings.SetMediaTypeMappingForFormat("json", MediaTypeHeaderValue.Parse("application/json"));

				// zapojení input formatters ve správném pořadí
				var jsonInputFormatter = options.InputFormatters.FirstOrDefault(x => x.GetType() == typeof(SystemTextJsonInputFormatter));
				options.InputFormatters.Clear();
				options.InputFormatters.Add(jsonInputFormatter);
				options.InputFormatters.Add(new XmlSerializerInputFormatter(new MvcOptions()));

				// zapojení output formatters ve správném pořadí
				var jsonOutputFormatter = options.OutputFormatters.FirstOrDefault(x => x.GetType() == typeof(SystemTextJsonOutputFormatter));
				options.OutputFormatters.Clear();
				options.OutputFormatters.Add(jsonOutputFormatter);
				options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
				
                options.Filters.AddService(typeof(ExceptionFilter));
                options.Filters.AddService(typeof(ValidationFilter));
                //options.Filters.AddService(typeof(ApiResultFilter));
            });

			services.Configure<FormOptions>(options =>
			{
				options.MultipartBodyLengthLimit = 1_048_576; // 1 MB
			});

			mvc.SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

			mvc.ConfigureApiBehaviorOptions(options =>
			{
				// výchozí nastavení validačního mechanismu
			});

		    mvc.AddJsonOptions(o =>
		    {
			    o.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
			    o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
		    });

			return services;
		}
	}
}
