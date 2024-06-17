using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Mixins;
using Avalonia.Input;
using Avalonia.Interactivity;
using Irihi.Avalonia.Shared.Common;

namespace Ursa.Controls;

internal enum CalendarYearViewMode
{
    Month,
    Year,

    // The button represents 10 years. 
    YearRange
}

[PseudoClasses(PC_Range, PseudoClassName.PC_Selected)]
public class CalendarYearButton : ContentControl
{
    public const string PC_Range = ":range";

    public static readonly RoutedEvent<CalendarYearButtonEventArgs> ItemSelectedEvent =
        RoutedEvent.Register<CalendarYearButton, CalendarYearButtonEventArgs>(
            nameof(ItemSelected), RoutingStrategies.Bubble);

    static CalendarYearButton()
    {
        PressedMixin.Attach<CalendarYearButton>();
    }

    internal int Year { get; private set; }

    internal int Month { get; private set; }

    internal int StartYear { get; private set; }

    internal int EndYear { get; private set; }

    internal CalendarYearViewMode Mode { get; private set; }

    public event EventHandler<CalendarDayButtonEventArgs> ItemSelected
    {
        add => AddHandler(ItemSelectedEvent, value);
        remove => RemoveHandler(ItemSelectedEvent, value);
    }

    internal void SetValues(CalendarYearViewMode mode, DateTime contextDate, int? month = null, int? year = null,
        int? startYear = null, int? endYear = null)
    {
        Debug.Assert(!(month is null && year is null && startYear is null && endYear is null));
        Mode = mode;
        Month = month ?? 0;
        Year = year ?? 0;
        StartYear = startYear ?? 0;
        EndYear = endYear ?? 0;
        Content = Mode switch
        {
            CalendarYearViewMode.Month => DateTimeHelper.GetCurrentDateTimeFormatInfo().AbbreviatedMonthNames[Month],
            CalendarYearViewMode.Year => Year.ToString(),
            CalendarYearViewMode.YearRange => StartYear + "-" + EndYear,
            _ => Content
        };
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        RaiseEvent(new CalendarYearButtonEventArgs(Mode, Year, Month, StartYear, EndYear)
            { RoutedEvent = ItemSelectedEvent, Source = this });
    }
}