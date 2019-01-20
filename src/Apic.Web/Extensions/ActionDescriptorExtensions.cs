using Microsoft.AspNetCore.Mvc.Abstractions;
using System;

namespace Apic.Web.Extensions
{
    public static class ActionDescriptorExtensions
    {
        public static bool ContainsFilter(this ActionDescriptor actionDescription, Type type)
        {
            foreach (var filterDescriptors in actionDescription.FilterDescriptors)
            {
                if (filterDescriptors.Filter.GetType() == type)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
