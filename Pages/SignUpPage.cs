using Microsoft.Playwright;
using Practice_Basics_of_Playwright.Models;

namespace Practice_Basics_of_Playwright.Pages
{
    public class SignUpPage
    {
        private readonly IPage page;
        private readonly AppSettings appSettings;
        private readonly string myAccountPageUrl;

        // Locators
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

            this.appSettings = appSettings;

            myAccountPageUrl = $"{appSettings.BaseUrl}/customer/account/";

            firstNameInput = page.Locator("#firstname");
            lastNameInput = page.Locator("#lastname");
            emailInput = page.Locator("#email_address");
            passwordInput = page.Locator("#password");
            confirmPasswordInput = page.Locator("#password-confirmation");
            createAccountButton = page.Locator("button.submit");
            errorMessageForEmail = page.Locator("#email_address-error");
            errorMessageForPassword = page.Locator("#password-error");
            accountInformation = page.Locator(".box-content");
        }
        public async Task SignUp(SignupModel signupModel)
        {
            await firstNameInput.FillAsync(signupModel.FirstName);
            await lastNameInput.FillAsync(signupModel.LastName);
            await emailInput.FillAsync(signupModel.Email);
            await passwordInput.FillAsync(signupModel.Password);
            await confirmPasswordInput.FillAsync(signupModel.ConfirmPassword);
            await createAccountButton.ClickAsync();

        }
        private async Task<string> GetAccountInformationAsync()
        {
            return await accountInformation.TextContentAsync() ?? "";
        }

        public async Task<bool> IsSignUpSuccessfull(SignupModel signupModel)
        {
            string currentUrl = page.Url;
            string accountInformationText = await GetAccountInformationAsync();

            return currentUrl == myAccountPageUrl && accountInformationText == $"{signupModel.FirstName} {signupModel.LastName}\n{signupModel.Email}";
        }
    }
}
