using NUnit.Framework;
using OpenQA.Selenium;

namespace SeleniumTests.Pages
{
    public class MfaPage
    {
        private IWebDriver driver;
     
        // Locators
        private IWebElement VerificationCodeElement => driver.FindElement(By.ClassName("css-12gm3jm"));

        // Constructor to initialize WebDriver
        public MfaPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void VerifyPageLoaded()
        {
            Assert.That(VerificationCodeElement.Text, Is.EqualTo("Enter verification code"));
        }
    }
}
