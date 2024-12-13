using Microsoft.Playwright;

namespace Practice_Basics_of_Playwright.Pages
{
    public class HomePage
    {
        private readonly IPage page;
        public readonly ILocator logo;
        public readonly ILocator searchField;
        public readonly ILocator cartIcon;
        public HomePage(IPage page)
        {
            this.page = page;
            logo = page.GetByLabel("store logo");
            searchField = page.Locator("#search");
            cartIcon = page.Locator(".showcart");
        }
        public async Task ClickOnLogoAsync()
        {
            await logo.ClickAsync();
        }
        public async Task<bool> IsSearchFieldInputElementAsync()
        {
            var searchFieldType = await searchField.GetAttributeAsync("type");
            return searchFieldType == "text";
        }
       
        
    }
}
