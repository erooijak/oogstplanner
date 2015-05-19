namespace Oogstplanner.Models
{
    public class MonthViewModel
    {
        public string MonthForDataAttribute { get; set; }
        public string MonthForDisplay { get; set; }
        public bool HasAction { get; set; }

        public MonthViewModel(string monthName, string monthDisplayName, bool hasAction)
        {
            MonthForDataAttribute = monthName;
            MonthForDisplay = monthDisplayName;
            HasAction = hasAction;
        }
    }
}
