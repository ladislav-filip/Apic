using System.Net;
using System.Threading.Tasks;
using Apic.Contracts.Customers;
using Apic.Contracts.Infrastructure.Transfer;
using Apic.Facades.Customers;
using Apic.Web.Areas._Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Apic.Web.Areas.Customers
{
	[Area("customers")]
	public class CustomersController :  ApiControllerBase
	{
		private readonly ICustomerFacade customerFacade;
		private readonly ILogger logger;

		public CustomersController(ICustomerFacade customerFacade, ILogger<CustomersController> logger)
		{
			this.customerFacade = customerFacade;
			this.logger = logger;
		}

		[Route("customers")]
		[HttpGet]
		[ProducesResponseType(typeof(DataListResult<Customer>), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> Get([FromQuery]CustomerFilter filter)
		{
			DataListResult<Customer> result = await customerFacade.Get(filter);
			UpdateModelState(result);

			return ErrorResult(result) ??
			       Ok(result);
		}

		[Route("customers/{id:int}")]
		[HttpGet]
		[ProducesResponseType(typeof(DataResult<Customer>), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> Get(int id)
		{
			DataResult<Customer> result = await customerFacade.Get(id);
			UpdateModelState(result);

			return ErrorResult(result) ?? 
			       Ok(result);
		}

		[Route("customers/{id}")]
		[HttpPut]
		[ProducesResponseType(typeof(DataResult<Customer>), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> Put(int id, [FromBody]CustomerUpdate model)
		{
			DataResult<Customer> result = await customerFacade.Update(id, model);
			UpdateModelState(result);

			return ErrorResult(result) ??
			       Ok(result);
		}

		[Route("customers")]
		[HttpPost]
		[ProducesResponseType(typeof(DataResult<Customer>), (int)HttpStatusCode.Created)]
		public async Task<IActionResult> Post([FromBody]CustomerCreate model)
		{
			DataResult<Customer> result = await customerFacade.Create(model);
			UpdateModelState(result);

			return ErrorResult(result) ??
			       CreatedAtAction(nameof(Get), new {id = result.Data.Id}, result);
		}

		[Route("customers/{id}")]
		[HttpDelete]
		[ProducesResponseType(typeof(DataResult<Customer>), (int)HttpStatusCode.NoContent)]
		public async Task<IActionResult> Delete(int id)
		{
			return await ReturnResult(() => customerFacade.Delete(id), NoContent());
		}
	}
}