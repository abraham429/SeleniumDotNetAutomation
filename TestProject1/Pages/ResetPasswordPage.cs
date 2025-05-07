using NUnit.Framework;
using OpenQA.Selenium;

namespace SeleniumTests.Pages
{
    public class ResetPasswordPage
    {
        private IWebDriver driver;

        // Locators
        private IWebElement SendButton => driver.FindElement(By.CssSelector("button.btn.btn-primary.m-t-sm"));

        // Constructor to initialize WebDriver
        public ResetPasswordPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void VerifyPageLoaded()
        {
            Assert.That(SendButton.Text, Is.EqualTo("Send"), "Button label does not match expected value.");
        }
    }
}
