using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.VisualBasic;
using Practice_Basics_of_Playwright.Models;

namespace Practice_Basics_of_Playwright.Pages
{
    public class SignInPage
    {
        private readonly IPage page;
        private readonly string homePageUrl;

        private ILocator signInNavigationButton;
        private ILocator emailInput;
        private ILocator passwordInput;
        private ILocator signInButton;
        private ILocator welcomeNote;
        private ILocator errorMessageForPassword;
        private ILocator errorMessageForEmail;
         
        public SignInPage(IPage page, AppSettings appSettings)
        {
            this.page = page;
            homePageUrl = appSettings.BaseUrl;

            signInNavigationButton = page.Locator(".panel.header > ul > li:nth-child(2) > a");
            emailInput = page.Locator("#email");
            passwordInput = page.Locator("#pass");
            signInButton = page.Locator(".primary#send2");
            welcomeNote = page.Locator(".panel.header > ul > li.greet.welcome > span.logged-in");
            errorMessageForPassword = page.GetByRole(AriaRole.Alert).First;
            errorMessageForEmail = page.Locator("#email-error");

        }
        public async Task SignInUser(SignInUser signInUser)
        {
            await signInNavigationButton.ClickAsync();
            await emailInput.ClearAsync();
            await emailInput.FillAsync(signInUser.Email);
            await passwordInput.ClearAsync();
            await passwordInput.FillAsync(signInUser.Password);
            await signInButton.ClickAsync();
        }
        public async Task<bool> IsSignInSuccessfullAsync(SignInUser signInUser)
        {
            var currentUrl = page.Url;
            await welcomeNote.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible
            });
            var isWelcomeNoteVisible = await welcomeNote.IsVisibleAsync();
            if (isWelcomeNoteVisible)
            {
                var welcomeNoteText = await welcomeNote.TextContentAsync();

                return currentUrl == homePageUrl &&
                    string.Equals(welcomeNoteText.Replace(" ", ""), $"welcome,{signInUser.FirstName}{signInUser.LastName}!",
                    StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }
        public async Task<bool> IsErrorShownAsync()
        {
            await errorMessageForPassword.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible
            });
            await errorMessageForPassword.ScrollIntoViewIfNeededAsync();
            var isErrorMessageVisible = await errorMessageForPassword.IsVisibleAsync();
            return isErrorMessageVisible;
        }
        public async Task<bool> IsErrorShownForInvalidEmailAsync()
        {
            var isErrorShown = await errorMessageForEmail.IsVisibleAsync();
            var errorMessageForEmailText = await errorMessageForEmail.TextContentAsync();
            return isErrorShown && string.Equals(errorMessageForEmailText.Replace(" ",""),
                $"Pleaseenteravalidemailaddress(Ex:johndoe@domain.com).", StringComparison.OrdinalIgnoreCase);
        }
        public async Task<bool> IsRequiredErrorMessageShownAsync()
        {
            var errorClasses = await page.Locator(".mage-error[for]").AllAsync();

            var expectedForValues = new List<string>
            {
                "email",
                "pass"
            };
            var actualValues = new List<string>();
            bool isErrorTextCorrect = false;
            foreach (var errorClass in errorClasses)
            {
                var forAttributeValue = await errorClass.GetAttributeAsync("for");
                if (!string.IsNullOrEmpty(forAttributeValue))
                {
                    actualValues.Add(forAttributeValue);
                }
                var errorMessageText = await errorClass.TextContentAsync();
                isErrorTextCorrect = string.Equals(errorMessageText.Replace(" ", ""), "Thisisarequiredfield.",
                    StringComparison.OrdinalIgnoreCase);
            }
            bool areValuesPresent = expectedForValues.All(value => actualValues.Contains(value));
            return areValuesPresent && isErrorTextCorrect;
        }
    }
}
