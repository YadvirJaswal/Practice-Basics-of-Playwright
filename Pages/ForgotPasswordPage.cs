using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace Practice_Basics_of_Playwright.Pages
{
    public class ForgotPasswordPage
    {
        private readonly IPage page;
        private ILocator emailInput;


        public ForgotPasswordPage(IPage page)
        {
            this.page = page;
            emailInput = page.Locator("#email_address");
        }
        public async Task<bool> IsEmailInputFieldVisibleAsync()
        {
            var isEmailInputfieldVisible = await emailInput.IsVisibleAsync();
            return isEmailInputfieldVisible;
        }
    }
}
