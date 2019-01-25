using System.Collections.Generic;
using System.Linq;

namespace Apic.Contracts.Infrastructure.Transfer.Filters
{
    public class BaseFilter
    {
        public const string DefaultSortOrder = "asc";

        public virtual string OrderBy { get; set; }
        public string SearchQuery { get; set; }

        public Dictionary<string, string> OrderByRules()
        {
            Dictionary<string, string> rules = new Dictionary<string, string>();
            string[] orderRules = OrderBy.Split(',');

            foreach (var orderRule in orderRules)
            {
                string[] splitted = orderRule.Trim().Split(' ');
                if (splitted.Length > 1)
                {
                    string orientation = new[] { "asc", "desc" }.Contains(splitted[1].ToLowerInvariant()) ? splitted[1] : DefaultSortOrder;
                    rules.Add(splitted[0], orientation);
                }
                else
                {
                    rules.Add(splitted[0], DefaultSortOrder);
                }
            }

            return rules;
        }
    }
}
