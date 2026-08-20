using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;

namespace CRN.Product.API.Tests
{
    public class ProductsApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ProductsApiTests(
            WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task SwaggerEndpoint_ReturnsSuccess()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/swagger/index.html");

            // Assert
            Assert.Equal(
                HttpStatusCode.OK,
                response.StatusCode);
        }
    }
}