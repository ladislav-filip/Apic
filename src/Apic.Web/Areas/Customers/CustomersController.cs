using System.Net;
using System.Threading.Tasks;
using Apic.Contracts.Customers;
using Apic.Contracts.Infrastructure.Transfer;
using Apic.Facades.Customers;
using Apic.Services;
using Apic.Web.Areas._Base;
using Microsoft.AspNetCore.Mvc;
using Apic.Web.Filters;

namespace Apic.Web.Areas.Customers
{
    [Area("customers")]
	public class CustomersController :  ApiControllerBase
	{
		private readonly ICustomerFacade customerFacade;

		public CustomersController(ICustomerFacade customerFacade, ModelStateAccessor modelStateAccessor) : base(modelStateAccessor)
		{
			this.customerFacade = customerFacade;
		}

		[Route("customers")]
		[HttpGet]
		[ProducesResponseType(typeof(Result<Collection<Customer>>), (int)HttpStatusCode.OK)]
        [IgnoreModelStateOnBinding]
		public async Task<IActionResult> Get([FromQuery]CustomerFilter filter)
		{
			Collection<Customer> result = await customerFacade.Get(filter);

			return Ok(result);
		}

		[Route("customers/{id:int}")]
		[HttpGet]
		[ProducesResponseType(typeof(Result<Customer>), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> Get(int id)
		{
            Customer result = await customerFacade.Get(id);

			return Ok(result);
		}

		[Route("customers/{id:int}")]
		[HttpPut]
		[ProducesResponseType(typeof(Result<Customer>), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> Put(int id, [FromBody]CustomerUpdate model)
		{
            Customer result = await customerFacade.Update(id, model);

			return Ok(result);
		}

		[Route("customers")]
		[HttpPost]
		[ProducesResponseType(typeof(Result<Customer>), (int)HttpStatusCode.Created)]
		public async Task<IActionResult> Post([FromBody]CustomerCreate model)
		{
            Customer result = await customerFacade.Create(model);

			return CreatedAtAction(nameof(Get), new {id = result.Id}, result);
		}

	    [Route("customers/{id:int}")]
	    [HttpDelete]
	    [ProducesResponseType(typeof(Result<Customer>), (int)HttpStatusCode.NoContent)]
	    public async Task<IActionResult> Delete(int id)
	    {
	        await customerFacade.Delete(id);

	        return NoContent();
	    }
    }
}
