using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Practice_Basics_of_Playwright.Models;

namespace Practice_Basics_of_Playwright.Pages
{
    public class SignInPage
    {
        private readonly IPage page;
        private readonly string homePageUrl;
        

        public ILocator signInNavigationButton;
        private ILocator emailInput;
        private ILocator passwordInput;
        private ILocator signInButton;
        private ILocator welcomeNote;
        private ILocator errorMessageForPassword;
        private ILocator errorMessageForEmail;
        private ILocator createAnAccountButton;
        private ILocator forgotPasswordLink;

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
            createAnAccountButton = page.GetByLabel("New Customers").GetByRole(AriaRole.Link, new() 
                                    { Name = "Create an Account" });
            forgotPasswordLink = page.Locator(".action.remind");
        }
        public async Task SignInUserAsync(SignInUser signInUser)
        {
            await signInNavigationButton.ClickAsync();
            await emailInput.ClearAsync();
            await emailInput.FillAsync(signInUser.Email);
            await passwordInput.ClearAsync();
            await passwordInput.FillAsync(signInUser.Password);
            await signInButton.ClickAsync();
        }
        public async Task EnterPasswordAsync(SignInUser signInUser)
        {
            await signInNavigationButton.ClickAsync();
            await passwordInput.ClearAsync();
            await passwordInput.FillAsync(signInUser.Password);
        }
        public async Task ClickOnCreateAnAccountButtonAsync()
        {
            await signInNavigationButton.ClickAsync();
            await createAnAccountButton.ClickAsync();
        }
        public async Task ClickOnForgotPasswordLinkAsync()
        {
            await signInNavigationButton.ClickAsync();
            await forgotPasswordLink.ClickAsync();
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
            return isErrorShown && string.Equals(errorMessageForEmailText.Replace(" ", ""),
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
        public async Task<bool> IsPasswordFieldToggledToHideItsVisibilityAsync()
        {
            var passFieldType = await passwordInput.GetAttributeAsync("type");
            return passFieldType == "password";
        }
        public async Task<bool> AreRequiredFiledsMarkedAsMandatoryAsync()
        {
            await page.Locator("#login-form > fieldset.login").WaitForAsync();

            var requiredClass = page.Locator("#login-form > fieldset.login");
            var requiredFieldClasses = await requiredClass.Locator(".required > div> input").AllAsync();
            await page.WaitForRequestFinishedAsync();
            var requiredAttribute = "";
            foreach (var requiredFieldClass in requiredFieldClasses)
            {
                requiredAttribute = await requiredFieldClass.GetAttributeAsync("aria-required");
            }
            return string.Equals(requiredAttribute, "true");
        }
        public async Task<bool> IsNavigateToSignUpPageAsync()
        {
            
            var actualUrl =   page.Url;
            var expectedUrl = "https://magento.softwaretestingboard.com/customer/account/create/";
            return actualUrl == expectedUrl;
        }
        
    }
}
