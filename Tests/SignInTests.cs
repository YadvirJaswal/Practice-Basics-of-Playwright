using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Practice_Basics_of_Playwright.Core;
using Practice_Basics_of_Playwright.Models;
using Practice_Basics_of_Playwright.Pages;

namespace Practice_Basics_of_Playwright.Tests
{
    public class SignInTests : BaseTest
    {
        private readonly SignInTestData testData;
        public SignInTests()
        {
            // Load data from json
            var jsonContent = File.ReadAllText("Test Data/SignInData.json");
            testData = JsonConvert.DeserializeObject<SignInTestData>(jsonContent) ?? new SignInTestData();
        }

        [Fact]
        public async Task SignIn_ValidCredentials_ShouldSucceed()
        {
            // Arrange
            var signInPage = new SignInPage(page,appSettings);
            var userData = testData.SignInWithValidCredentials;

            // Act
            await signInPage.SignInUser(userData);

            // Assert
            var isSignInSuccessfull = await signInPage.IsSignInSuccessfullAsync(userData);
            Assert.True(isSignInSuccessfull, "User is not signed In");
        }
        [Fact]
        public async Task SignIn_ValidEmailAndInvalidPassword_ErrorMessageShouldShown()
        {
            // Arrange
            var signInPage = new SignInPage(page, appSettings);
            
            // Act
            await signInPage.SignInUser(testData.SignInWithValidEmailInvalidPassword);

            // Assert
            var isErrorMessageShown = await signInPage.IsErrorShownForInvalidPasswordAsync();
            Assert.True(isErrorMessageShown, "Error message is not shown");
        }
        [Fact]
        public async Task SignIn_InvalidEmailAndValidPassword_ErrorMessageShouldShown()
        {
            // Arrange
            var signInPage = new SignInPage(page, appSettings);

            // Act
            await signInPage.SignInUser(testData.SignInWithInvalidEmailValidPassword);

            //Assert
            var isErrorShown = await signInPage.IsErrorShownForInvalidEmailAsync();
            Assert.True(isErrorShown, "Error message is not shown");
        }
        [Fact]
        public async Task RequiredFields_AreEmpty_ShouldShowErrorMessages()
        {
            // Arrange
            var signInPage = new SignInPage(page, appSettings);

            // Act
            await signInPage.SignInUser(testData.SignInWithEmptyFields);

            //Assert
            var isRequiredMessageShown = await signInPage.IsRequiredErrorMessageShownAsync();
            Assert.True(isRequiredMessageShown, "Error message ");
        }

    }
}
