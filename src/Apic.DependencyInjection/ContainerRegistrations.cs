using System;
using Apic.Common.Attributes;
using Apic.Facades.Customers;
using Apic.Facades.Customers.Queries;
using Apic.Facades.Mappers;
using Apic.Services;
using Apic.Services.AzureStorage;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Apic.DependencyInjection
{
	public static class ContainerRegistrations
	{
		public static WindsorContainer RegisterServices(this WindsorContainer container, IHostingEnvironment env, IConfiguration configuration)
		{
			// [ScopedService]Attribute
			container.Register(Classes
				.FromAssemblyContaining(typeof(CustomerFacade))
				.Where(x => Attribute.IsDefined(x, typeof(ScopedServiceAttribute)))
				.WithService.DefaultInterfaces()
				.LifestyleCustom<MsScopedLifestyleManager>());

			// another services
			container.Register(
				Component.For<AzureStorageService>()
				.ImplementedBy<AzureStorageService>()
				.LifestyleCustom<MsScopedLifestyleManager>());

            container.Register(
                Component.For<ModelStateAccessor>()
                .ImplementedBy<ModelStateAccessor>()
                .LifestyleCustom<MsScopedLifestyleManager>());

			return container;
		}
	}
}
