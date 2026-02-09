using NUnit.Framework;
using AutomationTests.Framework;

namespace AutomationTests.Tests;

public abstract class BaseUiTest
{
    protected PlaywrightDriver Driver = default!;
    protected TestConfig Config = default!;

    [OneTimeSetUp]
    public async Task GlobalSetup()
    {
        Config = ConfigLoader.Load(Path.Combine(TestContext.CurrentContext.TestDirectory, "appsettings.json"));
        Driver = new PlaywrightDriver();
        await Driver.InitAsync(Config);
    }

    [OneTimeTearDown]
    public async Task GlobalTeardown()
    {
        if (Driver != null)
            await Driver.DisposeAsync();
    }
}
