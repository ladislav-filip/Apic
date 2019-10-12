using System.Threading.Tasks;
using Apic.Contracts.Customers;
using Apic.Contracts.Infrastructure.Transfer;
using Apic.Facades.Customers;
using Apic.Web.Controllers._Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Apic.Web.Controllers
{
	public class CustomersController :  ApiControllerBase
	{
		private readonly ICustomerFacade customerFacade;

		public CustomersController(ICustomerFacade customerFacade)
		{
			this.customerFacade = customerFacade;
		}

		[Route("customers")]
		[HttpGet, HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<Collection<Customer>>> GetCustomers([FromQuery]CustomerFilter filter)
		{
			Collection<Customer> result = await customerFacade.Get(filter);

			return Ok(result);
		}

		[Route("customers/{id:int}")]
		[HttpGet, HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<Customer>> GetCustomer(int id)
		{
            Customer result = await customerFacade.Get(id);

			return Ok(result);
		}

		[Route("customers/{id:int}")]
		[HttpPut]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<Customer>> UpdateCustomer(int id, [FromBody]CustomerUpdate model)
		{
            Customer result = await customerFacade.Update(id, model);

			return Ok(result);
		}

		[Route("customers")]
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Customer>> CreateCustomer([FromBody]CustomerCreate model)
		{
            Customer result = await customerFacade.Create(model);

			return CreatedAtAction(nameof(GetCustomer), new {id = result.Id}, result);
		}

	    [Route("customers/{id:int}")]
	    [HttpDelete]
	    [ProducesResponseType(StatusCodes.Status204NoContent)]
	    public async Task<ActionResult<Customer>> DeleteCustomer(int id)
	    {
	        await customerFacade.Delete(id);

	        return NoContent();
	    }
    }
}
