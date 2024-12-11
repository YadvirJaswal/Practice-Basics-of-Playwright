
using System.Net.Http.Json;
using Newtonsoft.Json;
using Practice_Basics_of_Playwright.Core;
using Practice_Basics_of_Playwright.Models;
using Practice_Basics_of_Playwright.Pages;

namespace Practice_Basics_of_Playwright.Tests
{
    public class ForgotPasswordTests:BaseTest
    {
        private readonly Dictionary<string, List<TestCase>> testCaseData;
        private string forgotPasswordSheetName = "ForgotPassword_Tests";
        private readonly List<TestCase> testCaseList;
        public ForgotPasswordTests()
        {
            // Read data from excel
            testCaseData = excelReader.ReadExcelFile("Test Data/ECT-TestCases.xlsx", [forgotPasswordSheetName]);
            testCaseList = testCaseData[forgotPasswordSheetName];
            if (testCaseList.Count == 0)
            {
                throw new Exception("No test cases found for Forgot password functionality");
            }
        }
        private FPTestData GetTestCaseData(string testCaseId)
        {
            var testcase = testCaseList.Find(l => l.TestCaseId == testCaseId);
            Assert.NotNull(testcase);
            Assert.NotEmpty(testcase.TestData);
            var testData = JsonConvert.DeserializeObject<FPTestData>(testcase.TestData);
            Assert.NotNull(testData);
            return testData;
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
       
        [Theory]
        [InlineData("FP-002")]
        public async Task ResetPassword_ValidEmail_ShouldBeReset(string testCaseId)
        {
            // Arrange
            // Test data
            var testData = GetTestCaseData(testCaseId);

            // Page
            var fpPage = new ForgotPasswordPage(page, appSettings);
            var signInPage = new SignInPage(page,appSettings);

            // Act
            await signInPage.ClickOnForgotPasswordLinkAsync();
            await fpPage.EnterEmailAndClickOnResetPasswordAsync(testData);
            // Assert
            var isSuccessMessageVisible = await fpPage.IsResetLinkSuccessfullySentAsync();
            Assert.True(isSuccessMessageVisible, "Success Message is not shown");

            //// Act
            //await fpPage.NavigateToMailinatorForResetPasswordLinkAsync(testData);

            ////var isNavigateToPasswordPage = await fpPage.IsNavigateToChangePasswordPage();
            //////await page.WaitForURLAsync($"{appSettings.BaseUrl}customer/account/createpassword/");
            ////Assert.True(isNavigateToPasswordPage, "User is not navigate to password page");

            //await fpPage.EnterPasswordAndConfirmPassword(testData);
        }
        [Theory]
        [InlineData("FP-003")]
        public async Task ResetPassword_InvalidEmail_ShouldShownError(string testCaseId)
        {
            // Arrange test data
            var testData = GetTestCaseData(testCaseId);

            // Arrange page
            var fpPage = new ForgotPasswordPage(page, appSettings);
            var signInPage = new SignInPage(page, appSettings);

            // Act
            await signInPage.ClickOnForgotPasswordLinkAsync();
            await fpPage.EnterEmailAndClickOnResetPasswordAsync(testData);

            // Assert
            var isErrorShown = await fpPage.IsErrorShownAsync();
            Assert.True(isErrorShown, "Error is not shown for invalid email format");
        }

        [Theory]
        [InlineData("FP-004")]
        public async Task ResetPassword_EmptyEmailField_ShouldShownError(string testCaseId)
        {
            // Arrange test data
            var testData = GetTestCaseData(testCaseId);

            // Arrange page
            var fpPage = new ForgotPasswordPage(page, appSettings);
            var signInPage = new SignInPage(page, appSettings);

            // Act
            await signInPage.ClickOnForgotPasswordLinkAsync();
            await fpPage.EnterEmailAndClickOnResetPasswordAsync(testData);

            // Assert
            var isErrorShown = await fpPage.IsErrorShownAsync();
            Assert.True(isErrorShown, "Error is not shown for empty email address field");
        }

        [Theory]
        [InlineData("FP-005")]
        public async Task ResetPassword_UnregisteredEmail_ShouldShownError(string testCaseId)
        {
            // Arrange
            var testData = GetTestCaseData(testCaseId);
            var fpPage = new ForgotPasswordPage(page, appSettings);
            var signInPage = new SignInPage(page,appSettings);

            // Act
            await signInPage.ClickOnForgotPasswordLinkAsync();
            await fpPage.EnterEmailAndClickOnResetPasswordAsync(testData);

            //Assert
            var isSuccessMessageShown = await fpPage.IsResetLinkSuccessfullySentAsync();
            Assert.False(isSuccessMessageShown, "Success Message is Shown");
        }
    }
}
