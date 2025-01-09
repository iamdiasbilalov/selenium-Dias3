using System.Reflection;
using AventStack.ExtentReports;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using Xunit.Abstractions;

namespace SeleniumWebDriverPracticeWithDotNet.AdvancedTests.WaitTests;

public sealed class ImplicitWaitTests : IClassFixture<ExtentReportsFixture>, IDisposable
{
    private readonly ExtentReports _extent;
    private readonly ExtentTest _test;
    private readonly IWebDriver _webDriver;

    public ImplicitWaitTests(ExtentReportsFixture extent, ITestOutputHelper output)
    {
        var type = output.GetType();
        var testMember = type.GetField("test", BindingFlags.Instance | BindingFlags.NonPublic);
        var test = (ITest)testMember?.GetValue(output)!;

        _extent = extent.Extent;
        _test = _extent.CreateTest($"Implicit Wait Tests - {test.TestCase.TestMethod.Method.Name}");

        _webDriver = new FirefoxDriver();
    }

    [Fact]
    public void WaitUntilFirstNameFieldAppears_WhenCreateNewAccountButtonClicked_UsingImplicitWait()
    {
        try
        {
            _webDriver
                .Navigate()
                .GoToUrl("https://www.facebook.com");

            var createNewAccountButton =
                _webDriver.FindElement(By.CssSelector("a[data-testid=\"open-registration-form-button\"]"));

            _webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);

            createNewAccountButton.Click();

            var firstNameField = _webDriver.FindElement(By.CssSelector("input[name=\"firstname\"]"));

            Assert.NotNull(firstNameField);
            _test.Log(Status.Pass, "Implicit wait works as expected. Web driver waits for the first name field appears.");
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