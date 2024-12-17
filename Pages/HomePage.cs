using Microsoft.Playwright;

namespace Practice_Basics_of_Playwright.Pages
{
    public class HomePage
    {
        private readonly IPage page;
        private ProductPage productPage;
        public readonly ILocator logo;
        public readonly ILocator searchField;
        public readonly ILocator cartIcon;
        public readonly ILocator bannerImage;
        private readonly ILocator hotSellerImage;
        
        public HomePage(IPage page)
        {
            this.page = page;
            productPage = new ProductPage(page);
            logo = page.GetByLabel("store logo");
            searchField = page.Locator("#search");
            cartIcon = page.Locator(".showcart");
            bannerImage = page.Locator(".home-main>img");
            hotSellerImage = page.GetByAltText("Radiant Tee");
            
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
       public async Task ClickOnBannerImageAsync()
       {
            await bannerImage.ClickAsync();
       } 
        public async Task ClickOnFirstImageAsync()
        {
            await hotSellerImage.ClickAsync();
        }
        public async Task<bool> HasNavigatedToCorrectPage()
        {
            var hotSellerImages = await page.Locator(".product-items>li").AllAsync();
            var hasNavigateToCorrectPage = false;
            foreach (var hotSellerImage in hotSellerImages)
            {
                var title = hotSellerImage.Locator(".product-item-name");
                var titleText = await title.TextContentAsync() ?? "";
                var actualTitleText = titleText.Trim();
                var expectedUrl = await title.GetByRole(AriaRole.Link).GetAttributeAsync("href");
                await hotSellerImage.ClickAsync();
                var titleAfterClicking = await productPage.GetTitleAfterClicking();
                var actualUrl = page.Url; 
                if (actualTitleText == titleAfterClicking && expectedUrl == actualUrl)
                {
                    hasNavigateToCorrectPage = true;
                }
                await page.GoBackAsync();
                
            }
            return hasNavigateToCorrectPage;   
        }
    }
}
