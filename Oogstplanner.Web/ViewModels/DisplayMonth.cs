using System;

namespace Oogstplanner.ViewModels
{
    public class MonthViewModel
    {
        public MonthViewModel(string name, bool hasActions)
        {
            MonthForDisplay = name;
            HasActions = hasActions;
        }

        public string MonthForDisplay { get; set; }
        public bool HasActions { get; set; }
    }
}
    