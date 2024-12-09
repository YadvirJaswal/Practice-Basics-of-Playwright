using Practice_Basics_of_Playwright.Core;
using Practice_Basics_of_Playwright.Pages;

namespace Practice_Basics_of_Playwright.Tests
{
    public class ForgotPasswordTests:BaseTest
    {
        public ForgotPasswordTests()
        {
            
        }
        [Fact]
        public async Task EmailInputField_ShouldBeVisible()
        {
            // Arrange
            var signInPage = new SignInPage(page,appSettings);
            var forgotPasswordPage = new ForgotPasswordPage(page, appSettings);

            // Act
            await signInPage.ClickOnForgotPasswordLinkAsync();

            // Assert
            var isEmailInputFieldVisible = await forgotPasswordPage.IsEmailInputFieldVisibleAsync();
            Assert.True(isEmailInputFieldVisible,"Email input field is not visible on forgot password page");
        }
    }
}
