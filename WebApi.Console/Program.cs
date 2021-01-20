using Grpc.Core;
using Grpc.Net.Client;
using GrpcService;
using IdentityModel.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebApi
{
    class Program
    {
        static async Task Main(string[] args)
        {
             TestGrpc();
            //await TestToken();
            Console.ReadKey();
        }

        public static async Task TestToken()
        {
            var client = new HttpClient();

            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            // request token
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "client",
                ClientSecret = "secret",
                Scope = "api1"
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");

            // call api
            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await apiClient.GetAsync("https://localhost:6001/api/User/identity");
            Console.WriteLine(JsonConvert.SerializeObject(response));
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }
        }

        public static void TestGrpc()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:6001");
            var client = new Greeter.GreeterClient(channel);

            var response = client.SayHello(
                 new HelloRequest { Name = "World" });
            Console.WriteLine(response.Message);
        }


        public static async void TestFromServer()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:6001");
            var client = new Greeter.GreeterClient(channel);
            using var response = client.StreamingFromServer(new HelloRequest { Name = "World" });
            await foreach (var responses in response.ResponseStream.ReadAllAsync())
            {
                Console.WriteLine("Greeting: " + responses.Message);
                // "Greeting: Hello World" is written multiple times
            }
        }
    }
}
