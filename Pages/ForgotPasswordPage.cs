using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace Practice_Basics_of_Playwright.Pages
{
    public class ForgotPasswordPage
    {
        private readonly IPage page;
        private ILocator emailInput;
        private readonly string forgotPasswordPageUrl;

        public ForgotPasswordPage(IPage page, AppSettings appSettings)
        {
            this.page = page;
            forgotPasswordPageUrl = $"{appSettings.BaseUrl}customer/account/forgotpassword/";
            emailInput = page.Locator("#email_address");
        }
        public async Task<bool> IsNavigatedToForgotPasswordPageAsync()
        {
            var pageUrl = page.Url;
            var pageTitle = await page.Locator(".page-title").TextContentAsync();
            return pageUrl == forgotPasswordPageUrl && string.Equals(pageTitle.Replace(" ", "").Trim(),
                "ForgotYourPassword?", StringComparison.OrdinalIgnoreCase);
        }
        public async Task<bool> IsEmailInputFieldVisibleAsync()
        {
            var isEmailInputfieldVisible = await emailInput.IsVisibleAsync();
            return isEmailInputfieldVisible;
        }
    }
}
