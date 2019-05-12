using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Apic.Common.Options;
using Apic.Data.Context;
using Apic.Facades.Mappers;
using Apic.Services.AzureStorage;
using Apic.Web.Cors;
using Apic.Web.Filters.Action;
using Apic.Web.Filters.Exception;
using AutoMapper;
using BeatPulse;
using BeatPulse.UI;
using Microsoft.AspNetCore.Antiforgery.Internal;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
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
				x.SwaggerDoc("v1", new Info
				{
					Title = "API Documentation",
					Version = "v1",
				});

				// zahrne XML soubory s dokumentací
				x.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, XmlFileName(typeof(Startup))));
                
				// všechno camelcase
				x.DescribeAllParametersInCamelCase();
				x.DescribeStringEnumsInCamelCase();
                
				x.EnableAnnotations();
				x.DescribeAllEnumsAsStrings();
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

			return services;
		}

		public static IServiceCollection AddCustomizedMvc(this IServiceCollection services)
		{
            services.AddScoped<ExceptionFilter>();
            services.AddScoped<ValidationFilter>();

            IMvcBuilder mvc = services.AddMvc(options =>
			{
				options.ReturnHttpNotAcceptable = true;
				options.RespectBrowserAcceptHeader = true;

				options.FormatterMappings.SetMediaTypeMappingForFormat("xml", MediaTypeHeaderValue.Parse("application/xml"));
				options.FormatterMappings.SetMediaTypeMappingForFormat("config", MediaTypeHeaderValue.Parse("application/xml"));
				options.FormatterMappings.SetMediaTypeMappingForFormat("js", MediaTypeHeaderValue.Parse("application/json"));
				options.FormatterMappings.SetMediaTypeMappingForFormat("json", MediaTypeHeaderValue.Parse("application/json"));

				// zapojení input formatters ve správném pořadí
				var jsonInputFormatter = options.InputFormatters.FirstOrDefault(x => x.GetType() == typeof(JsonInputFormatter));
				options.InputFormatters.Clear();
				options.InputFormatters.Add(jsonInputFormatter);
				options.InputFormatters.Add(new XmlSerializerInputFormatter(new MvcOptions()));

				// zapojení output formatters ve správném pořadí
				var jsonOutputFormatter = options.OutputFormatters.FirstOrDefault(x => x.GetType() == typeof(JsonOutputFormatter));
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

			mvc.SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

			mvc.ConfigureApiBehaviorOptions(options =>
			{
				options.SuppressModelStateInvalidFilter = true;
				/*options.SuppressConsumesConstraintForFormFileParameters = true;
				options.SuppressInferBindingSourcesForParameters = true;
				options.SuppressMapClientErrors = true;
				options.SuppressUseValidationProblemDetailsForInvalidModelStateResponses = true;*/
			});

		    mvc.AddJsonOptions(o => o.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());

			return services;
		}
	}
}
