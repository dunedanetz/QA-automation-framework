using System;
using System.IO;
using System.Text.Json;

namespace AutomationTests.Framework;

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
        // Default values after refactoring
        var baseUrl = "https://example.com";
        var browser = BrowserType.Chromium;
        var headless = true;
        var timeoutMs = 30_000;

        // Load from appsettings.json needed after refactoring to set defaults in one place, but also to allow local overrides without env vars
        if (File.Exists(filePath))
        {
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
        }

        // Environment variable override (for CI to point to right URL)
        var envBaseUrl = Environment.GetEnvironmentVariable("BASE_URL");
        if (!string.IsNullOrWhiteSpace(envBaseUrl))
            baseUrl = envBaseUrl.Trim();

        return new TestConfig(baseUrl, browser, headless, timeoutMs);
    }

    private static BrowserType ParseBrowser(string browser)
    {
        return browser.Trim().ToLowerInvariant() switch
        {
            "firefox" => BrowserType.Firefox,
            "webkit"  => BrowserType.Webkit,
            _         => BrowserType.Chromium
        };
    }
}
