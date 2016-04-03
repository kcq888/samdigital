using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CMAUniversal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AssistancePage : Page
    {
        public AssistancePage()
        {
            this.InitializeComponent();

            cmaMap.Loaded += CmaMap_Loaded;

            if (CMAProxy.Instance.Response != null)
            {
                DataContext = new
                {
                    CustomMessage = CMAProxy.Instance.CustomMessage,
                    CustomBanner = CMAProxy.Instance.CustomBanner,
                    Results = CMAProxy.Instance.ResultsList
                };
            }
        }

        private void CmaMap_Loaded(object sender, RoutedEventArgs e)
        {
            cmaMap.Center =
               new Geopoint(new BasicGeoposition()
               {
                   //Geopoint for Seattle 
                   Latitude = 37.7842472,
                   Longitude = -122.3984093
               });

            cmaMap.ZoomLevel = 15;

            AddPushpins();
        }

        private void AddPushpins()
        {
            foreach(var result in CMAProxy.Instance.ResultsList)
            {
                MapIcon mapIcon1 = new MapIcon();
                mapIcon1.Location = new Geopoint(
                    new BasicGeoposition {
                        Latitude = Double.Parse(result.Position.Latitude),
                        Longitude = Double.Parse(result.Position.Longitude)
                    });
                mapIcon1.NormalizedAnchorPoint = new Point(0.5, 1.0);
                mapIcon1.ZIndex = 0;
                cmaMap.MapElements.Add(mapIcon1);
            }
        }
    }
}
