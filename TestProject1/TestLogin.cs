using System;
using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using SeleniumTests.Pages;

namespace SeleniumTests
{
    [TestFixture]
    public class TestLogin
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        //Page Objects
        private LoginPage loginPage;
        private MfaPage mfaPage;
        private ResetPasswordPage resetPasswordPage;
        private RegisterAccountPage registerAccountPage;

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
            string buildNumber = "2.0.0";
            Console.WriteLine("Running test: " + testName + " version " + buildNumber);

            // Note: instead of using SauceLabs, for local runs, we can use:
            // driver = new ChromeDriver();

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

            // Create Page Objects
            loginPage = new LoginPage(driver);
            mfaPage = new MfaPage(driver);
            resetPasswordPage = new ResetPasswordPage(driver);
            registerAccountPage = new RegisterAccountPage(driver);
        }

        [Test]
        public void TestSuccessfulLogin()
        {
            loginPage.EnterCredentialsAndClickSignInButton(ValidEmail1, ValidPassword1);
            mfaPage.VerifyPageLoaded();
        }

        [Test]
        public void TestLoginViaTabbing()
        {
            loginPage.LoginViaTabKey(ValidEmail1, ValidPassword1);
            mfaPage.VerifyPageLoaded();
        }

        [Test]
        public void TestLoginPageUiComponents()
        {
            loginPage.VerifyUiComponents();
        }

        [Test]
        public void TestLoginWithNoEntriesWithErrorPopupAlert()
        {
            loginPage.ClickSigninButton();
            loginPage.VerifyLoginErrorAlertPopup();
        }

        [Test]
        public void TestUnsuccessfulLogin()
        {
            loginPage.EnterCredentialsAndClickSignInButton(InvalidEmail1, InvalidPassword1);
            loginPage.VerifyLoginErrorWithinModal();
        }
        
        [Test]
        public void TestDoNotHaveAnAccountLink()
        {
            loginPage.ClickOnLink("Do not have an account?");
            registerAccountPage.VerifyPageLoaded();
        }

        [Test]
        public void TestForgotPasswordLink()
        {
            loginPage.ClickOnLink("Forgot your password?");
            resetPasswordPage.VerifyPageLoaded();
        }

        [TearDown]
        public void Cleanup()
        {
            // Close browser after each test
            driver.Quit();
        }
    }
}