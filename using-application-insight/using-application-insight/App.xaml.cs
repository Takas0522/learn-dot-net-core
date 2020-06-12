using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace using_application_insight
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            TelemetryConfiguration configuration = TelemetryConfiguration.CreateDefault();
            configuration.InstrumentationKey = "<InstrumentationKey>";
            var telemetryClient = new TelemetryClient(configuration);
            telemetryClient.TrackTrace("Application Run");
            telemetryClient.TrackEvent("My Event");
            try
            {
                throw new Exception("CUSTOM ERROR");
            }
            catch (Exception e)
            {
                telemetryClient.TrackException(e);
            }
            
        }
    }
}
