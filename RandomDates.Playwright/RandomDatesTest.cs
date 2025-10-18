using System;
using System.Threading.Tasks;
using Microsoft.Playwright;
using NUnit.Framework;

public class Tests
{

    IPlaywright playwright;
    IBrowser browser;
    IPage page;
    RandomDatesPageHelper randomDatesPageHelper;

    [SetUp]
    public async Task Setup()
    {
        playwright = await Playwright.CreateAsync();
        browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true,
        });

        var context = await browser.NewContextAsync();
        
        page = await context.NewPageAsync();

        const string url = "https://www.random.org/calendar-dates/";
        randomDatesPageHelper = new RandomDatesPageHelper(page, url);
    }

    [TearDown]
    public async Task Teardown()
    {
        if(page != null) await page.CloseAsync();
        if(browser != null) await browser.CloseAsync();
        playwright?.Dispose();
    }

    [TestCase(4, "2025-01-05", "2025-11-25")]
    public async Task ShouldGenerateRandomDatesWithinRange(int countDates, string startDateStr, string endDateStr)
    {
        var startParts = startDateStr.Split('-');
        var startDate = new DateTime(int.Parse(startParts[0]), int.Parse(startParts[1]), int.Parse(startParts[2]));
        var endParts = endDateStr.Split('-');
        var endDate = new DateTime(int.Parse(endParts[0]), int.Parse(endParts[1]), int.Parse(endParts[2]));
        Assert.That(startDate <= endDate);

        await randomDatesPageHelper.Open();
        await randomDatesPageHelper.SetNumberOfRandomDates(countDates);

        await randomDatesPageHelper.SetDateRange(startDate, endDate);
        await randomDatesPageHelper.GenerateRandomDates();
        
        var results = await randomDatesPageHelper.GetRandomDatesFromPage(countDates);
        Assert.That(results.Length, Is.EqualTo(countDates));
        
        var (pageStartDate, pageEndDate) = await randomDatesPageHelper.GetDateRangeFromPage();
        Assert.That(pageStartDate, Is.EqualTo(startDate));
        Assert.That(pageEndDate, Is.EqualTo(endDate));
        
        foreach(var generatedDate in results)
        {
            Console.WriteLine($"Generated date: {generatedDate:yyyy-MM-dd}");
            Assert.That(generatedDate >= startDate && generatedDate <= endDate, Is.True);
        }
    }
}