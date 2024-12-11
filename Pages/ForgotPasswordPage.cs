using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Practice_Basics_of_Playwright.Models;

namespace Practice_Basics_of_Playwright.Pages
{
    public class ForgotPasswordPage
    {
        private readonly IPage page;
        private ILocator emailInput;
        private readonly string forgotPasswordPageUrl;
        private readonly string passwordPageUrl;
        private ILocator resetPasswordButton;
        private ILocator mailinatorSearchField;
        private ILocator mailinatorSearchButton;
        private ILocator passwordInput;
        private ILocator confirmPasswordInput;
        private ILocator setNewPasswordButton;
        private ILocator emailError;

        public ForgotPasswordPage(IPage page, AppSettings appSettings)
        {
            this.page = page;
            forgotPasswordPageUrl = $"{appSettings.BaseUrl}customer/account/forgotpassword/";
            passwordPageUrl = $"{appSettings.BaseUrl}customer/account/createpassword/";
            emailInput = page.Locator("#email_address");
            resetPasswordButton = page.GetByText("Reset My Password");
            mailinatorSearchField = page.Locator("#inbox_field");
            mailinatorSearchButton = page.Locator(".primary-btn");
            passwordInput = page.Locator("#password");
            confirmPasswordInput = page.Locator("#password-confirmation");
            setNewPasswordButton = page.GetByRole(AriaRole.Button, new() { Name = "Set a New Password" });
            emailError = page.Locator("#email_address-error");
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
        public async Task EnterEmailAndClickOnResetPasswordAsync(FPTestData fPTestData)
        {
            await emailInput.FillAsync(fPTestData.Email);
            await resetPasswordButton.ClickAsync();
        }
        public async Task<bool> IsResetLinkSuccessfullySentAsync()
        {
            var successMessage = page.GetByRole(AriaRole.Alert).First;
            await successMessage.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible
            });
            var isSuccessMessageVisible = await successMessage.IsVisibleAsync();
            return isSuccessMessageVisible;
        }
        public async Task NavigateToMailinatorForResetPasswordLinkAsync(FPTestData fPTestData)
        {
            // Access Mailinator inbox
            await page.GotoAsync("https://www.mailinator.com/v4/public/inboxes.jsp");

            //Search for Password Reset Email 
            await mailinatorSearchField.FillAsync(fPTestData.Email);
            await mailinatorSearchButton.ClickAsync();

            var mailInInBox = page.GetByRole(AriaRole.Row, new() { Name = "CustomerSupport Reset your Main Website Store password just now" });
            await mailInInBox.DblClickAsync();

            await page.WaitForSelectorAsync("iframe");
            // Switch to the iframe containing the email content
            var emailFrame = page.Frame("html_msg_body");

            var setPasswordButton = emailFrame.GetByRole(AriaRole.Link, new() { Name = "Set a New Password" });
            await setPasswordButton.ClickAsync();

            // Wait for the URL to match the target URL
            //await page.WaitForRequestFinishedAsync();
            //await page.WaitForURLAsync("https://magento.softwaretestingboard.com/customer/account/createpassword/");
        }
        
        
        public async Task<bool> IsErrorShownAsync()
        {
            await emailError.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible
            });
            var isErrorShown = await emailError.IsVisibleAsync();
            return isErrorShown;
        }
        
    }
}
