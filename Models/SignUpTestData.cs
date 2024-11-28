using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_Basics_of_Playwright.Models
{
    public class SignUpTestData
    {
        public SignUpUser ValidSignUpUser { get; set; }
        public SignUpUser InvalidSignUpUser { get; set; }
      
    }
    public class SignUpUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
