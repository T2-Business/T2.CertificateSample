using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System;
using Microsoft.Extensions.Configuration;

namespace T2.CertificateSample
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
             var result = await GetApiDataUsingHttpClientHandler();
            Console.WriteLine(result);
            Console.ReadKey();
        }
        private static async Task<string> GetApiDataUsingHttpClientHandler()
        {
            var certificatePath = $"{Environment.CurrentDirectory}\\Certificate.pfx";
            var cert = new X509Certificate2(certificatePath, "123456");
            var handler = new HttpClientHandler();
            handler.ClientCertificates.Add(cert);
            var client = new HttpClient(handler);

            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri("http://localhost:5149/api/WeatherForecast"),
                Method = HttpMethod.Get,
            };
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }

            throw new ApplicationException($"Status code: {response.StatusCode}, Error: {response.ReasonPhrase}");
        }

    }
}