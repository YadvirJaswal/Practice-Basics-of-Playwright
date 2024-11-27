using Microsoft.Playwright;

namespace Practice_Basics_of_Playwright
{
    public class UnitTest1: IAsyncLifetime
    {
        private IPlaywright playwright;
        private IBrowser browser;
        private IPage page;

       

        public async Task InitializeAsync()
        {
            // Initialize playwright and launch browser
            playwright = await Playwright.CreateAsync();
            browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
            });
            // Create a new page
            var context = await browser.NewContextAsync();
            page = await context.NewPageAsync();
           
        }
        public async Task DisposeAsync()
        {
            // Dispose resources after tests
            page.CloseAsync();
            browser.CloseAsync();
            playwright.Dispose();
        }
        
        [Fact]
        public async Task Test1()
        {
            await page.GotoAsync("http://eaapp.somee.com/");
            await page.ClickAsync(selector:"text = Login");
            
            await page.FillAsync("#UserName", "admin");
            await page.FillAsync("#Password", "password");
            await page.ClickAsync("text = Log in");
            var isExist = await page.Locator("text = Employee Details").IsVisibleAsync();
            Assert.True(isExist);
        }
    }
}