using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Apic.Web.Filters
{
    /// <summary>
    /// Action method will be executed when ModelState.IsValid == false
    /// </summary>
    public class IgnoreModelStateOnBinding : Attribute
    {
        // Flag class
    }
}
