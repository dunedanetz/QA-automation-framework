using Microsoft.Playwright;

namespace AutomationTests.Framework;

public sealed class PlaywrightDriver : IAsyncDisposable
{
    public IPlaywright Playwright { get; private set; } = default!;
    public IBrowser Browser { get; private set; } = default!;
    public IBrowserContext Context { get; private set; } = default!;
    public IPage Page { get; private set; } = default!;

    public async Task InitAsync(TestConfig config)
    {
        Playwright = await Microsoft.Playwright.Playwright.CreateAsync();

        Browser = config.Browser switch
        {
            BrowserType.Firefox => await Playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = config.Headless
            }),
            BrowserType.Webkit => await Playwright.Webkit.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = config.Headless
            }),
            _ => await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = config.Headless
            }),
        };

        Context = await Browser.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize { Width = 1366, Height = 768 }
        });

        Page = await Context.NewPageAsync();
        Page.SetDefaultTimeout(config.TimeoutMs);
    }

    public async ValueTask DisposeAsync()
    {
        if (Context is not null)
            await Context.CloseAsync();

        if (Browser is not null)
            await Browser.CloseAsync();

        Playwright?.Dispose();
    }
}
