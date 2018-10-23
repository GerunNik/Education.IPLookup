using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;

namespace lookupIP
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter an IP address, for your own, enter \"My IP\"");
            Console.WriteLine("If you enter nothing, then your Public IP will be used");
            string IPToTest = Console.ReadLine();

            if (IPToTest == "My IP")
            {
                IPToTest = GetLocalIPAddress();
            }

            System.Threading.Tasks.Task<string> response = MakeIPRequestAsync(IPToTest);

            Console.WriteLine(response.Result);

            Console.ReadKey();
        }
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public static async System.Threading.Tasks.Task<string> MakeIPRequestAsync(string IPAddress)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            string apiKey = "?api-key=25b95eeec10d3b30d284eaa239ee834cef2bf568bc35ba9810339754";
            string baseURL = "https://api.ipdata.co/";

            var baseAddress = new Uri(baseURL + IPAddress + apiKey);

            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {

                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");

                using (var response = await httpClient.GetAsync(""))
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    return responseData;
                }
            }
            

        }
    }
}
