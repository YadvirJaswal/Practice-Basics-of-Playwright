using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace Practice_Basics_of_Playwright.Core
{
    public class BaseTest : IAsyncLifetime
    {
        protected IPlaywright playwright;
        protected IBrowser browser;
        protected IPage page;
        
        protected AppSettings AppSettings;

        public BaseTest()
        {
            AppSettings = new TestConfiguration().GetSettings();
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
            page.GotoAsync(AppSettings.BaseUrl);
        }
        public async Task DisposeAsync()
        {
            page.CloseAsync();
            browser.CloseAsync();
            playwright.Dispose();
        }
    }
}
