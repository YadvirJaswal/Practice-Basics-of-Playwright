using Microsoft.Playwright;

namespace Practice_Basics_of_Playwright.Pages
{
    public class HomePage
    {
        private readonly IPage page;
        public readonly ILocator logo;
        public HomePage(IPage page)
        {
            this.page = page;
            logo = page.GetByLabel("store logo");

        }
        public async Task ClickOnLogo()
        {
            await logo.ClickAsync();
        }
        
    }
}
