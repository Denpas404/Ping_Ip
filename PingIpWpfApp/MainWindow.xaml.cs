using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
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


namespace PingIpWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Ping myPing = new Ping();

        string inputIP = "";

        SolidColorBrush _brush;
        Color colorGreen = Color.FromRgb(50, 205, 50);
        Color colorRed = Color.FromRgb(255, 0, 0);

        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            inputIP = ipInput_textbox.Text;

            // IP API URL
            var Ip_Api_Url = "http://ip-api.com/json/"; // 206.189.139.232 - This is a sample IP address. You can pass yours if you want to test          

            // Use HttpClient to get the details from the Json response
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                // Pass API address to get the Geolocation details 
                httpClient.BaseAddress = new Uri(Ip_Api_Url);
                HttpResponseMessage httpResponse = httpClient.GetAsync(Ip_Api_Url).GetAwaiter().GetResult();
                // If API is success and receive the response, then get the location details
                if (httpResponse.IsSuccessStatusCode)
                {
                    var geolocationInfo = httpResponse.Content.ReadAsAsync<LocationDetails_IpApi>().GetAwaiter().GetResult();
                    if (geolocationInfo != null)
                    {                        
                        country_label.Content = geolocationInfo.country;
                    }
                }
            }



            PingReply reply = myPing.Send(inputIP, 1000);

            if (reply != null)
            {
                //_brush = new SolidColorBrush(colorGreen);
                status_label.Content = reply.Status; 
                time_label.Content = reply.RoundtripTime.ToString() + " ms";
                address_label.Content = reply.Address;              
                
                //status_label.Foreground = _brush;
                
            }
            else
            {
                //_brush = new SolidColorBrush(colorRed);
                status_label.Content = "Timeout"; //status_label.Foreground = _brush;
                time_label.Content = "Timeout";
                address_label.Content = "Timeout";
                country_label.Content = "Timeout";
                
            }

        }
    }
    public class LocationDetails_IpApi
    {
        public string query { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string countryCode { get; set; }
        public string isp { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public string org { get; set; }
        public string region { get; set; }
        public string regionName { get; set; }
        public string status { get; set; }
        public string timezone { get; set; }
        public string zip { get; set; }
    }
}
