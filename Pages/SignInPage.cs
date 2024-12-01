using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace Practice_Basics_of_Playwright.Pages
{
    public class SignInPage
    {
        private readonly IPage page;
       

        public SignInPage(IPage page, AppSettings appSettings)
        {
            this.page = page;
            
        }
    }
}
