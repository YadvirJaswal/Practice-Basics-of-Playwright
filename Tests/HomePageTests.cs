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
            await Assertions.Expect(homePage.cartIcon).ToBeVisibleAsync();
            await Assertions.Expect(homePage.cartIcon).ToBeEnabledAsync();
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



    }
}
