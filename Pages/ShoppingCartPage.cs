﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace Practice_Basics_of_Playwright.Pages
{
    public class ShoppingCartPage
    {
        private readonly IPage page;

        public ShoppingCartPage(IPage page)
        {
            this.page = page;
        }
    }
}
