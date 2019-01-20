using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Apic.Contracts.Customers;
using Apic.Contracts.Infrastructure.Transfer;
using Apic.Data.Context;
using Apic.Facades.Customers;
using Apic.Facades.Mappers;
using Apic.Services;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using CustomerDbo = Apic.Entities.Customers.Customer;

namespace Apic.Tests.Facades.Customers
{
	[TestClass]
	public class CustomerFacadeTests : FacadeUnitTestBase
	{
		[TestMethod]
		public async Task GetOne_OkResult()
		{
			//using (var db = GetInMemoryDbContext(true))
			//{
			//	//  db
			//	db.Customers.Add(new CustomerDbo(){Id = 1, FirstName = "A", LastName = "B"});
			//	db.SaveChanges();

			//	// arrange
			//	Mock<ICustomerMapper> customerMapper = new Mock<ICustomerMapper>(MockBehavior.Strict);
			//	customerMapper.Setup(x => x.Map(It.IsAny<CustomerDbo>())).Returns(new Customer() { Id = 1 });
                
   //             Mock<ModelStateAccessor> accessor = new Mock<ModelStateAccessor>();

			//	// act
			//	var facade = new CustomerFacade(db, customerMapper.Object, accessor.Object);
			//	Result<Customer> result = await facade.Get(1);

			//	// assert
			//	result.Should().NotBeNull("result vždy existuje");
			//	result.Data.Should().NotBeNull("záznam existuje");
			//	result.Code.Should().Be(ResultCodes.Ok, "záznam existuje");
			//}
		}

		[TestMethod]
		public async Task GetOne_NotFoundResult()
		{
			//using (var db = GetInMemoryDbContext(true))
			//{
			//	//  db
			//	db.Customers.Add(new CustomerDbo() { Id = 99, FirstName = "A", LastName = "B" });
			//	db.SaveChanges();

			//	// arrange
			//	Mock<ICustomerMapper> customerMapper = new Mock<ICustomerMapper>(MockBehavior.Strict);
			//	customerMapper.Setup(x => x.Map(It.IsAny<CustomerDbo>())).Returns(new Customer() { Id = 99 });

   //             Mock<ModelStateAccessor> accessor = new Mock<ModelStateAccessor>();

   //             // act
   //             var facade = new CustomerFacade(db, customerMapper.Object, accessor.Object);
			//	Result<Customer> result = await facade.Get(1);

			//	// assert
			//	result.Should().NotBeNull("result vždy existuje");
			//	result.Data.Should().BeNull("záznam neexistuje");
			//	result.Code.Should().Be(ResultCodes.NotFound, "záznam neexistuje");
			//}
		}
	}
}
