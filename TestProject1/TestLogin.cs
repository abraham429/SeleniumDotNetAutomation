using System;
using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SeleniumTests
{
    [TestFixture]
    public class TestLogin
    {
        private IWebDriver driver;
        private const string LoginUrl = "https://app.ninjarmm.com/auth/#/login";
        private const string ValidEmail1 = "arun.abraham@yahoo.com";
        private const string ValidPassword1 = "Ninja1Test";
        private const string InvalidEmail1 = "InvalidEmail@yahoo.com";
        private const string InvalidPassword1 = "Ninja1Test";



        [SetUp]
        public void Setup()
        {
            /* --Local Driver Enable 
            // Initialize WebDriver before each test
            driver = new ChromeDriver();
	    -- */


            // Get current test name
            string testName = TestContext.CurrentContext.Test.Name;
            string buildNumber = "0.1.0";
            Console.WriteLine("Running test: " + testName + " version " + buildNumber);


            /* --Saucelabs Enabled  */
            var browserOptions = new ChromeOptions();
            browserOptions.PlatformName = "Windows 11";
            browserOptions.BrowserVersion = "latest";
            var sauceOptions = new Dictionary<string, object>();
            sauceOptions.Add("username", "arunabraham1279");
            sauceOptions.Add("accessKey", "eaaec9f1-176c-49f2-bfe7-b17401e584c8");
            sauceOptions.Add("build", buildNumber);
            sauceOptions.Add("name", testName);
            browserOptions.AddAdditionalOption("sauce:options", sauceOptions);

            var uri = new Uri("https://ondemand.us-west-1.saucelabs.com:443/wd/hub");
            driver = new RemoteWebDriver(uri, browserOptions);

            driver.Manage().Window.Maximize();

        }

        [Test]
        public void TestSuccessfulLogin()
        {
            // Navigate to URL
            driver.Navigate().GoToUrl(LoginUrl);

            // Set implicit wait (applies globally to all element searches)
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);

            // Create WebDriverWait instance (max wait time: 10 seconds)
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // Wait until an element is visible and clickable
            // Locate email text box and enter email id
            IWebElement emailTextBox = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("email")));
            emailTextBox.Click();
            emailTextBox.SendKeys(ValidEmail1);


            // Locate password text box and enter password
            IWebElement passwordTextBox = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("password")));
            passwordTextBox.Click();
            passwordTextBox.SendKeys(ValidPassword1);


            // Wait briefly to let results load
            System.Threading.Thread.Sleep(2000);

            IWebElement button = wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("btn-primary")));
            button.Click();

            IWebElement verification = wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("css-12gm3jm")));
            Console.WriteLine("verification text: " + verification.Text);
            Assert.That(verification.Text, Is.EqualTo("Enter verification code"));
        }


        [Test]
        public void TestUnsuccessfulLogin()
        {
            // Navigate to URL
            driver.Navigate().GoToUrl(LoginUrl);

            // Set implicit wait (applies globally to all element searches)
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            // Create WebDriverWait instance (max wait time: 10 seconds)
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // Wait until an element is visible and clickable
            // Locate email text box and enter email id
            IWebElement emailTextBox = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("email")));
            emailTextBox.Click();
            emailTextBox.SendKeys(InvalidEmail1);

            // Locate password text box and enter password
            IWebElement passwordTextBox = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("password")));
            passwordTextBox.Click();
            passwordTextBox.SendKeys(InvalidPassword1);

            IWebElement button = wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("btn-primary")));
            button.Click();

            IWebElement alertTextBox = wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("alert-danger")));
            Console.WriteLine("Alert Text: " + alertTextBox.Text);
            Assert.That(alertTextBox.Text.Contains("contact your system administrator for assistance."));
        }


        [TearDown]
        public void Cleanup()
        {
            // Close browser after each test
            driver.Quit();
        }
    }
}