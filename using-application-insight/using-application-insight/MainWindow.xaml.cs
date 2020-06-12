using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace using_application_insight
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

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
            telemetryClient.Flush();
        }
    }
}
