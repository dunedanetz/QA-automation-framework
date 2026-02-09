using System;
using NUnit.Framework;
using Rosetic.AutomationTests.Pages;

namespace Rosetic.AutomationTests.Tests;

public sealed class RoseticSmokeTests : BaseUiTest
{
    [Test]
    public async Task HomePage_Loads_And_ShowsContent()
    {
        var home = new HomePage(Driver.Page);

        await home.NavigateAsync(Config.BaseUrl);

        Assert.That(
            await home.IsLoadedAsync(),
            Is.True,
            "Home page is not loading or showing expected content."
        );
    }

    [Test]
    public async Task JoinWaitlist_Cta_Navigates()
    {
        var home = new HomePage(Driver.Page);
        await home.NavigateAsync(Config.BaseUrl);

        await home.ClickJoinWaitlistAsync();
        await Driver.Page.WaitForTimeoutAsync(500);

        Assert.That(
            Driver.Page.Url,
            Does.Contain("waitlist"),
            "Join Waitlist did not navigate to waitlist section."
        );
    }

    [Test]
    public async Task Footer_IsVisible_And_LegalLinks_Work()
    {
        var page = Driver.Page;
        var home = new HomePage(page);

        await home.NavigateAsync(Config.BaseUrl);

        await home.ScrollToFooterAsync();
        await page.WaitForTimeoutAsync(300);

        Assert.That(
            await home.FooterVisibleAsync(),
            Is.True,
            "Footer is not visible after scrolling."
        );

        try
        {
            var popup = await page.RunAndWaitForPopupAsync(
                async () => await home.ClickFooterLegalAsync("terms"),
                new() { Timeout = 2000 }
            );

            Assert.That(
                popup.Url,
                Does.Contain("terms"),
                "Terms link did not navigate correctly (popup)."
            );

            await popup.CloseAsync();
        }
        catch (TimeoutException)
        {
            await page.WaitForTimeoutAsync(300);

            Assert.That(
                page.Url,
                Does.Contain("terms"),
                "Terms link did not navigate correctly (same tab)."
            );
        }
    }
}
