using Microsoft.Playwright;
using Practice_Basics_of_Playwright.Core;
using Practice_Basics_of_Playwright.Pages;

namespace Practice_Basics_of_Playwright.Tests
{
    public class HomePageTests : BaseTest
    {
        [Fact]
        public async Task LogoShouldBeVisisbleAndClickable()
        {
            // Arrange
            var homePage = new HomePage(page);

            // Assert
            await Assertions.Expect(homePage.logo).ToBeVisibleAsync();
            await Assertions.Expect(homePage.logo).ToBeEnabledAsync();

            // Act
            await homePage.ClickOnLogoAsync();

            // Assert
            await Assertions.Expect(page).ToHaveURLAsync(appSettings.BaseUrl);
            await Assertions.Expect(page).ToHaveTitleAsync("Home Page");
        }

        [Fact]
        public async Task SearchFieldShouldBeVisible()
        {
            // Arrange 
            var homePage = new HomePage(page);

            // Assert that search field is visible on page
            await Assertions.Expect(homePage.searchField).ToBeVisibleAsync();

            // Assert that search field is an input element
            var isSearchFieldInputElement = await homePage.IsSearchFieldInputElementAsync();
            Assert.True(isSearchFieldInputElement);
        }
        [Fact]
        public async Task CartButtonShouldBeVisibleAndClickable()
        {
            // arrange
            var homePage = new HomePage(page);

            // Assert
            await Assertions.Expect(homePage.cartIconInHeader).ToBeVisibleAsync();
            await Assertions.Expect(homePage.cartIconInHeader).ToBeEnabledAsync();
        }
        [Fact]
        public async Task BannerImage_ClickOnBanner_ShouldBeNavigatedToYogaCollectionPage()
        {
            // Arrange
            var homePage = new HomePage(page);

            // Assert that the banner is visible and clickable
            await Assertions.Expect(homePage.bannerImage).ToBeVisibleAsync();
            await Assertions.Expect(homePage.bannerImage).ToBeEnabledAsync();

            // Act - Click on banner image
            await homePage.ClickOnBannerImageAsync();

            // Assert the navigation of banner image
            await Assertions.Expect(page).ToHaveURLAsync($"{appSettings.BaseUrl}collections/yoga-new.html");
        }
        [Fact]
        public async Task HotSellerImages_ClickOnImage_ShouldBeNavigatedToProductPage()
        {
            // Arrange
            var homePage = new HomePage(page);
            
            // Act and Assert
            var hasTitleMatched = await homePage.HasNavigatedToCorrectPage();
            Assert.True(hasTitleMatched);
        }

        [Fact]
        public async Task HotSellerImage_Hover_ShouldDisplayAddToCartButton()
        {
            // Arrange
            var homePage = new HomePage(page);

            // Act
            await homePage.HoverOnImageAsync();

            // Assert
            await Assertions.Expect(homePage.addToCartButton).ToBeVisibleAsync();
        }

        [Fact]
        public async Task HotSellerImages_Hover_ClickAddToCart_ShouldNavigateAndPrompt()
        {
            // Arrange
            var homePage = new HomePage(page);

            // Act
            await homePage.HoverOnImageAsync();
            await homePage.ClickOnAddToCartButtonAsync();

            // Assert
            await Assertions.Expect(page).ToHaveURLAsync($"{appSettings.BaseUrl}radiant-tee.html");
            await Assertions.Expect(page.GetByRole(AriaRole.Alert).First).ToBeVisibleAsync();
        }

        [Fact]
        public async Task HotSellerImages_Hover_SelectOptions_ClickAddToCart_ShouldNavigateAndPrompt()
        {
            // Arrange
            var homePage = new HomePage(page);

            // Act 
            await homePage.HoverOnImageAsync();
            await homePage.SelectSizeAsync();
            await homePage.SelectColorAsync();
            await homePage.ClickOnAddToCartButtonAsync();

            // Assert the visibility of success message on the page and the visibility of shopping cart link in message
            var isMessageShown = await homePage.IsSuccessMessageShownAsync();
            Assert.True(isMessageShown);

            // Click on the shopping cart link shown in the success message
            await homePage.ClickOnShoppingCartLinkAsync();

            // Assert the navigation after clicking on link
            await Assertions.Expect(page).ToHaveURLAsync($"{appSettings.BaseUrl}checkout/cart/");
        }

        [Fact]
        public async Task HotSellerImages_Hover_SelectSize_ClickAddToCart_ShouldNavigateAndPrompt()
        {
            // Arrange
            var homePage = new HomePage(page);
            var productPage = new ProductPage(page);

            // Act
            await homePage.HoverOnImageAsync();
            await homePage.SelectSizeAsync();
            await homePage.ClickOnAddToCartButtonAsync();

            // Assert
            await Assertions.Expect(page).ToHaveURLAsync($"{appSettings.BaseUrl}radiant-tee.html");
            await Assertions.Expect(productPage.errorMessage).ToBeVisibleAsync();
        }

        [Fact]
        public async Task HotSellerImages_Hover_SelectColor_ClickAddToCart_ShouldNavigateAndPrompt()
        {
            // Arrange
            var homePage = new HomePage(page);
            var productPage = new ProductPage(page);

            // Act
            await homePage.HoverOnImageAsync();
            await homePage.SelectColorAsync();
            await homePage.ClickOnAddToCartButtonAsync();

            // Assert
            await Assertions.Expect(page).ToHaveURLAsync($"{appSettings.BaseUrl}radiant-tee.html");
            await Assertions.Expect(productPage.errorMessage).ToBeVisibleAsync();
        }

        [Fact]
        public async Task HoSellerImages_Hover_ShouldDisplayAddToCompareIcon()
        {
            // Arrange
            var homePage = new HomePage(page);

            // Act
            await homePage.HoverOnImageAsync();

            // Assert
            await Assertions.Expect(homePage.addToCompareIcon).ToBeVisibleAsync();
        }
    }
}
