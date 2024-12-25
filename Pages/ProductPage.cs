using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace Practice_Basics_of_Playwright.Pages
{
    public class ProductPage
    {
        private readonly IPage page;
        private readonly ILocator title;
        public readonly ILocator errorMessage;
        public readonly ILocator addToCartButton;
        public readonly ILocator sizeOption;
        public readonly ILocator colorOption;
        public readonly ILocator quantityField;
        public readonly ILocator successMessage;
        public readonly ILocator errorMessageForSizeOption;
        public readonly ILocator errorMessageForColorOption;
        private readonly ILocator productInfo;
        public readonly ILocator addToCompareIcon;
        public readonly ILocator addToWishList;

        public ProductPage(IPage page)
        {
            this.page = page;
            title = page.Locator(".base");
            errorMessage = page.GetByRole(AriaRole.Alert).First;
            addToCartButton = page.GetByRole(AriaRole.Button, new() { Name = "Add to Cart" });
            sizeOption = page.Locator(".swatch-attribute.size");
            colorOption = page.Locator(".swatch-attribute.color");
            quantityField = page.Locator("#qty");
            successMessage = page.GetByRole(AriaRole.Alert).First;
            errorMessageForSizeOption = page.Locator("#super_attribute\\[143\\]-error");
            errorMessageForColorOption = page.Locator("#super_attribute\\[93\\]-error");
            productInfo = page.Locator(".product-info-main");
            addToCompareIcon = productInfo.GetByRole(AriaRole.Link, new() { Name = "Add to Compare" });
            addToWishList = productInfo.GetByRole(AriaRole.Link, new() { Name = "Add to Wish List" });
        }
        public async Task<string> GetTitleAfterClicking()
        {
            var titleText = await title.TextContentAsync() ?? "";
            return titleText;
        }
        public async Task<bool> VerifyTypeOfQuantityFieldAsync()
        {
            var type = await quantityField.GetAttributeAsync("type");
            return type == "number";
        }
        public async Task SelectColorOptionAsync()
        {
            // Select white color
            await colorOption.Locator("#option-label-color-93-item-59").ClickAsync();
        }
        public async Task SelectSizeOptionAsync()
        {
            // Select small(S) size
            await sizeOption.Locator("#option-label-size-143-item-167").ClickAsync();
        }
        public async Task ClickAddToCartButtonAsync()
        {
            await addToCartButton.ClickAsync();
        }
    }
}
