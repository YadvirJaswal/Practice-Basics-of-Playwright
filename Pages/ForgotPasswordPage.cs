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
        private ILocator signInNavigationButton;
        private ILocator forgotPasswordLink;


        public ForgotPasswordPage(IPage page)
        {
            this.page = page;
            signInNavigationButton = page.Locator(".panel.header > ul > li:nth-child(2) > a");
            forgotPasswordLink = page.Locator(".action.remind");
        }
    }
}
