using System.Reflection;
using AventStack.ExtentReports;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using Xunit.Abstractions;

namespace SeleniumWebDriverPracticeWithDotNet.BasicTests;

public sealed class SearchFunctionalityTests : IClassFixture<ExtentReportsFixture>, IDisposable
{
    private readonly ExtentReports _extent;
    private readonly ExtentTest _test;
    private readonly IWebDriver _webDriver;

    public SearchFunctionalityTests(ExtentReportsFixture extent, ITestOutputHelper output)
    {
        var type = output.GetType();
        var testMember = type.GetField("test", BindingFlags.Instance | BindingFlags.NonPublic);
        var test = (ITest)testMember?.GetValue(output)!;

        _extent = extent.Extent;
        _test = _extent.CreateTest($"Search Functionality Tests - {test.TestCase.TestMethod.Method.Name}");
        _webDriver = new FirefoxDriver();
    }

    [Fact]
    public void SearchTest_ByUserImitation_UsingCssSelector()
    {
        try
        {
            // Arrange
            _webDriver
                .Navigate()
                .GoToUrl("https://www.google.com");

            var searchBar = _webDriver.FindElement(By.CssSelector("textarea#APjFqb"));

            // Act
            searchBar.SendKeys("Quality Assurance");
            searchBar.SendKeys(Keys.Enter);

            var wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(3));
            wait.Until(driver => driver.Title.Contains("Quality Assurance"));

            //Assert
            Assert.Contains("Quality Assurance", _webDriver.Title);
            _test.Log(Status.Pass, $"Test assertion passed. Page title contains {_webDriver.Title}");
        }
        catch (Exception e)
        {
            _test.Log(Status.Fail, "Test Failed. See below for details.");
            _test.Log(Status.Error, e.Message);
        }
    }

    [Fact]
    public void SearchTest_ByUserImitation_UsingXPath()
    {
        try
        {
            // Arrange
            _webDriver
                .Navigate()
                .GoToUrl("https://www.google.com");

            var searchBar = _webDriver.FindElement(By.XPath("//textarea[@id='APjFqb']"));

            // Act
            searchBar.SendKeys("Quality Assurance");
            searchBar.SendKeys(Keys.Enter);

            var wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(3));
            wait.Until(driver => driver.Title.Contains("Quality Assurance"));

            // Assert
            Assert.Contains("Quality Assurance", _webDriver.Title);
            _test.Log(Status.Pass, $"Test assertion passed. Page title contains {_webDriver.Title}");
        }
        catch (Exception e)
        {
            _test.Log(Status.Fail, "Test Failed. See below for details.");
            _test.Log(Status.Error, e.Message);
        }
    }

    [Fact]
    public void SearchTest_ByChangingUrlContents()
    {
        try
        {
            // Arrange
            _webDriver
                .Navigate()
                .GoToUrl("https://www.google.com");

            const string topicToSearch = "Quality+Assurance";

            // Act
            _webDriver.Url = _webDriver.Url + "/search?q=" + topicToSearch;

            var wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(3));
            wait.Until(driver => driver.Title.Contains("Quality Assurance"));

            //Assert
            Assert.Contains("Quality Assurance", _webDriver.Title);
            Assert.Contains("Quality+Assurance", _webDriver.Url);
            _test.Log(Status.Pass, "Test assertion passed. Page title contains " + topicToSearch); 
        }
        catch (Exception e)
        {
            _test.Log(Status.Fail, "Test Failed. See below for details.");
            _test.Log(Status.Error, e.Message);
        }
    }

    public void Dispose()
    {
        _webDriver.Quit();
        _webDriver.Dispose();
        ExtentReportsHelper.FinalizeReport();
    }
}