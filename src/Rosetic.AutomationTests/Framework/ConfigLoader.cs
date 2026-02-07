using System.Text.Json;

namespace Rosetic.AutomationTests.Framework;

public static class ConfigLoader
{
    private sealed class RawConfig
    {
        public string? baseUrl { get; set; }
        public string? browser { get; set; }
        public bool? headless { get; set; }
        public int? timeoutMs { get; set; }
    }

    public static TestConfig Load(string filePath)
    {
        // Defaults (keep it simple + stable)
        var baseUrl = "https://www.rosetic.ai/";
        var browser = BrowserType.Chromium;
        var headless = true;
        var timeoutMs = 30_000;

        if (!File.Exists(filePath))
            return new TestConfig(baseUrl, browser, headless, timeoutMs);

        var json = File.ReadAllText(filePath);

        var raw = JsonSerializer.Deserialize<RawConfig>(json) ?? new RawConfig();

        if (!string.IsNullOrWhiteSpace(raw.baseUrl))
            baseUrl = raw.baseUrl.Trim();

        if (!string.IsNullOrWhiteSpace(raw.browser))
            browser = ParseBrowser(raw.browser);

        if (raw.headless.HasValue)
            headless = raw.headless.Value;

        if (raw.timeoutMs.HasValue && raw.timeoutMs.Value > 0)
            timeoutMs = raw.timeoutMs.Value;

        return new TestConfig(baseUrl, browser, headless, timeoutMs);
    }

    private static BrowserType ParseBrowser(string value) =>
        value.Trim().ToLowerInvariant() switch
        {
            "firefox" => BrowserType.Firefox,
            "webkit" => BrowserType.Webkit,
            _ => BrowserType.Chromium
        };
}
