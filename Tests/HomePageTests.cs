using Microsoft.Playwright;
using Newtonsoft.Json;
using Practice_Basics_of_Playwright.Core;
using Practice_Basics_of_Playwright.Models;
using Practice_Basics_of_Playwright.Pages;

namespace Practice_Basics_of_Playwright.Tests
{
    public class HomePageTests : BaseTest
    {
        private readonly SignInTestData testData;
        private readonly Dictionary<string, List<TestCase>> testCaseData;
        private const string homePageSheetName = "HomePage_Tests ";
        private readonly List<TestCase> homePageTestCasesList;
        public HomePageTests()
        {
            // Read Data from excel
            testCaseData = excelReader.ReadExcelFile("Test Data/ECT-TestCases.xlsx", [homePageSheetName]);
            homePageTestCasesList = testCaseData[homePageSheetName];
            if (homePageTestCasesList.Count == 0)
            {
                throw new Exception("No test cases found");
            }
        }
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
            await homePage.ClickOnLinkInPromptAsync();

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
        public async Task HotSellerImages_Hover_ShouldDisplayAddToCompareIcon()
        {
            // Arrange
            var homePage = new HomePage(page);

            // Act
            await homePage.HoverOnImageAsync();

            // Assert
            await Assertions.Expect(homePage.addToCompareIcon).ToBeVisibleAsync();
        }

        [Fact]
        public async Task HotSellerImages_Hover_ClickAddToCompareIcon_ShouldNavigateAndPrompt()
        {
            // Arrange
            var homePage = new HomePage(page);

            // Act
            await homePage.HoverOnImageAsync();
            await homePage.ClickOnAddToCompareIconAsync();

            // Assert the visibility of success message on the page and the visibility of comparison list link in message
            var isMessageShown = await homePage.IsSuccessMessageShownAsync();
            Assert.True(isMessageShown);

            // Click on the shopping cart link shown in the success message
            await homePage.ClickOnLinkInPromptAsync();

            // Assert the navigation after clicking on link
            await Assertions.Expect(page).ToHaveURLAsync($"{appSettings.BaseUrl}catalog/product_compare/");
        }

        [Fact]
        public async Task HotSellerImages_Hover_ShouldDisplayAddToWishListIcon()
        {
            // Arrange
            var homePage = new HomePage(page);

            // Act
            await homePage.HoverOnImageAsync();

            //Assert
            await Assertions.Expect(homePage.addToWishListIcon).ToBeVisibleAsync();
        }

        [Fact]
        public async Task HotSellerImages_Hover_WithoutLoggingIn_ClickAddToWishList_ShouldNavigateAndPrompt()
        {
            // Arrange
            var homePage = new HomePage(page);

            // Act
            await homePage.HoverOnImageAsync();
            await homePage.ClickOnAddToWishListIcon();

            // Assert
            await Assertions.Expect(page).ToHaveTitleAsync("Customer Login");
            await Assertions.Expect(page.GetByRole(AriaRole.Alert).First).ToBeVisibleAsync();
        }

        [Theory]
        [InlineData("HP-015")]
        public async Task HotSellerImages_Hover_LogIn_ClickAddToWishList_ShouldNavigateAndPrompt(string testCaseId)
        {
            // Arrange
            var testCase = homePageTestCasesList.Find(l => l.TestCaseId == testCaseId);
            Assert.NotNull(testCase);
            Assert.NotEmpty(testCase.TestData);
            var testData = JsonConvert.DeserializeObject<SignInUser>(testCase.TestData);
            Assert.NotNull(testData);

            var homePage = new HomePage(page);
            var signInPage = new SignInPage(page,appSettings);

            // Act
            // 1. Log in to the application with valid credentials.
            await signInPage.SignInUserAsync(testData);

            // 2. Navigate to the home page.
            await homePage.ClickOnLogoAsync();

            // 3. Hover over a product image in the Hot Sellers section.
            await homePage.HoverOnImageAsync();

            // 4. Verify the "Add to Wishlist" icon is visible.
            await Assertions.Expect(homePage.addToWishListIcon).ToBeVisibleAsync();

            // 5. Click on the "Add to Wishlist" icon.
            await homePage.ClickOnAddToWishListIcon();

            // Assert
            // 6. Verify that the user is navigated to the wishlist page.
            await Assertions.Expect(page).ToHaveTitleAsync("My Wish List");

            // 7. Verify that a success message is displayed confirming the item was added to the wishlist.
            await Assertions.Expect(homePage.successMessage).ToBeVisibleAsync();
        }

        [Fact]
        public async Task HotSellerImages_ReviewsAndRatings_ShouldVisibleAndNavigate()
        {
            // Arrange
            var homePage = new HomePage(page);

            // star ratings and reviews are visible below the product image.
            await Assertions.Expect(homePage.ratingSummary).ToBeVisibleAsync();
            await Assertions.Expect(homePage.reviews).ToBeVisibleAsync();

            // Act (Click on the reviews link)
            await homePage.reviews.ClickAsync();

            // Assert Navigation
            await Assertions.Expect(page).ToHaveURLAsync($"{appSettings.BaseUrl}radiant-tee.html#reviews");
        }
    }
}
