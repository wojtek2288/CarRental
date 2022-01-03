using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using System.Threading;

namespace CarRentalTests
{
    [TestFixture]
    public class Tests
    {
        IWebDriver driver;
        string baseUrl;

        [SetUp]
        public void Setup()
        {
            baseUrl = "https://localhost:5001";

            driver = new ChromeDriver();
        }

        [Test]
        public void SeleniumTestPageLoading()
        {
            driver.Navigate().GoToUrl(baseUrl);
            try
            {
                driver.FindElement(By.Id("root"));
                Assert.Pass();
            }
            catch (NoSuchElementException)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void SeleniumTestRegister()
        {
            // Navigate to base page
            driver.Navigate().GoToUrl(baseUrl);
            IWebElement signInButton = driver.FindElement(By.XPath("//*[@id=\"root\"]/header/nav/div/div/ul/button"));

            // Click sign in button and switch to popup
            string currentHandle = driver.CurrentWindowHandle;
            PopupWindowFinder popupWindowFinder = new(driver);
            string popupWindowHandle = popupWindowFinder.Click(signInButton);
            driver.SwitchTo().Window(popupWindowHandle);

            // Enter login information
            driver.FindElement(By.XPath("//*[@id=\"identifierId\"]")).SendKeys("carrentaldotnettester@gmail.com");
            driver.FindElement(By.XPath("//*[@id=\"identifierNext\"]/div/button/span")).Click();

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until((driver) =>
            {
                driver.FindElement(By.XPath("//*[@id=\"password\"]/div[1]/div/div[1]/input"));
                return true;
            });

            driver.FindElement(By.XPath("//*[@id=\"password\"]/div[1]/div/div[1]/input")).SendKeys("DobreHaslo123");
            driver.FindElement(By.XPath("//*[@id=\"passwordNext\"]/div/button/span")).Click();

            driver.SwitchTo().Window(currentHandle);

            // Wait until signed in
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until((driver) =>
            {
                driver.FindElement(By.XPath("//*[@id=\"root\"]/div/h1"));
                return true;
            });

            // Check if in register page
            string text = driver.FindElement(By.XPath("//*[@id=\"root\"]/div/h1")).Text;
            Assert.AreEqual(text, "Please provide additional info to continue");

            // Enter register information
            driver.FindElement(By.Id("licenceDate")).SendKeys("02032010");
            driver.FindElement(By.Id("birthday")).SendKeys("03042000");
            driver.FindElement(By.Id("location")).SendKeys("Warsaw");
            driver.FindElement(By.XPath("//*[@id=\"root\"]/div/form/div/div/div/button")).Click();

            // Go to viewcars
            driver.Navigate().GoToUrl(baseUrl + "/viewcars");

            // Check if list visible
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until((driver) =>
            {
                driver.FindElement(By.XPath("//*[@id=\"root\"]/div[2]/table"));
                return true;
            });

            Assert.Pass();
        }

        [TearDown]
        public void Teardown()
        {
            driver.Quit();
        }

    }
}