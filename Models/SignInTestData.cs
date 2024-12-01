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
    }
    public class SignInUser
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
