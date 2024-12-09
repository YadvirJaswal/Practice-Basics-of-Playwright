using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_Basics_of_Playwright.Models
{
    public class SignInTestData
    {
        public SignInUser SignInWithValidCredentials { get; set; }
        public SignInUser SignInWithValidEmailInvalidPassword {  get; set; }
        public SignInUser SignInWithInvalidEmailValidPassword {  get; set; }
        public SignInUser SignInWithEmptyFields {  get; set; }
        public SignInUser SignInWithUngeristeredEmail {  get; set; }
    }
    public class SignInUser
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName {  get; set; }
        public string LastName { get; set; }
    }
}
