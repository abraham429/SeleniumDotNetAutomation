using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace SeleniumTests.Pages
{
    public class MfaPage
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        // Locators
        private IWebElement VerificationElement() => wait.Until(ExpectedConditions.ElementExists(By.CssSelector(".css-12gm3jm.esayeox0.control-label")));


        // Constructor to initialize WebDriver
        public MfaPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);

        }

        public void VerifyPageLoaded()
        {
            // Verify the text
            Assert.That(VerificationElement().Text, Is.EqualTo("Enter verification code"));
        }
    }
}
