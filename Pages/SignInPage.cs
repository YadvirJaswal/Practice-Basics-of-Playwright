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
        public async Task<bool> IsErrorShownForInvalidPasswordAsync()
        {
            await errorMessageForPassword.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible
            });
            await errorMessageForPassword.ScrollIntoViewIfNeededAsync();
            var isErrorMessageVisible = await errorMessageForPassword.IsVisibleAsync();
            return isErrorMessageVisible;
        }
    }
}
