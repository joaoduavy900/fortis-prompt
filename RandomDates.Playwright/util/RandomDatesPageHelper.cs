using Microsoft.Playwright;
using System.Linq;
using System.Text.RegularExpressions;
using System.Globalization;

public class RandomDatesPageHelper
{
    IPage page;
    private string url;
    public RandomDatesPageHelper(IPage page, string url)
    {
        this.page = page;
        this.url = url;
    }
    
    public Task Open() => page.GotoAsync(url);

    public Task SetNumberOfRandomDates(int n) => page.FillAsync(Selectors.NumberInput, n.ToString());
    
    public Task SetDateRange(DateTime startDate, DateTime endDate)
    {
        return Task.WhenAll(
            page.SelectOptionAsync(Selectors.StartDay, startDate.Day.ToString()),
            page.SelectOptionAsync(Selectors.StartMonth, startDate.Month.ToString()),
            page.SelectOptionAsync(Selectors.StartYear, startDate.Year.ToString()),

            page.SelectOptionAsync(Selectors.EndDay, endDate.Day.ToString()),
            page.SelectOptionAsync(Selectors.EndMonth, endDate.Month.ToString()),
            page.SelectOptionAsync(Selectors.EndYear, endDate.Year.ToString())
        );
    }

    public Task GenerateRandomDates() => page.ClickAsync(Selectors.GenerateButton);

    private async Task<string> GetTextFromLocator(ILocator locator)
    {
        await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        return await locator.InnerTextAsync();
    }

    private DateTime[] ExtractDatesFromText(string text)
    {
        var dateMatches = Regex.Matches(text, @"\b\d{4}-\d{2}-\d{2}\b");
        return dateMatches
            .Select(m => DateTime.ParseExact(m.Value, "yyyy-MM-dd", CultureInfo.InvariantCulture))
            .ToArray();
    }

    private DateTime ParseDate(string dateStr)
    {
        return DateTime.ParseExact(dateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture);
    }

    public async Task<DateTime[]> GetRandomDatesFromPage(int expectedCount)
    {
        var introParagraph = page.Locator(string.Format(Selectors.ResultsIntro, expectedCount));
        await introParagraph.WaitForAsync(new() { State = WaitForSelectorState.Visible });
        
        var resultsParagraph = introParagraph.Locator(Selectors.ResultsParagraph);
        var resultsText = await GetTextFromLocator(resultsParagraph);
        
        return ExtractDatesFromText(resultsText);
    }

    public async Task<(DateTime startDate, DateTime endDate)> GetDateRangeFromPage()
    {
        var rangeLocator = page.Locator(Selectors.RangeText);
        var rangeText = await GetTextFromLocator(rangeLocator);
        
        var dateMatches = Regex.Matches(rangeText, @"\b(\d{4}-\d{2}-\d{2})\b");
        
        if (dateMatches.Count >= 2)
        {
            var startDate = ParseDate(dateMatches[0].Groups[1].Value);
            var endDate = ParseDate(dateMatches[1].Groups[1].Value);
            return (startDate, endDate);
        }
        
        throw new InvalidOperationException($"Could not extract date range from text: {rangeText}");
    }
}
