using CarRental.Data;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using CarRental.POCO;
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
        string testingAuthID = "110551637047613637127";
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

        private void LogIn()
        {
            // Navigate to base page
            driver.Navigate().GoToUrl(baseUrl);
            string loginUrl = driver.Url;
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
            while (driver.Url == loginUrl)
            {
                Thread.Sleep(50);
            }
        }

        private void RegisterAndLogIn()
        {
            LogIn();

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

        [Test]
        public void AddCarTest()
        {
            User testingUser = new User()
            {
                Id = Guid.NewGuid(),
                Email = testingAccount,
                Location = "Atlantis",
                DateOfBirth = new DateTime(2000, 1, 1),
                DriversLicenseDate = new DateTime(2018, 1, 1),
                AuthID = testingAuthID,
                Role = User.UserRole.ADMINISTRATOR
            };
            dbUtils.AddUser(testingUser);

            string testBrand = "Test Brand";
            string testModel = "Test Model";
            string testYear = "2000";
            string testHorsepower = "100";
            string testDescription = "Test Description";

            try
            {
                LogIn();

                driver.Navigate().GoToUrl(baseUrl + "/addcar");

                WaitForXPath("//*[@id=\"car-brand\"]");

                driver.FindElement(By.Id("car-brand")).SendKeys(testBrand);
                driver.FindElement(By.Id("car-model")).SendKeys(testModel);
                new SelectElement(driver.FindElement(By.Id("year-of-production"))).SelectByText(testYear);
                driver.FindElement(By.Id("horse-power")).SendKeys(testHorsepower);
                driver.FindElement(By.Id("description")).SendKeys(testDescription);
                driver.FindElement(By.XPath("//button[@type=\"submit\"]")).Click();
                WaitForXPath("//div[contains(.,\"Successfully Added Car\")]");
            }
            catch (Exception)
            {
                dbUtils.RemoveUser(testingUser);
                Assert.Fail();
            }

            Car c = null;
            foreach (Car car in dbUtils.GetNewCars())
            {
                if (car.Brand == testBrand &&
                    car.Model == testModel &&
                    car.YearOfProduction.ToString() == testYear &&
                    car.Horsepower.ToString() == testHorsepower &&
                    car.Description == testDescription)
                {
                    c = car;
                    break;
                }
            }

            if (c != null)
            {
                dbUtils.RemoveCar(c);
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
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