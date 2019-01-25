using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Apic.Services.Throttling
{
    // singleton
    public class ThrottlingDemoService
    {
        public Dictionary<IPAddress, List<DateTime>> Log { get; set; } = new Dictionary<IPAddress, List<DateTime>>();

        public bool ThresholdExceeded(IPAddress ipAddress, int limit, TimeSpan timeSpan)
        {
            if (!Log.ContainsKey(ipAddress))
            {
                return false;
            }

            int count = Log[ipAddress].Count(x => x > x.Add(timeSpan.Negate()));
            if (count > limit)
            {
                return true;
            }

            return false;
        }

        public int RemainingLimit(IPAddress ipAddress, int limit, TimeSpan timeSpan)
        {
            if (!Log.ContainsKey(ipAddress))
            {
                return limit;
            }

            int count = Log[ipAddress].Count(x => x > x.Add(timeSpan.Negate()));

            return limit - count;
        }

        public void LogAccess(IPAddress ipAddress)
        {
            if (!Log.ContainsKey(ipAddress))
            {
                Log.Add(ipAddress, new List<DateTime>() {DateTime.Now});
            }

            Log[ipAddress].Add(DateTime.Now);
        }
    }
}
