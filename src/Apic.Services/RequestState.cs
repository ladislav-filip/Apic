using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace Apic.Services
{
    public class ModelStateAccessor
    {
        public ModelStateDictionary ModelState { get; set; }
        public List<string> Messages { get; set; } = new List<string>();
    }
}
