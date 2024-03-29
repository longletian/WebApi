﻿using System.Threading.Tasks;
using Newtonsoft.Json;
using WebApi.Api;
using WebApi.Models;
using WebApi.Test.Data;
using Xunit;

namespace WebApi.Test.Controllers
{
    /// <summary>
    /// 集成测试
    /// </summary>
    public class MenuControllerShould : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public MenuControllerShould(CustomWebApplicationFactory<Startup> factory) 
        {
            _factory = factory;
        }

        [Fact]
        public async Task MenuListTest()
        {
            var client = _factory.CreateClient();

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bebaer","");
            // Act
            var response = await client.GetAsync("api/Menu");
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            ResponseData responseData = JsonConvert.DeserializeObject<ResponseData>(responseContent);
            
            // Assert
            Assert.Equal( 200, responseData.MsgCode);
        }
    }
}