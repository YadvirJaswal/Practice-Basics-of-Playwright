using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Practice_Basics_of_Playwright.Core;
using Practice_Basics_of_Playwright.Pages;

namespace Practice_Basics_of_Playwright.Tests
{
    public class ProductPageTests : BaseTest
    {
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
    }
}
