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

        public ProductPage(IPage page)
        {
            this.page = page;
            title = page.Locator(".base");
            errorMessage = page.GetByRole(AriaRole.Alert).First;
        }
        public async Task<string> GetTitleAfterClicking()
        {
            var titleText = await title.TextContentAsync() ?? "";
            return titleText;
        }
    }
}
