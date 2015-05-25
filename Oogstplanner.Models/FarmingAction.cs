using System.ComponentModel.DataAnnotations;

namespace Oogstplanner.Models
{
    /// <summary>
    /// A farming action is the harvesting or sowing of a particular crop in a particular month.
    /// </summary>
    public class FarmingAction
    {
        public int Id { get; set; }
        public Month Month { get; set; }
        public ActionType Action { get; set; }
        public int CropCount { get ; set ; }

        public virtual Crop Crop { get; set; }

        [Required]
        public virtual Calendar Calendar { get; set; }

        /// <summary>
        /// This methods converts the action to its counterpart.
        /// </summary>
        /// <example>
        /// When an action says we have to harvest a broccoli in May,
        /// and a broccoli has a growing time of four months,
        /// this method returns the sowing action of a broccoli of four months ago
        /// which belongs to the same calendar and user.
        /// </example>
        /// <returns>The counterpart farming action.</returns>
        public FarmingAction CreateRelated()
        {
            return new FarmingAction
            {
                CropCount = this.CropCount,
                Calendar = this.Calendar,
                Crop = this.Crop,
                Month = this.Action == ActionType.Harvesting 
                    ? Month.Subtract(Crop.GrowingTime)
                    : Month.Add(Crop.GrowingTime),
                Action = this.Action == ActionType.Harvesting 
                    ? ActionType.Sowing
                    : ActionType.Harvesting
            };
        }
    }   

    /// <summary>
    /// Enumeration for action to take.
    /// </summary>
    public enum ActionType
    {
        Sowing = 0,
        Harvesting = 1
    }         
}
