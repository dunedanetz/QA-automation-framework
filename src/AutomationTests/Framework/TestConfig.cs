namespace AutomationTests.Framework;

public enum BrowserType
{
    Chromium,
    Firefox,
    Webkit
}

public sealed record TestConfig(
    string BaseUrl,
    BrowserType Browser,
    bool Headless,
    int TimeoutMs
);
