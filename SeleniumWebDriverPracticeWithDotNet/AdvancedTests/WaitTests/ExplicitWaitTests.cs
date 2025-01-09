using System.Reflection;
using AventStack.ExtentReports;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using Xunit.Abstractions;

namespace SeleniumWebDriverPracticeWithDotNet.AdvancedTests.WaitTests;

public sealed class ExplicitWaitTests : IClassFixture<ExtentReportsFixture>, IDisposable
{
    private readonly ExtentReports _extent;
    private readonly ExtentTest _test;
    private readonly IWebDriver _webDriver;

    public ExplicitWaitTests(ExtentReportsFixture extent, ITestOutputHelper output)
    {
        var type = output.GetType();
        var testMember = type.GetField("test", BindingFlags.Instance | BindingFlags.NonPublic);
        var test = (ITest)testMember?.GetValue(output)!;

        _extent = extent.Extent;
        _test = _extent.CreateTest($"Explicit Wait Tests - {test.TestCase.TestMethod.Method.Name}");

        _webDriver = new FirefoxDriver();
    }

    [Fact]
    public void WaitUntilFirstNameFieldAppears_WhenCreateNewAccountButtonClicked_UsingExplicitWait()
    {
        try
        {
            _webDriver
                .Navigate()
                .GoToUrl("https://www.facebook.com");

            var createNewAccountButton =
                _webDriver.FindElement(By.CssSelector("a[data-testid=\"open-registration-form-button\"]"));

            createNewAccountButton.Click();

            var wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(3));
            var firstNameField = _webDriver.FindElement(By.CssSelector("input[name=\"firstname\"]"));
            wait.Until(_ => firstNameField.Displayed);

            Assert.NotNull(firstNameField);
            _test.Log(Status.Pass, "Explicit wait works as expected. Web driver waits for the first name field to appear.");
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
    }
}