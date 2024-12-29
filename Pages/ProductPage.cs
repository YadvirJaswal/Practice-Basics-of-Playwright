using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Practice_Basics_of_Playwright.Models;

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
        private readonly ILocator detailsTab;
        private readonly ILocator descriptionTab;
        private readonly ILocator moreInfoTab;
        private readonly ILocator reviewsTab;
        public readonly ILocator CustomersReviews;
        public readonly ILocator ReviewsForm;
        private readonly ILocator ratingField;
        private readonly ILocator nickNameField;
        private readonly ILocator summaryField;
        private readonly ILocator reviewField;
        private readonly ILocator submitReviewButton;
        public readonly ILocator NickNameFieldError;
        public readonly ILocator SummaryFieldError;
        public readonly ILocator ReviewFieldError;


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
            detailsTab = page.Locator("#tab-label-description");
            descriptionTab = page.Locator("#description");
            moreInfoTab = page.Locator("#tab-label-additional");
            reviewsTab = page.Locator("#tab-label-reviews");
            CustomersReviews = page.Locator("#customer-reviews");
            ReviewsForm = page.Locator("#review-form");
            ratingField = ReviewsForm.Locator(".review-control-vote");
            nickNameField = ReviewsForm.Locator("#nickname_field");
            summaryField = ReviewsForm.Locator("#summary_field");
            reviewField = ReviewsForm.Locator("#review_field");
            submitReviewButton = ReviewsForm.GetByRole(AriaRole.Button, new() { Name = "Submit Review" });
            NickNameFieldError = page.Locator("#nickname_field-error");
            SummaryFieldError = page.Locator("#summary_field-error");
            ReviewFieldError = page.Locator("#review_field-error");
        }
        public async Task ClickAddToWishListIconAsync()
        {
            await addToWishList.ClickAsync();
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
        public async Task ClickOnDetailsTabAsync()
        {
            await detailsTab.ClickAsync();
        }
        public async Task<bool> DoesDescriptionExistAsync()
        {
            var firstParagraph = descriptionTab.Locator("p").First;
            var paragraphExists = await firstParagraph.IsVisibleAsync();
            var paragraphText = await firstParagraph.InnerTextAsync();
            return paragraphExists && !string.IsNullOrEmpty(paragraphText);
        }
        public async Task ClickOnMoreInfoTabAsync()
        {
            await moreInfoTab.ClickAsync();
        }
        public async Task VerifyMoreInfoContainsAttributesAsync()
        {
            // Locate the table
            var table = page.Locator("#product-attribute-specs-table");

            // Assert that the table is visible
            Assert.True(await table.IsVisibleAsync(), "Table is not visible");

            // Locate the table rows
            var rows = table.Locator("tr");

            // Assert that the table contains at least one row
            var rowCount = await rows.CountAsync();
            Assert.True(rowCount > 0, "The table does not contain any row");

            // Assert that each row contains data
            for (int i = 0; i < rowCount; i++)
            {
                var cells = rows.Nth(i).Locator("th, td");
                var cellCount = await cells.CountAsync();

                Assert.True(cellCount > 0, $"Row {i + 1} does not contain any cells.");
                
                var hasNonEmptyCells = false;
                for(int j = 0; j < cellCount; j++)
                {
                    var cellText = await cells.Nth(j).InnerTextAsync();
                    if (!string.IsNullOrEmpty(cellText))
                    {
                        hasNonEmptyCells = true;
                        break;
                    }
                }
                Assert.True(hasNonEmptyCells, $"Row {i + 1} does not contain any non-empty cells.");
                
            }
        }
        public async Task ClickOnReviewsTabAsync()
        {
            await reviewsTab.ClickAsync();
        }
        public async Task AssertFieldsOfReviewFormAsync()
        {
            var ratingField = page.Locator("#Rating_3_label");
            // Assert fields exist
            Assert.NotNull(ratingField);
            Assert.NotNull(nickNameField);
            Assert.NotNull(summaryField);
            Assert.NotNull(reviewField);

            // Assert fields are visible
            Assert.True(await ratingField.IsVisibleAsync(), "Rating field is not visible");

            Assert.True(await nickNameField.IsVisibleAsync(), "Nickname field is not visible");
            Assert.True(await summaryField.IsVisibleAsync(), "Summary field is not visible");
            Assert.True(await reviewField.IsVisibleAsync(), "Review field is not visible");
        }
        public async Task EnterReviewAsync(ReviewTestData reviewData)
        {
            //var rating = page.Locator("#Rating_4");
            //var rating = page.GetByRole(AriaRole.Radio, new() { Name = "ratings[4]" });
            //await rating.CheckAsync();
            await nickNameField.FillAsync(reviewData.Nickname);
            await summaryField.FillAsync(reviewData.Summary);
            await reviewField.FillAsync(reviewData.Review);
            await submitReviewButton.ClickAsync();
        }
    }
}
