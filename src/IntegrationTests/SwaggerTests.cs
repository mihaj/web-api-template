using System.Threading.Tasks;
using Xunit;

namespace Mjc.Templates.WebApi.IntegrationTests
{
    public class SwaggerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public SwaggerTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/swagger/v1/swagger.json")]
        public async Task SwaggerTest(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            Assert.True(json.Length > 0);
        }
    }
}
