using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Mjc.Templates.WebApi.IntegrationTests
{
    public class HealthCheckTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly ITestOutputHelper _output;
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public HealthCheckTests(CustomWebApplicationFactory<Startup> factory,
            ITestOutputHelper output)
        {
            _factory = factory;
            _output = output;
        }

        [Theory]
        [InlineData("/hc")]
        public async Task TestHealthCheckEndpoint(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("application/json",
                response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            _output.WriteLine("Result {0}", json);

            Assert.True(!json.Contains("Degraded") && !json.Contains("Unhealthy"));
            Assert.True(json.Length > 0);
        }

        [Theory]
        [InlineData("/self")]
        public async Task TestHealthCheckLivenessEndpoint(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299

            Assert.Equal("text/plain",
                response.Content.Headers.ContentType.ToString());

            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal("Healthy", result);
        }

        [Theory]
        [InlineData("/ready")]
        public async Task TestHealthCheckReadinessEndpoint(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299

            Assert.Equal("text/plain",
                response.Content.Headers.ContentType.ToString());

            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal("Healthy", result);
        }
    }
}
