using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace NUnitTestProject1
{
    public class Tests
    {
        private IWebDriver driver;
        private static string _department = "Research & Development";
        private static string[] _languages = new string[] { "English", "French" };
        private static int _expectedJobsQty = 12;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://cz.careers.veeam.com/vacancies");

            //Maximize window
            driver.Manage().Window.Maximize();
        }

        [Test]
        public void Test1()
        {
            var searchFields = driver.FindElements(By.Id("sl"));

            //Select Department
            searchFields[0].Click();
            driver.FindElement(By.LinkText(_department)).Click();

            //Select necessary languages
            searchFields[1].Click();
            var languages = driver.FindElements(By.ClassName("custom-checkbox"));
            foreach (var language in languages)
            {
                if (_languages.Any(x => x == language.Text))
                {
                    language.Click();
                }
            }

            //Check number of vacancies
            var vacancies = driver.FindElements(By.ClassName("card-no-hover"));

            if (_expectedJobsQty == vacancies.Count)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        [TearDown]
        public void CleanUp()
        {
            // Terminates the remote webdriver session
            driver.Quit();
        }
    }
}