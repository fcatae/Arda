using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Http;

namespace Arda.Main
{
    public enum ArdaUsage
    {
        Dashboard_Index,
        Workload_Add,
        ArdaMain_Start
    }

    public static class UsageTelemetry
    {
        public static void Track(string user, ArdaUsage ardaUsage)
        {
            var client = new TelemetryClient();
            
            client.Context.User.AuthenticatedUserId = user;
            
            string eventName = ardaUsage.ToString();

            client.TrackEvent(eventName);
        }        
    }
}
