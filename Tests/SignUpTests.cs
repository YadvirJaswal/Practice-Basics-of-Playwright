using Microsoft.Playwright;
using Newtonsoft.Json;
using Practice_Basics_of_Playwright.Core;
using Practice_Basics_of_Playwright.Models;
using Practice_Basics_of_Playwright.Pages;

namespace Practice_Basics_of_Playwright.Tests
{
    public class SignUpTests : BaseTest
    {
        private readonly SignUpTestData testData;      

        public SignUpTests()
        {
           // Load json Test data
            var jsonContent = File.ReadAllText("Test Data/SignUp_TestData.json");
            testData = JsonConvert.DeserializeObject<SignUpTestData>(jsonContent) ?? new SignUpTestData();
            if (testData == null)
            {
                throw new ArgumentNullException(nameof(testData), "Test data is null");
            }

            if (testData.ValidSignUpUser == null || testData.InvalidSignUpUser == null)
            {
                throw new ArgumentNullException(nameof(testData), "Test data user objects are null");
            }
        }

        [Fact]
        public async Task SignUpWithValidUser_ShouldSucceed()
        {
            var signUpPage = new SignUpPage(page, appSettings);
            await signUpPage.SignUp(testData.ValidSignUpUser);
            // Assert navigation and user information
            bool signUpResult = await signUpPage.IsSignUpSuccessfull(testData.ValidSignUpUser);
            Assert.True(signUpResult, "Sign-Up should be successfull");
        }

        [Fact]
        public async Task SignUpWithInValidUser_ShouldSucceed1()
        {
            var signUpPage = new SignUpPage(page, appSettings);
            await signUpPage.SignUp(testData.InvalidSignUpUser);
            // Assert navigation and user information
            bool signUpResult = await signUpPage.IsSignUpSuccessfull(testData.ValidSignUpUser);
            Assert.True(signUpResult, "Sign-Up should be successfull");
        }
    }
}
