using System.Linq;
using System.Reflection;
using Apic.Common.Attributes;
using Apic.Facades.Customers;
using Apic.Facades.Customers.Queries;
using Apic.Facades.Documents;
using Apic.Facades.Mappers;
using Apic.Services.AzureStorage;
using Autofac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;

namespace Apic.DependencyInjection
{
	public static class AutofacRegistrations
	{
		public static ContainerBuilder RegisterServices(this ContainerBuilder builder, IHostingEnvironment env, IConfiguration configuration)
		{
			// [Service]Attribute
			builder.RegisterAssemblyTypes(typeof(CustomerMapper).Assembly)
				.Where(t => t.GetCustomAttributes().Any(x=> x.Match(new ServiceAttribute())))
				.AsImplementedInterfaces()
				.InstancePerLifetimeScope();

			// another services
			builder.RegisterType<AzureStorageService>().As<AzureStorageService>().InstancePerLifetimeScope();

			// queries
			builder.RegisterType<GetCustomersQuery>().As<GetCustomersQuery>().InstancePerLifetimeScope();

			return builder;
		}
	}
}