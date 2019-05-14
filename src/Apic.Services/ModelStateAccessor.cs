using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Apic.Services
{
    public class ModelStateAccessor
    {
        public ModelStateDictionary ModelState { get; set; }
        public List<string> InfoMessages { get; } = new List<string>();

        public void AddInfoMessage(string message)
        {
            InfoMessages.Add(message);
        }
        
        public void AddModelError(string key, string errorMessage)
        {
            ModelState.AddModelError(key, errorMessage);
        }
        
        public void AddModelError(string errorMessage)
        {
            ModelState.AddModelError(string.Empty, errorMessage);
        }
    }
}
