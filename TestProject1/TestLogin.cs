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
        private WebDriverWait wait;
        private const string LoginUrl = "https://app.ninjarmm.com/auth/#/login";
        private const string ValidEmail1 = "arun.abraham@yahoo.com";
        private const string ValidPassword1 = "Ninja1Test";
        private const string InvalidEmail1 = "' UNION SELECT null, null, null --";
        private const string InvalidPassword1 = "' OR '1'='1'; --";


        [SetUp]
        public void Setup()
        {
             // Get current test name
            string testName = TestContext.CurrentContext.Test.Name;
            string buildNumber = "1.0.0";
            Console.WriteLine("Running test: " + testName + " version " + buildNumber);

            // Note: instead of using SauceLabs, for local runs, we can use:
            //       driver = new ChromeDriver();

            /* --Using Saucelabs -- */
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
            

            // Set implicit wait (applies globally to all element searches)
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);

            driver.Manage().Window.Maximize();

            // Create WebDriverWait instance (max wait time: 10 seconds)
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // Navigate to URL
            driver.Navigate().GoToUrl(LoginUrl);
        }

        [Test]
        public void TestSuccessfulLogin()
        {
            EnterCredentialsAndClickSignInButton(ValidEmail1, ValidPassword1);

            // confirm that after login, the verification page appears
            IWebElement verification = wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("css-12gm3jm")));
            Console.WriteLine("verification text: " + verification.Text);
            Assert.That(verification.Text, Is.EqualTo("Enter verification code"));
        }

        [Test]
        public void TestLoginViaTabbing()
        {
           // Wait until an element is visible and clickable
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("email")));

            // Send TAB key to move to username field
            IWebElement bodyElement = driver.FindElement(By.TagName("body")); // Focus on page
            bodyElement.SendKeys(Keys.Tab);
            bodyElement.SendKeys(Keys.Tab);

            // Locate the username field and enter credentials
            IWebElement usernameField = driver.SwitchTo().ActiveElement(); // After tabbing, focus should be on username field
            usernameField.SendKeys(ValidEmail1);

            // Locate the password field and enter credentials
            bodyElement.SendKeys(Keys.Tab);
            IWebElement passwordField = driver.SwitchTo().ActiveElement(); // After tabbing, focus should be on password field
            passwordField.SendKeys(ValidPassword1);

            // Wait briefly to let results load
            System.Threading.Thread.Sleep(500);

            // Press Enter to submit
            passwordField.SendKeys(Keys.Enter);

            // confirm that after login, the verification page appears
            IWebElement verification = wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("css-12gm3jm")));
            Console.WriteLine("verification text: " + verification.Text);
            Assert.That(verification.Text, Is.EqualTo("Enter verification code"));
        }

        [Test]
        public void TestLoginPageUiComponents()
        {
            // Locate Email label & confirm the text
            IWebElement emailLabel = driver.FindElement(By.XPath("//label[@for='email']"));
            Assert.That(emailLabel.Text, Is.EqualTo("Email"));

            // Locate password label & confirm the text
            IWebElement passwordLabel = driver.FindElement(By.XPath("//label[@for='password']"));
            Assert.That(passwordLabel.Text, Is.EqualTo("Password"));

            // Locate staySignedIn label & confirm the text
            IWebElement staySignedInLabel = driver.FindElement(By.ClassName("css-ifz9uv"));
            Assert.That(staySignedInLabel.Text, Is.EqualTo("Keep me signed in"));

            // Locate checkbox for staySignedIn & confirm that it can be toggled
            IWebElement staySignedInCheckbox = driver.FindElement(By.Id("staySignedIn"));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            bool isChecked = (bool)js.ExecuteScript("return arguments[0].checked;", staySignedInCheckbox);
            Assert.That(isChecked, Is.False);
            js.ExecuteScript("arguments[0].scrollIntoView(true);", staySignedInCheckbox);
            js.ExecuteScript("arguments[0].click();", staySignedInCheckbox);
            isChecked = (bool)js.ExecuteScript("return arguments[0].checked;", staySignedInCheckbox);
            Assert.That(isChecked, Is.True);
        }

        [Test]
        public void TestLoginWithNoEntriesWithErrorPopupAlert()
        {
            IWebElement submitButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("btn-primary")));
            Assert.That(submitButton.Text, Is.EqualTo("Sign in"), "Button label does not match expected value.");
            submitButton.Click();

            // Wait briefly to let results load
            System.Threading.Thread.Sleep(500);

            IWebElement alertPopup = driver.FindElement(By.ClassName("css-ab4h3z"));
            Console.WriteLine("Alert Text: " + alertPopup.Text);
            Assert.That(alertPopup.Text.Contains("Error during login"));
        }

        [Test]
        public void TestUnsuccessfulLogin()
        {
            EnterCredentialsAndClickSignInButton(InvalidEmail1, InvalidPassword1);

            IWebElement alertTextBox = wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("alert-danger")));
            Console.WriteLine("Alert Text: " + alertTextBox.Text);
            Assert.That(alertTextBox.Text.Contains("contact your system administrator for assistance."));
        }
        
        [Test]
        public void TestDoNotHaveAnAccountLink()
        {
            // Locate & click on "Do not have an account?" link
            IWebElement dontHaveAccountLink = wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Do not have an account?")));
            dontHaveAccountLink.Click();

            // Assert that the register button is presented
            IWebElement buttonToRegister = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("button.btn.btn-primary.m-t-sm")));
            Assert.That(buttonToRegister.Text, Is.EqualTo("Register"), "Button label does not match expected value.");
        }

        [Test]
        public void TestForgotPasswordLink()
        {
            // Locate & click on "Forgot your password?" link
            IWebElement forgotPwdLink = wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Forgot your password?")));
            forgotPwdLink.Click();

            // Assert that the register button is presented
            IWebElement buttonToSend = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("button.btn.btn-primary.m-t-sm")));
            Assert.That(buttonToSend.Text, Is.EqualTo("Send"), "Button label does not match expected value.");
        }

        [TearDown]
        public void Cleanup()
        {
            // Close browser after each test
            driver.Quit();
        }

        private void EnterCredentialsAndClickSignInButton(string theEmail, string thePassword)
        {
            // Wait until an element is visible and clickable
            // Locate email text box and enter email id
            IWebElement emailTextBox = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("email")));
            emailTextBox.Click();
            emailTextBox.SendKeys(theEmail);

            // Locate password text box and enter password
            IWebElement passwordTextBox = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("password")));
            passwordTextBox.Click();
            passwordTextBox.SendKeys(thePassword);

            IWebElement submitButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("btn-primary")));
            Assert.That(submitButton.Text, Is.EqualTo("Sign in"), "Button label does not match expected value.");
            submitButton.Click();
        }

    }
}