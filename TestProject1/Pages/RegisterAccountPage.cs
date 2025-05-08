using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace SeleniumTests.Pages
{
    public class RegisterAccountPage
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        // Locators 
        private IWebElement RegisterButton() => wait.Until(ExpectedConditions.ElementExists(By.CssSelector("button.btn.btn-primary.m-t-sm")));

        // Constructor to initialize WebDriver
        public RegisterAccountPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public void VerifyPageLoaded()
        {
            Assert.That(RegisterButton().Text, Is.EqualTo("Register"));
        }
    }
}
