using System;
using Apic.Common.Configuration;
using Apic.Data.Context;
using Apic.Web.ActionFilters;
using Apic.Web.Cors;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
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
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new Info { Title = "API Documentation", Version = "v1" });
			});

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
			string connectionString = configuration.GetConnectionString("Default");
			services.AddDbContext<ApicDbContext>(options => options.UseSqlServer(connectionString));

			return services;
		}

		public static IServiceCollection AddCustomizedOptions(this IServiceCollection services,
			IConfiguration configuration)
		{
			services.Configure<AppSettings>(configuration);

			return services;
		}

		public static IServiceCollection AddCustomizedMvc(this IServiceCollection services)
		{
			IMvcBuilder mvc = services.AddMvc(options =>
			{
				options.ReturnHttpNotAcceptable = true;
				options.RespectBrowserAcceptHeader = true;
				
				options.FormatterMappings.SetMediaTypeMappingForFormat("xml", MediaTypeHeaderValue.Parse("application/xml"));
				options.FormatterMappings.SetMediaTypeMappingForFormat("config", MediaTypeHeaderValue.Parse("application/xml"));
				options.FormatterMappings.SetMediaTypeMappingForFormat("js", MediaTypeHeaderValue.Parse("application/json"));
				options.FormatterMappings.SetMediaTypeMappingForFormat("json", MediaTypeHeaderValue.Parse("application/json"));

				options.Filters.Add(new ModelStateValidationActionFilter());
			});

			services.Configure<FormOptions>(options =>
			{
				options.MultipartBodyLengthLimit = 1_048_576; // 1 MB
			});

			mvc.AddXmlSerializerFormatters();
			
			mvc.SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			mvc.AddJsonOptions(jsonOptions =>
			{
				if (jsonOptions.SerializerSettings.ContractResolver != null)
				{
					jsonOptions.SerializerSettings.ContractResolver = new DefaultContractResolver()
					{
						NamingStrategy = new CamelCaseNamingStrategy(),
					};
				}
			});

			return services;
		}

		public static IServiceCollection AddCustomizedApiBehaviorOptions(this IServiceCollection services)
		{
			services.Configure<ApiBehaviorOptions>(options =>
			{
				options.SuppressModelStateInvalidFilter = false;
				options.SuppressConsumesConstraintForFormFileParameters = false;
				options.SuppressInferBindingSourcesForParameters = false;
			});

			return services;
		}
	}
}