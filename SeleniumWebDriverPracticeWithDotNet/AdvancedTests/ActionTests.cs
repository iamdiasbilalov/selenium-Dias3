using System.Reflection;
using AventStack.ExtentReports;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using Xunit.Abstractions;

namespace SeleniumWebDriverPracticeWithDotNet.AdvancedTests;

public sealed class ActionTests : IClassFixture<ExtentReportsFixture>, IDisposable
{
    private readonly ExtentReports _extent;
    private readonly ExtentTest _test;
    private readonly IWebDriver _webDriver;
    private const string Url = "https://www.selenium.dev/selenium/web/mouse_interaction.html";

    public ActionTests(ExtentReportsFixture extent, ITestOutputHelper output)
    {
        var type = output.GetType();
        var testMember = type.GetField("test", BindingFlags.Instance | BindingFlags.NonPublic);
        var test = (ITest)testMember?.GetValue(output)!;

        _extent = extent.Extent;
        _test = _extent.CreateTest($"Actions Class Tests - {test.TestCase.TestMethod.Method.Name}");

        _webDriver = new FirefoxDriver();
    }

    [Fact]
    public void DragAndDropElement_DroppedTextShouldAppear()
    {
        try
        {
            _webDriver
                .Navigate()
                .GoToUrl(Url);

            var dropStatusElement = _webDriver.FindElement(By.Id("drop-status"));
            var draggableElement = _webDriver.FindElement(By.Id("draggable"));
            var droppableElement = _webDriver.FindElement(By.Id("droppable"));

            new Actions(_webDriver)
                .DragAndDrop(draggableElement, droppableElement)
                .Release()
                .Build()
                .Perform();

            _webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);

            Assert.Equal("dropped", dropStatusElement.Text.Trim());
            _test.Log(Status.Pass, "Drag and drop works as expected. 'Dropped' text appears.");
        }
        catch (Exception e)
        {
            _test.Log(Status.Fail, "Test Failed. See below for details.");
            _test.Log(Status.Error, e.Message);
        }
    }

    [Fact]
    public void MouseHoveringOverElements_HoveredTextAndLocationShouldAppear()
    {
        try
        {
            _webDriver
                .Navigate()
                .GoToUrl(Url);

            _webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);

            var hoverStatusElement = _webDriver.FindElement(By.Id("move-status"));
            var hoverableElement = _webDriver.FindElement(By.Id("hover"));

            var relativeLocationElement = _webDriver.FindElement(By.Id("relative-location"));
            var mouseTrackerElement = _webDriver.FindElement(By.Id("mouse-tracker"));

            var actions = new Actions(_webDriver);

            actions.MoveToElement(hoverableElement)
                .Pause(TimeSpan.FromSeconds(3))
                .Perform();

            Assert.NotEmpty(hoverStatusElement.Text.Trim());
            Assert.Equal("hovered", hoverStatusElement.Text.Trim());
            _test.Log(Status.Pass, "Hovering over elements work as expected. 'Hovered' text appears.");

            actions.MoveToElement(mouseTrackerElement)
                .Pause(TimeSpan.FromSeconds(3))
                .Perform();

            Assert.NotEmpty(relativeLocationElement.Text.Trim());
            _test.Log(Status.Pass, "When Hovering over mouse tracking box, relative mouse location appears.");
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