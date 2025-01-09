using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Config;

namespace SeleniumWebDriverPracticeWithDotNet;
public static class ExtentReportsHelper
{
    private static ExtentReports? _extent;
    private static ExtentTest? _test;

    public static void InitializeReport()
    {
        var reportDirectory = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "../../../TestReports");

        Directory.CreateDirectory(reportDirectory);
        var reportPath = Path.Combine(reportDirectory, "TestReport.html");

        var sparkReporter = new ExtentSparkReporter(reportPath)
        {
            Config =
            {
                Theme = Theme.Dark,
            }
        };

        _extent = new ExtentReports();
        _extent.AttachReporter(sparkReporter);
    }

    public static ExtentTest CreateTest(string testName)
    {
        _test = _extent?.CreateTest(testName);
        return _test!;
    }

    public static void FinalizeReport()
    {
        _extent?.Flush();
    }
}
