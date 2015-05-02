using System;

namespace Oogstplanner.ViewModels
{
    public class MonthViewModel
    {
        public MonthViewModel(string name, bool hasAction)
        {
            MonthForDisplay = name;
            HasAction = hasAction;
        }

        public string MonthForDisplay { get; set; }
        public bool HasAction { get; set; }
    }
}
    