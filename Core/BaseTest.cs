using Microsoft.Playwright;
using Practice_Basics_of_Playwright.Utilities;

namespace Practice_Basics_of_Playwright.Core
{
    public class BaseTest : IAsyncLifetime
    {
        protected IPlaywright playwright;
        protected IBrowser browser;
        protected IPage page;
        
        protected AppSettings appSettings;
        protected ExcelReader excelReader;

        public BaseTest()
        {
            appSettings = new TestConfiguration().GetSettings();
            excelReader = new ExcelReader();
        }

        public async Task InitializeAsync()
        {
            // Initialize Palywright and launch browser
            playwright = await Playwright.CreateAsync();
            browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false
            });

            // create a new page
            var context = await browser.NewContextAsync();
            page = await context.NewPageAsync();

            // Navigate to Url
           await page.GotoAsync(appSettings.BaseUrl);
        }
        public async Task DisposeAsync()
        {
            await page.CloseAsync();
            await browser.CloseAsync();
            playwright.Dispose();
        }
    }
}
