using Microsoft.Extensions.Configuration;

namespace Practice_Basics_of_Playwright.Core
{
    public class TestConfiguration
    {
        private readonly IConfiguration _configuration;

        public TestConfiguration()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }
        public AppSettings GetSettings()
        {
            return _configuration.GetSection("AppSettings").Get<AppSettings>() ?? new AppSettings();
        }
      
    }
}
