using System.Reflection;
using AventStack.ExtentReports;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using Xunit.Abstractions;

namespace SeleniumWebDriverPracticeWithDotNet.AdvancedTests;

public sealed class SelectTests : IClassFixture<ExtentReportsFixture>, IDisposable
{
    private readonly ExtentReports _extent;
    private readonly ExtentTest _test;
    private readonly IWebDriver _webDriver;

    public SelectTests(ExtentReportsFixture extent, ITestOutputHelper output)
    {
        var type = output.GetType();
        var testMember = type.GetField("test", BindingFlags.Instance | BindingFlags.NonPublic);
        var test = (ITest)testMember?.GetValue(output)!;

        _extent = extent.Extent;
        _test = _extent.CreateTest($"Select Class Tests - {test.TestCase.TestMethod.Method.Name}");

        _webDriver = new FirefoxDriver();
    }

    [Fact]
    public void TestSelect()
    {
        try
        {
            _webDriver
                .Navigate()
                .GoToUrl("https://www.w3schools.com/tags/tryit.asp?filename=tryhtml_select");
            _webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);

            var iframe = _webDriver.FindElement(By.Id("iframeResult"));
            _webDriver.SwitchTo().Frame(iframe);

            var selectElement = _webDriver.FindElement(By.CssSelector("#cars"));
            var select = new SelectElement(selectElement);
            select.SelectByIndex(2);

            Assert.Equal(4, select.Options.Count);
            Assert.Equal("Opel", select.SelectedOption.Text.Trim());
            _test.Log(Status.Pass, "Option selection works as expected. Opel is selected.");
        }
        catch (Exception e)
        {
            _test.Log(Status.Fail, "Test Failed. See below for details.");
            _test.Log(Status.Error, e.Message);
        }
    }
    
    public void Dispose()
    {
        _webDriver.Close();
        _webDriver.Quit();
        _webDriver.Dispose();
    }
}