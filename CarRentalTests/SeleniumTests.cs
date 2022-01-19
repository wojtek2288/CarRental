using CarRental.Data;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using OpenQA.Selenium.Interactions;

namespace CarRentalTests
{
    [TestFixture]
    public class SeleniumTests
    {
        DbUtils dbUtils;
        IWebDriver driver;
        string baseUrl;
        int testTimeout = 10;

        string testingAccount = "carrentaldotnettester@gmail.com";
        string testingPassword = "DobreHaslo123";

        private CarRental.POCO.Car testCar;

        [SetUp]
        public void Setup()
        {
            baseUrl = "https://localhost:5001";

            driver = new ChromeDriver();

            dbUtils = new("appsettings.json");

            string modelName = "TestModel123";
            testCar = new CarRental.POCO.Car()
            {
                Model = modelName,
                Brand = "TestBrand",
                Description = "",
                Horsepower = 100,
                YearOfProduction = 2000,
                Id = Guid.NewGuid()
            };
            dbUtils.AddCar(testCar);
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

        private void WaitForXPath(string xpath)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(testTimeout));
            wait.Until((driver) =>
            {
                var element = driver.FindElement(By.XPath(xpath));
                return element.Enabled && element.Displayed;
            });
        }

        private void RegisterAndLogIn()
        {
            // Navigate to base page
            driver.Navigate().GoToUrl(baseUrl);
            IWebElement signInButton = driver.FindElement(By.XPath("//button[contains(.,\"Sign In\")]"));

            // Click sign in button and switch to popup
            string currentHandle = driver.CurrentWindowHandle;
            PopupWindowFinder popupWindowFinder = new(driver);
            string popupWindowHandle = popupWindowFinder.Click(signInButton);
            driver.SwitchTo().Window(popupWindowHandle);

            // Enter login information
            driver.FindElement(By.XPath("//*[@id=\"identifierId\"]")).SendKeys(testingAccount);
            driver.FindElement(By.XPath("//*[@id=\"identifierNext\"]/div/button/span")).Click();

            WaitForXPath("//*[@id=\"password\"]/div[1]/div/div[1]/input");
            driver.FindElement(By.XPath("//*[@id=\"password\"]/div[1]/div/div[1]/input")).SendKeys(testingPassword);
            driver.FindElement(By.XPath("//*[@id=\"passwordNext\"]/div/button/span")).Click();

            driver.SwitchTo().Window(currentHandle);

            // Wait until signed in
            WaitForXPath("//*[@id=\"root\"]/div/h1");

            // Check if in register page
            string text = driver.FindElement(By.XPath("//*[@id=\"root\"]/div/h1")).Text;
            Assert.AreEqual(text, "Please provide additional info to continue");

            // Enter register information
            driver.FindElement(By.Id("licenceDate")).SendKeys("02032010");
            driver.FindElement(By.Id("birthday")).SendKeys("03042000");
            driver.FindElement(By.Id("location")).SendKeys("Warsaw");
            driver.FindElement(By.XPath("//*[@id=\"root\"]/div/form/div/div/div/button")).Click();

            // Wait until list visible
            WaitForXPath("//*[@id=\"root\"]/div[2]/table");
        }

        [Test]
        public void SeleniumTestRegister()
        {
            RegisterAndLogIn();

            // Check if user added
            Assert.True(dbUtils.UserExists(new CarRental.POCO.User {Email = testingAccount}));
        }

        [Test]
        public void RentCarTest()
        {
            RegisterAndLogIn();

            // Click Check Price
            driver.FindElement((By.XPath($"//td[contains(.,\"{testCar.Model}\")]/../td/table/tbody/tr/td/button[2]")))
                .Click();

            // Wait for popup
            WaitForXPath("//*[@id=\"from\"]");

            // Check price
            DateTime fromDate = DateTime.Today.AddDays(1);
            DateTime toDate = DateTime.Today.AddDays(3);

            new Actions(driver).MoveToElement(driver.FindElement(By.Id("from"))).Click()
                .SendKeys(fromDate.ToShortDateString())
                .Perform();
            new Actions(driver).MoveToElement(driver.FindElement(By.Id("to"))).Click()
                .SendKeys(toDate.ToShortDateString())
                .Perform();
            new Actions(driver).MoveToElement(driver.FindElement(By.XPath("//input[@id=\"from\"]/../button"))).Click()
                .Perform();

            WaitForXPath("//input[@id=\"from\"]/../button");
            // Rent car
            new Actions(driver).MoveToElement(driver.FindElement(By.XPath("//input[@id=\"from\"]/../button"))).Click()
                            .Perform();

            WaitForXPath("//*[contains(.,\"Successfully Rented Car\")]");
            Assert.True((from rental in dbUtils.GetRentals()
                where rental.CarId == testCar.Id
                select rental).Any());
        }

        [TearDown]
        public void Teardown()
        {
            driver.Quit();
            CarRental.POCO.User user = dbUtils.FindUserByEmail(testingAccount);
            if (user != null) dbUtils.RemoveUser(user);
            dbUtils.RemoveCar(testCar);
        }
    }
}