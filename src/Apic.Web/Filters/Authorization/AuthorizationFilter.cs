using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Apic.Web.Filters.Authorization
{
    /// <summary>
    /// Filter, který pouze upravuje autorizaci. Buď propustí request nebo ho zastaví.
    /// </summary>
    public class AuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization (AuthorizationFilterContext context)
        {
            if(context.HttpContext.User != null)
            {
                // něco
                // context.Result
            }
        }
    }
}
