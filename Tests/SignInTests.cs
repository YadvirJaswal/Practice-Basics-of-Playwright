using Newtonsoft.Json;
using Practice_Basics_of_Playwright.Core;
using Practice_Basics_of_Playwright.Models;
using Practice_Basics_of_Playwright.Pages;
using Practice_Basics_of_Playwright.Utilities;

namespace Practice_Basics_of_Playwright.Tests
{
    public class SignInTests : BaseTest
    {
        private readonly SignInTestData testData_SignIn;
        private readonly SignUpTestData testData_SignUp;
        private readonly Dictionary<string, List<TestCase>> testCaseData;

        private const string signupSheetName = "SignUp_Tests";
        private const string signInSheetName = "SignIn_Tests";

        private readonly List<TestCase> signInTestCasesList;
        private readonly List<TestCase> signUpTestCasesList;
        
       
        public SignInTests()
        {
            // Read test data from excel
            testCaseData = excelReader.ReadExcelFile("Test Data/ECT-TestCases.xlsx", [signupSheetName,signInSheetName]);

            signInTestCasesList = testCaseData[signInSheetName];
            signUpTestCasesList = testCaseData[signupSheetName];

            if (signInTestCasesList.Count == 0)
            {
                throw new Exception("No test cases found");
            }

            
            

            // Load data from json
            var jsonContent_SignIn = File.ReadAllText("Test Data/SignInData.json");
            testData_SignIn = JsonConvert.DeserializeObject<SignInTestData>(jsonContent_SignIn) ?? new SignInTestData();

            var jsonContent_SignUp = File.ReadAllText("Test Data/SignupData.json");
            testData_SignUp = JsonConvert.DeserializeObject<SignUpTestData>(jsonContent_SignUp) ?? new SignUpTestData();
        }
        private SignInUser GetTestCaseData(string testCaseId)
        {
            var testCase = signInTestCasesList.Find(l => l.TestCaseId == testCaseId);
            Assert.NotNull(testCase);
            Assert.NotEmpty(testCase.TestData);
            var testData = JsonConvert.DeserializeObject<SignInUser>(testCase.TestData);
            Assert.NotNull(testData);
            return testData;
        }
        [Theory]
        [InlineData("TC-SIGNIN-001")]
        public async Task SignIn_ValidCredentials_ShouldSucceed(string testCaseId)
        {
            try
            {
                // update in progress status in Excel
                // Arrange
                var testData = GetTestData(testCaseId);

                var signInPage = new SignInPage(page, appSettings);

                // Act
                await signInPage.SignInUserAsync(testData);

                // Assert
                var isSignInSuccessfull = await signInPage.IsSignInSuccessfullAsync(testData);
                Assert.True(isSignInSuccessfull, "User is not signed In");

                // if it reaches here it means the test cases has passes - update passed status in excel
            }
            catch (Exception ex)
            {
                // testcase failed - update failed status in excel
                // create a bug in Github issues
            }
        }

        private SignInUser GetTestData(string testCaseId)
        {
            var testCase = signInTestCasesList.Find(l => l.TestCaseId == testCaseId);
            Assert.NotNull(testCase);
            Assert.NotEmpty(testCase.TestData);
            var testData = JsonConvert.DeserializeObject<SignInUser>(testCase.TestData);
            Assert.NotNull(testData);
            return testData;
        }

        [Theory]
        [InlineData("TC-SIGNIN-002")]
        public async Task SignIn_ValidEmailAndInvalidPassword_ErrorMessageShouldShown(string testCaseId)
        {
            // Arrange
            var signInPage = new SignInPage(page, appSettings);
            var testData = GetTestCaseData(testCaseId);

            // Update status in Excel to "In Progress"
            var updater = new TestStatusUpdater();
            string status = "In Progress";
            updater.UpdateTestStatus("Test Data/ECT-TestCases.xlsx", signInSheetName, testCaseId, status);

            // Act
            await signInPage.SignInUserAsync(testData);

            // Assert
            var isErrorMessageShown = await signInPage.IsErrorShownAsync();
            Assert.True(isErrorMessageShown, "Error message is not shown");
            if (isErrorMessageShown)
            {
                status = "Passed";
            }
            else
            {
                status = "Failed";
            }
            // Update test status in Excel after test execution
            updater.UpdateTestStatus("Test Data/ECT-TestCases.xlsx", signInSheetName, testCaseId, status);
        }

       
        [Theory]
        [InlineData("TC-SIGNIN-003")]
        public async Task SignIn_InvalidEmailAndValidPassword_ErrorMessageShouldShown(string testCaseId)
        {
            // Arrange
            var signInPage = new SignInPage(page, appSettings);
            var testData = GetTestCaseData(testCaseId);

            // Act
            await signInPage.SignInUserAsync(testData);

            //Assert
            var isErrorShown = await signInPage.IsErrorShownForInvalidEmailAsync();
            Assert.True(isErrorShown, "Error message is not shown");
        }
        [Theory]
        [InlineData("TC-SIGNIN-004")]
        public async Task RequiredFields_AreEmpty_ShouldShowErrorMessages(string testCaseId)
        {
            // Arrange
            var signInPage = new SignInPage(page, appSettings);
            var testData = GetTestCaseData(testCaseId);

            // Act
            await signInPage.SignInUserAsync(testData);

            //Assert
            var isRequiredMessageShown = await signInPage.IsRequiredErrorMessageShownAsync();
            Assert.True(isRequiredMessageShown, "Error message ");
        }
        [Theory]
        [InlineData("TC-SIGNIN-005")]
        public async Task SignIn_UnregisteredEmail_ShouldShownErrorMessage(string testCaseId)
        {
            // Arrange
            var signInPage = new SignInPage(page, appSettings);
            var testData = GetTestData(testCaseId);

            // Act
            await signInPage.SignInUserAsync(testData);

            // Assert
            var isErrorMessageShown = await signInPage.IsErrorShownAsync();
            Assert.True(isErrorMessageShown, "Error message is not shown");
        }
        [Theory]
        [InlineData("TC-SIGNIN-006")]
        public async Task Password_EnterPassword_ShouldToggledToHideItsVisibility(string testCaseId)
        {
            // Arrange 
            var signInPage = new SignInPage(page, appSettings);
            var testData = GetTestData(testCaseId);

            //Act
            await signInPage.EnterPasswordAsync(testData);

            // Assert
            var isPasswordToggledToHideItsVisiblity = await signInPage.IsPasswordFieldToggledToHideItsVisibilityAsync();
            Assert.True(isPasswordToggledToHideItsVisiblity, "Password is not toggled to hide its visiblity");
        }
        [Fact]
        public async Task RequiredFileds_ShouldBeMarkedAsRequired()
        {
            // Arrange 
            var signInPage = new SignInPage(page, appSettings);

            //Act
            await signInPage.EnterPasswordAsync(testData_SignIn.SignInWithValidEmailInvalidPassword);
         
            // Assert
            var areRequiredFieldsMarked = await signInPage.AreRequiredFiledsMarkedAsMandatoryAsync();
            Assert.True(areRequiredFieldsMarked, "Required Fields are not marked");
        }

        [Theory]
        [InlineData("TC-SIGNUP-001")]
        public async Task SignInPage_VerifyCreateAnAccountOption_ShouldBeRegisteredAndSignedIn(string testCaseId)
        {
            // Arrange 
            var testCase = signUpTestCasesList.Find(l => l.TestCaseId == testCaseId);
            Assert.NotNull(testCase);
            Assert.NotEmpty(testCase.TestData);
            var testUserData = JsonConvert.DeserializeObject<SignUpUser>(testCase.TestData);
            Assert.NotNull(testUserData);

            var signInPage = new SignInPage(page, appSettings);
            var signUpPage = new SignUpPage(page, appSettings);
            
            testUserData.Email = $"{testUserData.FirstName}.{testUserData.LastName}-{Guid.NewGuid()}@mailinator.com";

            // Act
            await signInPage.ClickOnCreateAnAccountButtonAsync();

            // Assert Navigation
            var isNavigateToSignUpPage = await signInPage.IsNavigateToSignUpPageAsync();
            Assert.True(isNavigateToSignUpPage, "User is not navigated to signup page.");

            //Act
            await signUpPage.SignUpUserFromSigninPage(testUserData);


            // Assert registeration
            var isSignUpSuccessfull = await signUpPage.IsSignUpSuccessfull(testUserData);
            Assert.True(isSignUpSuccessfull, "User is not registered.");
        }
        [Theory]
        [InlineData("TC-SIGNIN-009")]
        public async Task Email_VerifyCaseSenstivity_ShouldBeSignedIn(string testCaseId)
        {
            // Arrange
            var signInPage = new SignInPage(page,appSettings);
            var testData = GetTestData(testCaseId);

            // Act
            await signInPage.SignInUserAsync(testData);

            //Assert
            var isSignInSuccessful = await signInPage.IsSignInSuccessfullAsync(testData);
            Assert.True(isSignInSuccessful, "User is not signed In");
        }
        [Theory]
        [InlineData("TC-SIGNIN-010")]
        public async Task Password_VerifyCaseSenstivity_SignInShouldFail(string testCaseId)
        {
            //Arrange
            var signInPage = new SignInPage(page, appSettings);
            var testData = GetTestData(testCaseId);

            // Act
            await signInPage.SignInUserAsync(testData);

            // Assert
            var isErrorShown = await signInPage.IsErrorShownAsync();
            Assert.True(isErrorShown, "Error is not shown");
        }
        [Fact]
        public async Task ForgotPassword_ClickOnForgotPasswordLink_ShouldBeRedirectedToForgotPasswordPage()
        {
            // Arrange
            var signInPage = new SignInPage(page,appSettings);
            var forgotPasswordPage = new ForgotPasswordPage(page, appSettings);

            // Act
            await signInPage.ClickOnForgotPasswordLinkAsync();

            // Assert
            var isNavigatedToForgotPasswordPage = await forgotPasswordPage.IsNavigatedToForgotPasswordPageAsync();
            Assert.True(isNavigatedToForgotPasswordPage, "User is not navigated to Forgot Password Page.");
        }
        [Theory]
        [InlineData("TC-SIGNIN-012")]
        public async Task ResetPassword_SignInWithNewPassword_ShouldSucceed( string testCaseId)
        {
            // Arrange 
            var testData = GetTestData(testCaseId);
            var signinPage = new SignInPage(page, appSettings);

            // Act
            await signinPage.SignInUserAsync(testData);

            // Assert
            var isSignInSuccessfull = await signinPage.IsSignInSuccessfullAsync(testData);
            Assert.True(isSignInSuccessfull, "User is not signed In");
        }
    }
}
