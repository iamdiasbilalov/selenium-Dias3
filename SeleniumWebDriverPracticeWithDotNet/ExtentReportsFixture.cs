using AventStack.ExtentReports;

namespace SeleniumWebDriverPracticeWithDotNet;

public sealed class ExtentReportsFixture : IDisposable
{
    public ExtentReports Extent { get; }

    public ExtentReportsFixture()
    {
        ExtentReportsHelper.InitializeReport();
        Extent = ExtentReportsHelper.CreateTest("Initializing new test suite...").Extent;
    }

    public void Dispose()
    {
        ExtentReportsHelper.FinalizeReport();
    }
}