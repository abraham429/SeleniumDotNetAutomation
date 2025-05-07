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
//        private string "btn-primary"
        private IWebElement BodyElement => driver.FindElement(By.TagName("body"));
        private IWebElement AlertPopup  => driver.FindElement(By.ClassName("css-ab4h3z"));
        private IWebElement AlertTextBox => driver.FindElement(By.ClassName("alert-danger"));
        private IWebElement EmailTextBox => driver.FindElement(By.Id("email"));
        private IWebElement PasswordTextBox => driver.FindElement(By.Id("password"));
        private IWebElement SignInButton => driver.FindElement(By.ClassName("btn-primary"));

        // Constructor to initialize WebDriver
        public LoginPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public void EnterEmail(string theEmail)
        {
            EmailTextBox.Click();
            EmailTextBox.SendKeys(theEmail);
        }

        public void EnterPassword(string thePassword)
        {
            PasswordTextBox.Click();
            PasswordTextBox.SendKeys(thePassword);
        }

        public void ClickSigninButton()
        {
            Assert.That(SignInButton.Text, Is.EqualTo("Sign in"), "Button label does not match expected value.");
            SignInButton.Click();
        }

        public void EnterCredentialsAndClickSignInButton(string theEmail, string thePassword)
        {
            EnterEmail(theEmail);
            EnterPassword(thePassword);
            ClickSigninButton();
        }

        public void VerifyLoginErrorAlertPopup()
        {
            // Initialize WebDriverWait
            // Wait until an element is visible and clickable
            wait.Until(ExpectedConditions.ElementToBeClickable(EmailTextBox));
            wait.Until(ExpectedConditions.ElementToBeClickable(AlertPopup));
            //            wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName(AlertPopupClassName)));
            Console.WriteLine("Alert Text: " + AlertPopup.Text);
            Assert.That(AlertPopup.Text.Contains("Error during login"));
        }

        public void VerifyLoginErrorWithinModal()
        {
            Console.WriteLine("Alert Text: " + AlertTextBox.Text);
            Assert.That(AlertTextBox.Text.Contains("contact your system administrator for assistance."));
        }

        public void VerifyUiComponents()
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

        public void ClickOnLink(string theLinkText)
        {
            // Locate & click on "Do not have an account?" link
            IWebElement link = wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText(theLinkText)));
            link.Click();
        }

        public void LoginViaTabKey(string theEmail, string thePassword)
        {
            // Wait until an element is visible and clickable
            wait.Until(ExpectedConditions.ElementToBeClickable(EmailTextBox));

            // Send TAB key to move to username field
            BodyElement.SendKeys(Keys.Tab);
            BodyElement.SendKeys(Keys.Tab);

            // Locate the username field and enter credentials
            IWebElement emailField = driver.SwitchTo().ActiveElement(); // After tabbing, focus should be on Email field
            emailField.SendKeys(theEmail);

            // Locate the password field and enter credentials
            BodyElement.SendKeys(Keys.Tab);
            IWebElement passwordField = driver.SwitchTo().ActiveElement(); // After tabbing, focus should be on password field
            passwordField.SendKeys(thePassword);

            // Press Enter to submit
            passwordField.SendKeys(Keys.Enter);
        }
    }
}
