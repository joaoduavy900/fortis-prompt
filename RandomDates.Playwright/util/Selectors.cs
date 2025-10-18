public static class Selectors
{
    public const string NumberInput = "input[name='num']";
    public const string StartDay = "select[name='start_day']";
    public const string StartMonth = "select[name='start_month']";
    public const string StartYear = "select[name='start_year']";
    public const string EndDay = "select[name='end_day']";
    public const string EndMonth = "select[name='end_month']";
    public const string EndYear = "select[name='end_year']";
    public const string GenerateButton = "input[value='Get Dates']";
    public const string ResultsIntro = "p:has-text('Here are your {0} calendar dates:')";
    public const string ResultsParagraph = "xpath=following-sibling::p[1]";
    public const string RangeText = "text=/They were picked randomly out of \\d+ possible dates between \\d{4}-\\d{2}-\\d{2} and \\d{4}-\\d{2}-\\d{2}/";
}
