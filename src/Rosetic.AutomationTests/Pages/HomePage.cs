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

    // Locators

    private ILocator HeroHeading => _page.Locator("h1").First;

    private ILocator JoinWaitlistCtaCandidates =>
        _page.GetByRole(AriaRole.Link, new() { Name = "JOIN WAITLIST" });

    public ILocator FooterAnchor =>
        _page.GetByText("TERMS & CONDITIONS", new() { Exact = false });

    public ILocator TermsLink =>
        _page.GetByText("TERMS & CONDITIONS", new() { Exact = false });

    public ILocator PrivacyLink =>
        _page.GetByText("PRIVACY POLICY", new() { Exact = false });

    public ILocator SecurityLink =>
        _page.GetByText("SECURITY", new() { Exact = false });

    // Actions

    public async Task NavigateAsync(string baseUrl)
    {
        await _page.GotoAsync(baseUrl, new()
        {
            WaitUntil = WaitUntilState.DOMContentLoaded,
            Timeout = 60000
        });

        // Wait for UI to appear
        await HeroHeading.WaitForAsync(new() { Timeout = 30000 });
    }

    public async Task<bool> IsLoadedAsync()
    {
        return await HeroHeading.IsVisibleAsync();
    }

    public async Task ClickJoinWaitlistAsync()
    {
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

    // Footer

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
