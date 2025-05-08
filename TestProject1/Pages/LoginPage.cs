using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SeleniumTests.Pages
{
    public class LoginPage
    {
        private IWebDriver driver;
        private WebDriverWait wait;


        // Locators
        private IWebElement BodyElement() =>  wait.Until(ExpectedConditions.ElementExists((By.TagName("body"))));
        private IWebElement AlertPopup()  =>  wait.Until(ExpectedConditions.ElementExists(By.ClassName("css-ab4h3z")));
        private IWebElement AlertTextBox() => wait.Until(ExpectedConditions.ElementExists(By.ClassName("alert-danger")));
        private IWebElement EmailTextBox() => wait.Until(ExpectedConditions.ElementExists(By.Id("email")));
        private IWebElement PasswordTextBox() => wait.Until(ExpectedConditions.ElementExists(By.Id("password")));
        private IWebElement SignInButton() => wait.Until(ExpectedConditions.ElementExists(By.ClassName("btn-primary")));
        private IWebElement EmailLabel() => wait.Until(ExpectedConditions.ElementExists(By.CssSelector("label[for='email']")));
        private IWebElement PasswordLabel() => wait.Until(ExpectedConditions.ElementExists(By.CssSelector("label[for='password']")));
        private IWebElement StaySignedInCheckbox() => wait.Until(ExpectedConditions.ElementExists(By.Id("staySignedIn")));



        // Constructor to initialize WebDriver
        public LoginPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public void EnterEmail(string theEmail)
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(EmailTextBox()));
            EmailTextBox().Click();
            EmailTextBox().SendKeys(theEmail);
        }

        public void Clear()
        {
            // Wait until EmailTextBox is clickable
            wait.Until(ExpectedConditions.ElementToBeClickable(EmailTextBox()));
            EmailTextBox().Click();
            EmailTextBox().SendKeys(Keys.Control + "a"); // Select all text
            EmailTextBox().SendKeys(Keys.Backspace); // Delete selected text

            // Wait until PasswordTextBox is clickable
            wait.Until(ExpectedConditions.ElementToBeClickable(PasswordTextBox()));
            PasswordTextBox().Click();
            PasswordTextBox().SendKeys(Keys.Control + "a"); // Select all text
            PasswordTextBox().SendKeys(Keys.Backspace); // Delete selected text
        }

        public void EnterPassword(string thePassword)
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(PasswordTextBox()));
            PasswordTextBox().Click();
            PasswordTextBox().SendKeys(thePassword);
        }

        public void ClickSigninButton()
        {
            //Wait until Signin button is clickable
            System.Threading.Thread.Sleep(500);
            wait.Until(ExpectedConditions.ElementToBeClickable(SignInButton()));

            Assert.That(SignInButton().Text, Is.EqualTo("Sign in"), "Button label does not match expected value.");
            SignInButton().Click();
        }

        public void EnterCredentialsAndClickSignInButton(string theEmail, string thePassword)
        {
            EnterEmail(theEmail);
            EnterPassword(thePassword);
            ClickSigninButton();
        }

        public void VerifyLoginErrorAlertPopup()
        {
            // Wait until an element is visible and clickable
            wait.Until(ExpectedConditions.ElementToBeClickable(AlertPopup()));
            Console.WriteLine("Alert Text: " + AlertPopup().Text);
            Assert.That(AlertPopup().Text.Contains("Error during login"));
        }

        public void VerifyLoginErrorWithinModal()
        {
            Console.WriteLine("Alert Text: " + AlertTextBox().Text);
            Assert.That(AlertTextBox().Text.Contains("contact your system administrator for assistance."));
        }

        public void VerifyUiComponents()
        {
            // confirm Email label text
            Assert.That(EmailLabel().Text, Is.EqualTo("Email"));

            // confirm Password label text
            Assert.That(PasswordLabel().Text, Is.EqualTo("Password"));

            // confirm staySignedIn label text
            IWebElement staySignedInLabel = driver.FindElement(By.ClassName("css-ifz9uv"));
            Assert.That(staySignedInLabel.Text, Is.EqualTo("Keep me signed in"));

            // confirm staySignedIn checkbox is disabled by default and it can be enabled
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            bool isChecked = (bool)js.ExecuteScript("return arguments[0].checked;", StaySignedInCheckbox());
            Assert.That(isChecked, Is.False);
            js.ExecuteScript("arguments[0].scrollIntoView(true);", StaySignedInCheckbox());
            js.ExecuteScript("arguments[0].click();", StaySignedInCheckbox());
            isChecked = (bool)js.ExecuteScript("return arguments[0].checked;", StaySignedInCheckbox());
            Assert.That(isChecked, Is.True);
        }

        public void ClickOnLink(string theLinkText)
        {
            // Locate & click on "Do not have an account?" link
            IWebElement link = wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText(theLinkText)));
            link.Click();
        }

        public void LoginViaTabKey(string theEmail, string thePassword)
        {
            // Wait until an element is visible and clickable
            wait.Until(ExpectedConditions.ElementToBeClickable(EmailTextBox()));

            // Send TAB key to move to username field
            BodyElement().SendKeys(Keys.Tab);
            BodyElement().SendKeys(Keys.Tab);

            // Locate the username field and enter credentials
            IWebElement emailField = driver.SwitchTo().ActiveElement(); // After tabbing, focus should be on Email field
            emailField.SendKeys(theEmail);

            // Locate the password field and enter credentials
            BodyElement().SendKeys(Keys.Tab);
            IWebElement passwordField = driver.SwitchTo().ActiveElement(); // After tabbing, focus should be on password field
            passwordField.SendKeys(thePassword);

            System.Threading.Thread.Sleep(500);

            // Press Enter to submit
            passwordField.SendKeys(Keys.Enter);
        }
    }
}
