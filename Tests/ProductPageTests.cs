using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Newtonsoft.Json;
using Practice_Basics_of_Playwright.Core;
using Practice_Basics_of_Playwright.Models;
using Practice_Basics_of_Playwright.Pages;

namespace Practice_Basics_of_Playwright.Tests
{
    public class ProductPageTests : BaseTest
    {
        private readonly Dictionary<string, List<TestCase>> testCaseData;
        private const string productPageSheetName = "ProductPage_Tests";
        private readonly List<TestCase> testCaseList;
        public ProductPageTests()
        {
            testCaseData = excelReader.ReadExcelFile("Test Data/ECT-TestCases.xlsx", [productPageSheetName]);
            testCaseList = testCaseData[productPageSheetName];
            if (testCaseList.Count == 0)
            {
                throw new Exception("No test cases found");
            }
        }
        [Fact]
        public async Task Verify_AddToCartButton_ProductOptions_Visibility_Functionality()
        {
            // Arrange
            var homePage = new HomePage(page);
            var productPage = new ProductPage(page);

            // Act
            // 1. Click on product Image from home page
            await homePage.ClickOnSecondImageInHotsellerSectionAsync();

            // Assert
            // 1. Assert navigation to product page
            await Assertions.Expect(page).ToHaveURLAsync($"{appSettings.BaseUrl}breathe-easy-tank.html");
            // 2. Assert the visibility of add to cart button and product details
            await Assertions.Expect(productPage.addToCartButton).ToBeVisibleAsync();
            await Assertions.Expect(productPage.addToCartButton).ToBeEnabledAsync();
            await Assertions.Expect(productPage.sizeOption).ToBeVisibleAsync();
            await Assertions.Expect(productPage.colorOption).ToBeVisibleAsync();
        }

        [Fact]
        public async Task Verify_QuantityField_Visibility_Functionality()
        {
            // Arrange
            var homePage = new HomePage(page);
            var productPage = new ProductPage(page);

            // Act
            // 1. Click on product Image from home page
            await homePage.ClickOnSecondImageInHotsellerSectionAsync();

            // Assert the visibility and functionality of quantity field
            await Assertions.Expect(productPage.quantityField).ToBeVisibleAsync();
            var isInputField = await productPage.VerifyTypeOfQuantityFieldAsync();
            Assert.True(isInputField, "Quantity field is not a input element");
        }
        [Fact]
        public async Task Verify_AddToCart_WithOptions_ShouldDisplaySuccessMessage()
        {
            // Arrange
            var homePage = new HomePage(page);
            var productPage = new ProductPage(page);

            // Act 
            await homePage.ClickOnSecondImageInHotsellerSectionAsync();
            await productPage.SelectSizeOptionAsync();
            await productPage.SelectColorOptionAsync();
            await productPage.ClickAddToCartButtonAsync();

            // Assert success message
            await Assertions.Expect(productPage.successMessage).ToBeVisibleAsync();
        }
        [Fact]
        public async Task Verify_AddToCart_NotSelectSizeOption_ShouldDisplayErrorMessage()
        {
            // Arrange
            var homePage = new HomePage(page);
            var productPage = new ProductPage(page);

            // Act
            await homePage.ClickOnSecondImageInHotsellerSectionAsync();
            await productPage.SelectColorOptionAsync();
            await productPage.ClickAddToCartButtonAsync();

            // Assert error message
            await Assertions.Expect(productPage.errorMessageForSizeOption).ToBeVisibleAsync();
            await Assertions.Expect(productPage.errorMessageForSizeOption).ToContainTextAsync("This is a required field.");
        }
        [Fact]
        public async Task Verify_AddToCart_NotSelectColorOption_ShouldDisplayErrorMessage()
        {
            // Arrange
            var homePage = new HomePage(page);
            var productPage = new ProductPage(page);

            // Act
            await homePage.ClickOnSecondImageInHotsellerSectionAsync();
            await productPage.SelectSizeOptionAsync();
            await productPage.ClickAddToCartButtonAsync();

            // Assert error message
            await Assertions.Expect(productPage.errorMessageForColorOption).ToBeVisibleAsync();
            await Assertions.Expect(productPage.errorMessageForColorOption).ToContainTextAsync("This is a required field.");
        }
        [Fact]
        public async Task Verify_AddToCart_NotSelectingOptions_ShouldDisplayErrorMessage()
        {
            // Arrange
            var homePage = new HomePage(page);
            var productPage = new ProductPage(page);

            // Act
            await homePage.ClickOnSecondImageInHotsellerSectionAsync();
            await productPage.ClickAddToCartButtonAsync();

            // Assert
            await Assertions.Expect(productPage.errorMessageForSizeOption).ToBeVisibleAsync();
            await Assertions.Expect(productPage.errorMessageForSizeOption).ToContainTextAsync("This is a required field.");
            await Assertions.Expect(productPage.errorMessageForColorOption).ToBeVisibleAsync();
            await Assertions.Expect(productPage.errorMessageForColorOption).ToContainTextAsync("This is a required field.");
        }
        [Fact]
        public async Task Verify_AddToCompareIcon_ShouldBeVisible()
        {
            // Arrange
            var homePage = new HomePage(page);
            var productPage = new ProductPage(page);

            // Act
            await homePage.ClickOnSecondImageInHotsellerSectionAsync();

            // Assert
            await Assertions.Expect(productPage.addToCompareIcon).ToBeVisibleAsync();
        }
        [Fact]
        public async Task Verify_ClickAddToCompareIcon_ShouldBeShowErrorMessage()
        {
            // Arrange
            var homePage = new HomePage(page);
            var productPage = new ProductPage(page);

            // Act
            await homePage.ClickOnSecondImageInHotsellerSectionAsync();

            await page.WaitForURLAsync(page.Url);

            await productPage.addToCompareIcon.ClickAsync();
                      
            // Assert
            await Assertions.Expect(productPage.successMessage).ToBeVisibleAsync();
            await Assertions.Expect(productPage.successMessage).ToContainTextAsync("You added product Breathe-Easy Tank to the comparison list.");
        }
        [Fact]
        public async Task AddToWishList_WithoutLogin_ShouldNavigateAndPrompt()
        {
            // Arrange
            var homePage = new HomePage(page);
            var productPage = new ProductPage(page);

            // Act
            await homePage.ClickOnSecondImageInHotsellerSectionAsync();
            await page.WaitForURLAsync(page.Url);
            await productPage.ClickAddToWishListIconAsync();

            // Assert
            await Assertions.Expect(page).ToHaveTitleAsync("Customer Login");
            await Assertions.Expect(page.GetByRole(AriaRole.Alert).First).ToBeVisibleAsync();
        }

        [Theory]
        [InlineData("PP-010")]
        public async Task AddToWishList_Login_ClickOnAddToWishList_ShouldNavigateAndPrompt(string testCaseId)
        {
            // Arrange
            var homePage = new HomePage(page);
            var productPage = new ProductPage(page);
            var signInPage = new SignInPage(page,appSettings);

            // Arrange test data
            var testCase = testCaseList.Find(l => l.TestCaseId == testCaseId);
            Assert.NotNull(testCase);
            Assert.NotEmpty(testCase.TestData);
            var testData = JsonConvert.DeserializeObject<SignInUser>(testCase.TestData);
            Assert.NotNull(testData);

            // Act
            await homePage.ClickOnSecondImageInHotsellerSectionAsync();
            await page.WaitForURLAsync(page.Url);
            await productPage.ClickAddToWishListIconAsync();

            // Assert
            await Assertions.Expect(page).ToHaveTitleAsync("Customer Login");

            // Log in to the application with valid credentials
            await signInPage.SignInUserFromProductPageAsync(testData);

            // Assert
            // 6. Verify that the user is navigated to the wishlist page.
            await Assertions.Expect(page).ToHaveTitleAsync("My Wish List");

            // 7. Verify that a success message is displayed confirming the item was added to the wishlist.
            await Assertions.Expect(productPage.successMessage).ToBeVisibleAsync();
        }

        [Fact]
        public async Task ProductInfo_ClickOnDetailsTab_ShouldContainDescription()
        {
            // Arrange
            var homePage = new HomePage(page);
            var productPage = new ProductPage(page);

            // Act
            await homePage.ClickOnSecondImageInHotsellerSectionAsync();
            await page.WaitForURLAsync(page.Url);
            await productPage.ClickOnDetailsTabAsync();

            // Assert
            var doesDescriptionContainText = await productPage.DoesDescriptionExistAsync();
            Assert.True(doesDescriptionContainText,"Description does'nt contain text.");
        }

        [Fact]
        public async Task ProductInfo_ClickOnMoreInfoTab_ShouldContainAddtionalAttributes()
        {
            // Arrange
            var homePage = new HomePage(page);
            var productPage = new ProductPage(page);

            // Act
            await homePage.ClickOnSecondImageInHotsellerSectionAsync();
            await page.WaitForURLAsync(page.Url);
            await productPage.ClickOnMoreInfoTabAsync();

            // Assert
            // Verify the table contains rows and each row has data
            await productPage.VerifyMoreInfoContainsAttributesAsync();
        }

        [Fact]
        public async Task ProductInfo_ClickOnReviewsTab_ShouldContainsReviewsAndSubmissionForm()
        {
            // Arrange
            var homePage = new HomePage(page);
            var productPage = new ProductPage(page);

            // Act
            await homePage.ClickOnSecondImageInHotsellerSectionAsync();
            await page.WaitForURLAsync(page.Url);
            await productPage.ClickOnReviewsTabAsync();

            // Assert
            await Assertions.Expect(productPage.CustomersReviews).ToBeVisibleAsync();
            await Assertions.Expect(productPage.ReviewsForm).ToBeVisibleAsync();
        }
    }
}
