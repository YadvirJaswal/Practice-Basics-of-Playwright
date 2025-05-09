﻿using Microsoft.Playwright;
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
            var jsonContent = File.ReadAllText("Test Data/SignupData.json");
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
            // Arrange
            var signUpPage = new SignUpPage(page, appSettings);
            var user = testData.ValidSignUpUser;
            user.Email = $"{user.FirstName}.{user.LastName}-{Guid.NewGuid()}@mailinator.com";

            //Act
            await signUpPage.SignUp(user);

            // Assert navigation and user information
            bool signUpResult = await signUpPage.IsSignUpSuccessfull(user);
            Assert.True(signUpResult, "Sign-Up should be successfull");
        }

        [Fact]
        public async Task SignUpWithInValidUser_ShouldNotSucceed()
        {
            // Arrange
            var signUpPage = new SignUpPage(page, appSettings);

            // Act
            await signUpPage.SignUp(testData.InvalidSignUpUser);

            // Assert error message and its text
            bool emailError = await signUpPage.HasEmailErrorOccured();
            Assert.True(emailError, "Email Error should be occured");
        }

        [Fact]
        public async Task SignUp_MisMatchedPasswords_ShowsErrorMessage()
        {
            // Arrange
            var signUpPage = new SignUpPage(page, appSettings);
            var userData = testData.SignUpWithMisMatchedPasswords;
            userData.Email = $"{userData.FirstName}.{userData.LastName}-{Guid.NewGuid()}@mailinator.com";

            // Act
            await signUpPage.SignUp(userData);

            // Assert Error Message for password confirmation and its text
            bool passwordConfirmationError = await signUpPage.HasPasswordConfirmationErrorOccured();
            Assert.True(passwordConfirmationError, "Password Confirmation Error should be occured");
        }
        [Fact]
        public async Task SignUp_WeakPassword_ShowsErrorMessage()
        {
            // Arrange
            var signUpPage = new SignUpPage(page, appSettings);
            var userData = testData.SignUpWithWeakPassword;
            userData.Email = $"{userData.FirstName}.{userData.LastName}-{Guid.NewGuid()}@mailinator.com";

            // Act
            await signUpPage.SignUp(userData);

            // Assert Error Message for password confirmation and its text
            bool weakPasswordError = await signUpPage.HasErrorOccuredForWeakPassword();
            Assert.True(weakPasswordError, "Weak password error message not shown");
        }
        [Fact]
        public async Task RequiredFields_AreEmpty_ShouldShowErrorMessages()
        {
            // Arrange
            var signUpPage = new SignUpPage(page, appSettings);

            // Act
            await signUpPage.SignUp(testData.SignUpWithEmptyFields);

            //Assert
           var isRequiredMessageShown = await signUpPage.IsRequiredErrorMessageShownAsync();
            Assert.True(isRequiredMessageShown, "Error message ");
        }
        
        [Fact]
        public async Task SignUp_RegisteredEmail_ShouldShownError()
        {
            // Arrange
            var signUpPage = new SignUpPage(page, appSettings);

            // Act
            await signUpPage.SignUp(testData.SignUpWithRegisteredEmail);

            //Assert
            var isRegisteredEmailErrorShown = await signUpPage.IsErrorShownForRegisteredEmailAsync();
            Assert.True(isRegisteredEmailErrorShown, "Error message not shown");
        }
        [Fact]
        public async Task PasswordAndConfirmPasswordFields_EnterPasswords_ShouldToggledToHideTheirVisibility()
        {
            // Arrange 
            var signUpPage = new SignUpPage(page, appSettings);

            //Act
            await signUpPage.CreateAnAccountLink.ClickAsync();
            await signUpPage.PasswordInput.FillAsync(testData.ValidSignUpUser.Password);
            await signUpPage.ConfirmPasswordInput.FillAsync(testData.ValidSignUpUser.ConfirmPassword);

            // Assert
            var ArePasswordAndConfirmPasswordFieldsToggledToHideTheirVisiblity = await signUpPage.
                ArePasswordAndConfirmPasswordFieldsToggledToHideTheirVisiblityAsync();
            Assert.True(ArePasswordAndConfirmPasswordFieldsToggledToHideTheirVisiblity, "Password and confirm fields are" +
                "not toggled to hide their visiblity");
        }
    }
}
