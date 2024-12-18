using Microsoft.Playwright;

namespace Practice_Basics_of_Playwright.Pages
{
    public class HomePage
    {
        private readonly IPage page;
        private ProductPage productPage;
        public readonly ILocator logo;
        public readonly ILocator searchField;
        public readonly ILocator cartIconInHeader;
        public readonly ILocator bannerImage;
        private readonly ILocator hotSellerImage;
        public readonly ILocator addToCartButton;
        private readonly ILocator imageLists;
        private readonly ILocator sizeOption;
        private readonly ILocator colorOption;
        private readonly ILocator successMessage;
        public readonly ILocator addToCompareIcon;
        public HomePage(IPage page)
        {
            this.page = page;
            productPage = new ProductPage(page);
            logo = page.GetByLabel("store logo");
            searchField = page.Locator("#search");
            cartIconInHeader = page.Locator(".showcart");
            bannerImage = page.Locator(".home-main>img");
            hotSellerImage = page.GetByAltText("Radiant Tee");
            imageLists = page.Locator(".product-items>li");
            addToCartButton = imageLists.Filter(new() { HasText = "Radiant Tee"}).GetByRole(AriaRole.Button, new() { Name = "Add to Cart" });
            sizeOption = imageLists.Filter(new() { HasText = "Radiant Tee" }).Locator("#option-label-size-143-item-167");
            colorOption = imageLists.Filter(new() { HasText = "Radiant Tee" }).Locator("#option-label-color-93-item-50");
            successMessage = page.GetByRole(AriaRole.Alert).First;
            addToCompareIcon = imageLists.Filter(new() { HasText = "Radiant Tee" }).GetByRole(AriaRole.Link, new() { Name = "Add to Compare" });
        }
        public async Task ClickOnLogoAsync()
        {
            await logo.ClickAsync();
        }
        public async Task ClickOnBannerImageAsync()
        {
            await bannerImage.ClickAsync();
        }
        public async Task ClickOnFirstImageAsync()
        {
            await hotSellerImage.ClickAsync();
        }
        public async Task HoverOnImageAsync()
        {
            await page.HoverAsync("//img[@alt='Radiant Tee']");
        }
        public async Task ClickOnAddToCartButtonAsync()
        {
            await addToCartButton.ClickAsync();
        }
        public async Task SelectSizeAsync()
        {
            await sizeOption.ClickAsync();
        }
        public async Task SelectColorAsync()
        {
            await colorOption.ClickAsync();
        }
        public async Task ClickOnLinkInPromptAsync()
        {
            var cartLink = successMessage.Locator("a");
            await cartLink.ClickAsync();
        }
        public async Task ClickOnAddToCompareIconAsync()
        {
            await addToCompareIcon.ClickAsync();
        }
        public async Task<bool> IsSearchFieldInputElementAsync()
        {
            var searchFieldType = await searchField.GetAttributeAsync("type");
            return searchFieldType == "text";
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
        public async Task<bool> IsSuccessMessageShownAsync()
        {
            await successMessage.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible
            });
            var isMessageShown = await successMessage.IsVisibleAsync();
            var hasLink = await successMessage.Locator("a").IsVisibleAsync();
             
            return isMessageShown && hasLink;
        }
    }
}
