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
        public ILocator CreateAnAccountLink;
        private ILocator firstNameInput;
        private ILocator lastNameInput;
        private ILocator emailInput;
        public ILocator PasswordInput;
        public ILocator ConfirmPasswordInput;
        private ILocator createAccountButton;
        private ILocator errorMessageForEmail;
        private ILocator errorMessageForPasswordConfirmation;
        private ILocator errorMessageForWeakPassword;
        private ILocator errorMessageForEmptyFields;
        private ILocator errorMessageForRegisteredEmail;
        private ILocator accountInformation;

        public SignUpPage(IPage page, AppSettings appSettings)
        {
            this.page = page;

            myAccountPageUrl = $"{appSettings.BaseUrl}customer/account/";
            CreateAnAccountLink = page.Locator(".panel.header > ul > li:nth-child(3) > a");
            firstNameInput = page.Locator("#firstname");
            lastNameInput = page.Locator("#lastname");
            emailInput = page.Locator("#email_address");
            PasswordInput = page.Locator("#password");
            ConfirmPasswordInput = page.Locator("#password-confirmation");
            createAccountButton = page.Locator("button.submit");
            errorMessageForEmail = page.Locator("#email_address-error");
            errorMessageForPasswordConfirmation = page.Locator("#password-confirmation-error");
            errorMessageForWeakPassword = page.Locator("#password-error");
            errorMessageForRegisteredEmail = page.Locator(".page.messages");
            accountInformation = page.Locator(".panel.header > ul > li.greet.welcome > span.logged-in");
        }

        public async Task SignUp(SignUpUser signupUser)
        {
            await CreateAnAccountLink.ClickAsync();
            await firstNameInput.FillAsync(signupUser.FirstName);
            await lastNameInput.FillAsync(signupUser.LastName);
            await emailInput.FillAsync(signupUser.Email);
            await PasswordInput.FillAsync(signupUser.Password);
            await ConfirmPasswordInput.FillAsync(signupUser.ConfirmPassword);
            await createAccountButton.ClickAsync();

        }
        public async Task SignUpUserFromSigninPage(SignUpUser signupUser)
        {
            
            await firstNameInput.FillAsync(signupUser.FirstName);
            await lastNameInput.FillAsync(signupUser.LastName);
            await emailInput.FillAsync(signupUser.Email);
            await PasswordInput.FillAsync(signupUser.Password);
            await ConfirmPasswordInput.FillAsync(signupUser.ConfirmPassword);
            await createAccountButton.ClickAsync();
        }

        public async Task<bool> IsSignUpSuccessfull(SignUpUser signupUser)
        {
            await page.WaitForURLAsync(myAccountPageUrl);
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
        public async Task<bool> IsErrorShownForRegisteredEmailAsync()
        {
            await errorMessageForRegisteredEmail.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
            });
            var registeredEmailError = await errorMessageForRegisteredEmail.IsVisibleAsync();
            return registeredEmailError;
        }
        public async Task<bool> ArePasswordAndConfirmPasswordFieldsToggledToHideTheirVisiblityAsync()
        {
            var passwordFieldAttribute = await PasswordInput.GetAttributeAsync("type");
            var confirmPasswordFieldAttribute = await ConfirmPasswordInput.GetAttributeAsync("type");

            return passwordFieldAttribute == "password" && confirmPasswordFieldAttribute == "password";
        }
        
    }
}
