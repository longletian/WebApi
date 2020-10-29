using IdentityModel.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebApi
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5000");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }
            Console.ReadKey();
        }
    }
}
