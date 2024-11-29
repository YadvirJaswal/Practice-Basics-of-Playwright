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
        private ILocator errorMessageForPasswordConfirmation;
        private ILocator errorMessageForWeakPassword;
        private ILocator errorMessageForEmptyFields;
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
            errorMessageForPasswordConfirmation = page.Locator("#password-confirmation-error");
            errorMessageForWeakPassword = page.Locator("#password-error");
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
                $"Welcome,{signupUser.FirstName}{signupUser.LastName}!", StringComparison.OrdinalIgnoreCase);
        }
        public async Task<bool> HasEmailErrorOccured()
        {
            var emailError = await errorMessageForEmail.IsVisibleAsync();
            var emailErrorText = await errorMessageForEmail.TextContentAsync();

            return emailError && string.Equals(emailErrorText.Replace(" ", ""),
                "Pleaseenteravalidemailaddress(Ex:johndoe@domain.com).", StringComparison.OrdinalIgnoreCase);
        }
        public async Task<bool> HasPasswordConfirmationErrorOccured()
        {
            var passwordConfirmationError = await errorMessageForPasswordConfirmation.IsVisibleAsync();
            var passwordConfirmationErrorText = await errorMessageForPasswordConfirmation.TextContentAsync();

            return passwordConfirmationError && string.Equals(passwordConfirmationErrorText.Replace(" ", ""),
                "Pleaseenterthesamevalueagain.", StringComparison.OrdinalIgnoreCase);
        }
        public async Task<bool> HasErrorOccuredForWeakPassword()
        {
            var weakPasswordError = await errorMessageForWeakPassword.IsVisibleAsync();
            var weakPasswordErrorText = await errorMessageForWeakPassword.TextContentAsync();

            return weakPasswordError && string.Equals(weakPasswordErrorText.Replace(" ", ""),
                "Minimumlengthofthisfieldmustbeequalorgreaterthan8symbols.Leadingandtrailingspaceswillbeignored.",
                StringComparison.OrdinalIgnoreCase);
        }
        public async Task<bool> IsRequiredErrorMessageShownAsync()
        {
            var errorClasses = await page.Locator(".mage-error[for]").AllAsync();

            var expectedForValues = new List<string>
{
    "firstname",
    "lastname",
    "email_address",
    "password",
    "password-confirmation"
};
            var actualForValues = new List<string>();
            foreach (var errorClass in errorClasses)
            {
                var forAttribute = await errorClass.GetAttributeAsync("for");
                if (!string.IsNullOrEmpty(forAttribute))
                {
                    actualForValues.Add(forAttribute);
                }
            }

            bool allValuesPresent = expectedForValues.All(value => actualForValues.Contains(value));
            return allValuesPresent;
        }
    }
}
