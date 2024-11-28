using Microsoft.Playwright;
using Practice_Basics_of_Playwright.Core;
using Practice_Basics_of_Playwright.Models;

namespace Practice_Basics_of_Playwright.Pages
{
    public class SignUpPage
    {
        private readonly IPage page;
        
        private readonly string myAccountPageUrl;

        // Locators
        private ILocator createAnAccountLink;
        private ILocator firstNameInput;
        private ILocator lastNameInput;
        private ILocator emailInput;
        private ILocator passwordInput;
        private ILocator confirmPasswordInput;
        private ILocator createAccountButton;
        private ILocator errorMessageForEmail;
        private ILocator errorMessageForPassword;
        private ILocator accountInformation;

        public SignUpPage(IPage page, AppSettings appSettings)
        {
            this.page = page;           

            myAccountPageUrl = $"{appSettings.BaseUrl}customer/account/";
            createAnAccountLink = page.Locator(".panel.header > ul > li:nth-child(3) > a");
            firstNameInput = page.Locator("#firstname");
            lastNameInput = page.Locator("#lastname");
            emailInput = page.Locator("#email_address");
            passwordInput = page.Locator("#password");
            confirmPasswordInput = page.Locator("#password-confirmation");
            createAccountButton = page.Locator("button.submit");
            errorMessageForEmail = page.Locator("#email_address-error");
            errorMessageForPassword = page.Locator("#password-error");
            accountInformation = page.Locator(".panel.header > ul > li.greet.welcome > span");
        }

        public async Task SignUp(SignUpUser signupUser)
        {
            await createAnAccountLink.ClickAsync();
            await firstNameInput.FillAsync(signupUser.FirstName);
            await lastNameInput.FillAsync(signupUser.LastName);
            await emailInput.FillAsync(signupUser.Email);
            await passwordInput.FillAsync(signupUser.Password);
            await confirmPasswordInput.FillAsync(signupUser.ConfirmPassword);
            await createAccountButton.ClickAsync();

        }


        public async Task<bool> IsSignUpSuccessfull(SignUpUser signupUser)
        {
            string currentUrl = page.Url;
            string accountInformationText = await accountInformation.TextContentAsync() ?? "";

            return currentUrl == myAccountPageUrl && 
                string.Equals(accountInformationText.Replace(" ", ""),
                $"Welcome,{signupUser.FirstName}{signupUser.LastName}!",StringComparison.OrdinalIgnoreCase);
        }
    }
}
