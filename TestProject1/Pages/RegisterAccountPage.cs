using NUnit.Framework;
using OpenQA.Selenium;

namespace SeleniumTests.Pages
{
    public class RegisterAccountPage
    {
        private IWebDriver driver;

        // Locators
        private IWebElement RegisterButton => driver.FindElement(By.CssSelector("button.btn.btn-primary.m-t-sm"));

        // Constructor to initialize WebDriver
        public RegisterAccountPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void VerifyPageLoaded()
        {
            Assert.That(RegisterButton.Text, Is.EqualTo("Register"));
        }
    }
}
