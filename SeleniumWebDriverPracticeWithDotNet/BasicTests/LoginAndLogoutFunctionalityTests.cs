using System.Reflection;
using System.Text.Json;
using AventStack.ExtentReports;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using SeleniumWebDriverPracticeWithDotNet.Models;
using Xunit.Abstractions;

namespace SeleniumWebDriverPracticeWithDotNet.BasicTests;

public sealed class LoginAndLogoutFunctionalityTests : IClassFixture<ExtentReportsFixture>, IDisposable
{
    private readonly ExtentReports _extent;
    private readonly ExtentTest _test;
    private readonly IWebDriver _webDriver;
    private readonly UserData? _userData;
    
    public LoginAndLogoutFunctionalityTests(ExtentReportsFixture extent, ITestOutputHelper output)
    {
        var type = output.GetType();
        var testMember = type.GetField("test", BindingFlags.Instance | BindingFlags.NonPublic);
        var test = (ITest)testMember?.GetValue(output)!;

        _webDriver = new FirefoxDriver();
        
        var buildDirectory = Directory.GetCurrentDirectory();
        var jsonFilePath = Path.Combine(buildDirectory + "/Data", "UserData.json");
        var json = File.ReadAllText(jsonFilePath);

        _userData = JsonSerializer.Deserialize<UserData>(json)!;

        _extent = extent.Extent;
        _test = _extent.CreateTest($"Login and Logout Functionality Tests - {test.TestCase.TestMethod.Method.Name}");
    }

    [Fact]
    public void LoginToWebsiteAndLogout_UsingCssSelector()
    {
        try
        {
            // Arrange
            _webDriver
                .Navigate()
                .GoToUrl("https://saucedemo.com");

            var usernameField = _webDriver.FindElement(By.CssSelector("input#user-name"));
            var passwordField = _webDriver.FindElement(By.CssSelector("input#password"));
            var button = _webDriver.FindElement(By.CssSelector("input#login-button"));

            // Act
            usernameField.SendKeys(_userData?.Username);
            passwordField.SendKeys(_userData?.Password);
            button.Click();

            var wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(3));
            wait.Until(driver => driver.Url.Contains("inventory.html"));

            // Assert
            Assert.Contains("saucedemo.com/inventory.html", _webDriver.Url);

            var menuIcon = _webDriver.FindElement(By.CssSelector("button#react-burger-menu-btn"));
            menuIcon.Click();

            var logoutButton = _webDriver.FindElement(By.CssSelector("a#logout_sidebar_link"));
            logoutButton.Click();

            wait.Until(driver => driver.FindElement(By.CssSelector("input#login-button")));
            var loginButtonAfterLoggingOut = _webDriver.FindElement(By.CssSelector("input#login-button"));

            Assert.NotNull(loginButtonAfterLoggingOut);
            _test.Log(Status.Pass, "Test passed. Logging in and logging out work as expected.");
        }
        catch (Exception e)
        {
            _test.Log(Status.Fail, "Test Failed. See below for details.");
            _test.Log(Status.Error, e.Message);
        }
    }

    [Fact]
    public void LoginToWebsiteAndLogout_UsingXPath()
    {
        try
        {
            // Arrange
            _webDriver.Navigate().GoToUrl("https://saucedemo.com");

            var usernameField = _webDriver.FindElement(By.XPath("//input[@id='user-name']"));
            var passwordField = _webDriver.FindElement(By.XPath("//input[@id='password']"));
            var button = _webDriver.FindElement(By.XPath("//input[@id='login-button']"));

            // Act
            usernameField.SendKeys(_userData?.Username);
            passwordField.SendKeys(_userData?.Password);
            button.Click();

            var wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(3));
            wait.Until(driver => driver.Url.Contains("inventory.html"));
        
            // Assert
            Assert.Contains("saucedemo.com/inventory.html", _webDriver.Url);

            var menuIcon = _webDriver.FindElement(By.XPath("//button[@id='react-burger-menu-btn']"));
            menuIcon.Click();

            var logoutButton = _webDriver.FindElement(By.XPath("//a[@id='logout_sidebar_link']"));
            logoutButton.Click();

            wait.Until(driver => driver.FindElement(By.XPath("//input[@id='login-button']")));
            var loginButtonAfterLoggingOut = _webDriver.FindElement(By.XPath("//input[@id='login-button']"));

            Assert.NotNull(loginButtonAfterLoggingOut);
            _test.Log(Status.Pass, "Test passed. Logging in and logging out work as expected.");
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