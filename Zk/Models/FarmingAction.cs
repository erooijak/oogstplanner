namespace Zk.Models
{
    /// <summary>
    ///     A farming action is the harvesting or sowing of a particular crop in a particular month.
    /// </summary>
    public class FarmingAction
    {
        public int Id { get; set; }
        public Month Month { get; set; }
        public ActionType Action { get; set; }
        public int CropCount { get ; set ; }

        public virtual Crop Crop { get; set; }
        public virtual Calendar Calendar { get; set; }
    }   

    /// <summary>
    ///     Enumeration for action to take.
    /// </summary>
    public enum ActionType
    {
        Sowing = 0,
        Harvesting = 1
    }
		
}