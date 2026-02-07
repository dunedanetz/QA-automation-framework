using System;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace Rosetic.AutomationTests.Pages;

public sealed class HomePage
{
    private readonly IPage _page;

    public HomePage(IPage page)
    {
        _page = page;
    }

    // --------- Locators ---------

    // Basic "page is loaded" signal: first H1 exists and is visible
    private ILocator HeroHeading => _page.Locator("h1").First;

    // Header Join Waitlist CTA candidates (sometimes there is also a hidden dropdown item)
    private ILocator JoinWaitlistCtaCandidates =>
        _page.GetByRole(AriaRole.Link, new() { Name = "JOIN WAITLIST" });

    // Fallback theme toggle button (kept as fallback only)
    private ILocator ThemeToggleButton =>
        _page.Locator("button").Filter(new() { Has = _page.Locator("svg") }).First;

    // Footer anchor: rely on visible footer text instead of <footer> tag
    public ILocator FooterAnchor =>
        _page.GetByText("TERMS & CONDITIONS", new() { Exact = false });

    public ILocator TermsLink =>
        _page.GetByText("TERMS & CONDITIONS", new() { Exact = false });

    public ILocator PrivacyLink =>
        _page.GetByText("PRIVACY POLICY", new() { Exact = false });

    public ILocator SecurityLink =>
        _page.GetByText("SECURITY", new() { Exact = false });

    // --------- Actions ---------

    public async Task NavigateAsync(string baseUrl)
    {
        // NetworkIdle may never happen on marketing sites due to long-lived requests (analytics, etc.)
        await _page.GotoAsync(baseUrl, new()
        {
            WaitUntil = WaitUntilState.DOMContentLoaded,
            Timeout = 60000
        });

        // Wait for a visible UI signal
        await HeroHeading.WaitForAsync(new() { Timeout = 30000 });
    }

    public async Task<bool> IsLoadedAsync()
    {
        return await HeroHeading.IsVisibleAsync();
    }

    public async Task ClickJoinWaitlistAsync()
    {
        // We may have multiple matches: hidden dropdown entry + visible header CTA.
        // Prefer visible. If first isn't visible, try last.
        var first = JoinWaitlistCtaCandidates.First;

        if (await first.IsVisibleAsync())
        {
            await first.ScrollIntoViewIfNeededAsync();
            await first.ClickAsync();
            return;
        }

        var last = JoinWaitlistCtaCandidates.Last;
        await last.ScrollIntoViewIfNeededAsync();
        await last.ClickAsync();
    }

    public async Task ToggleThemeAsync()
    {
        // Take a lightweight snapshot to detect whether our click had any effect.
        async Task<string> SnapshotAsync()
        {
            return await _page.EvaluateAsync<string>(
                @"() => {
                    const html = document.documentElement;
                    const body = document.body;

                    const sHtml = getComputedStyle(html);
                    const sBody = getComputedStyle(body);

                    const main =
                        document.querySelector('main') ||
                        document.querySelector('[class*=""page""]') ||
                        document.querySelector('[class*=""container""]');

                    const sMain = main ? getComputedStyle(main) : null;

                    return JSON.stringify({
                        htmlBg: sHtml.backgroundColor,
                        htmlColor: sHtml.color,
                        bodyBg: sBody.backgroundColor,
                        bodyColor: sBody.color,
                        mainBg: sMain ? sMain.backgroundColor : null,
                        colorScheme: sHtml.colorScheme
                    });
                }"
            );
        }

        var before = await SnapshotAsync();

        // Try likely candidates: visible buttons with SVG icons.
        var candidates = _page.Locator("button:has(svg)");
        var count = await candidates.CountAsync();
        var maxToTry = Math.Min(count, 8);

        for (int i = 0; i < maxToTry; i++)
        {
            var btn = candidates.Nth(i);

            if (!await btn.IsVisibleAsync())
                continue;

            await btn.ScrollIntoViewIfNeededAsync();
            await btn.ClickAsync();
            await _page.WaitForTimeoutAsync(300);

            var after = await SnapshotAsync();
            if (after != before)
            {
                // We found a button that changes observable theme indicators
                return;
            }
        }

        // Fallback: try the original basic locator once more
        await ThemeToggleButton.ScrollIntoViewIfNeededAsync();
        await ThemeToggleButton.ClickAsync();
    }

    // --------- Footer helpers ---------

    public async Task ScrollToFooterAsync()
    {
        await FooterAnchor.ScrollIntoViewIfNeededAsync();
    }

    public async Task<bool> FooterVisibleAsync()
    {
        return await FooterAnchor.IsVisibleAsync();
    }

    public async Task ClickFooterLegalAsync(string which)
    {
        switch (which.ToLowerInvariant())
        {
            case "terms":
                await TermsLink.ClickAsync();
                break;
            case "privacy":
                await PrivacyLink.ClickAsync();
                break;
            case "security":
                await SecurityLink.ClickAsync();
                break;
            default:
                throw new ArgumentException("Expected: terms/privacy/security");
        }
    }
}
