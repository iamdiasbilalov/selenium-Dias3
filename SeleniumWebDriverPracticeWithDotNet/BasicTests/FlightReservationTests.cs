using System.Reflection;
using AventStack.ExtentReports;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using Xunit.Abstractions;

namespace SeleniumWebDriverPracticeWithDotNet.BasicTests;

public sealed class FlightReservationTests : IClassFixture<ExtentReportsFixture>, IDisposable
{
    private readonly ExtentReports _extent;
    private readonly ExtentTest _test;
    private readonly IWebDriver _webDriver;

    public FlightReservationTests(ExtentReportsFixture extent, ITestOutputHelper output)
    {
        var type = output.GetType();
        var testMember = type.GetField("test", BindingFlags.Instance | BindingFlags.NonPublic);
        var test = (ITest)testMember?.GetValue(output)!;

        _extent = extent.Extent;
        _test = _extent.CreateTest($"Flight Reservation Tests - {test.TestCase.TestMethod.Method.Name}");
        var options = new FirefoxOptions();
        _webDriver = new FirefoxDriver(options);
    }

    [Fact]
    public async Task TryToMakeFlightReservation_UsingCssSelectors()
    {
        try
        {
            // Arrange
            await _webDriver
                .Navigate()
                .GoToUrlAsync("https://aviata.kz");

            var fromWhere = _webDriver.FindElement(By.CssSelector("input[placeholder='Откуда']"));
            var whereTo = _webDriver.FindElement(By.CssSelector("input[placeholder='Куда']"));
            var searchButton = _webDriver.FindElement(By.CssSelector("button.search-form__btn"));

            // Act
            fromWhere.Click();
            fromWhere.SendKeys("Астана");

            await Task.Delay(3000);
            var suggestion1 = _webDriver.FindElement(By.CssSelector(".border-b"));
            suggestion1.Click();
            whereTo.Click();
            whereTo.SendKeys("Алматы");
            await Task.Delay(3000);
            var suggestion2 = _webDriver.FindElement(By.CssSelector(".border-b"));
            suggestion2.Click();

            searchButton.Click();
            await Task.Delay(3000);
            var chooseButtons = _webDriver.FindElements(By.CssSelector("button.search-form__btn"));
            
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => chooseButtons[0].Click());
            _test.Log(Status.Pass, "Test passed. Website expectedly detected us as a bot");
        }
        catch (Exception e)
        {
            _test.Log(Status.Fail, "Test failed. See below for error details");
            _test.Log(Status.Error, e.Message);
        }
    }

    [Fact]
    public async Task TryToMakeFlightReservation_UsingXPath()
    {
        try
        {
            // Arrange
            await _webDriver
                .Navigate()
                .GoToUrlAsync("https://aviata.kz");

            var fromWhere = _webDriver.FindElement(By.XPath("//input[@placeholder='Откуда']"));
            var whereTo = _webDriver.FindElement(By.XPath("//input[@placeholder='Куда']"));
            var searchButton = _webDriver.FindElement(By.XPath("//button[contains(@class, 'search-form__btn')]"));

            // Act
            fromWhere.Click();
            fromWhere.SendKeys("Астана");

            await Task.Delay(2000);
            var suggestion1 = _webDriver.FindElement(By.XPath("//div[contains(@class, 'border-b')]"));
            suggestion1.Click();
            whereTo.Click();
            whereTo.SendKeys("Алматы");
            await Task.Delay(2000);
            var suggestion2 = _webDriver.FindElement(By.XPath("//div[contains(@class, 'border-b')]"));
            suggestion2.Click();

            searchButton.Click();
            await Task.Delay(3000);
            var chooseButtons = _webDriver.FindElements(By.XPath("//button[contains(@class, 'search-form__btn')]"));

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => chooseButtons[0].Click());
            _test.Log(Status.Pass, "Test passed. Website expectedly detected us as a bot");
        }
        catch (Exception e)
        {
            _test.Log(Status.Fail, "Test failed. See below for error details");
            _test.Log(Status.Error, e.Message);
        }
    }
    
    public void Dispose()
    {
        _webDriver.Quit();
        _webDriver.Dispose();
    }
}