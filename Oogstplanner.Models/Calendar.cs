using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Oogstplanner.Models
{
    /// <summary>
    /// A calendar contains the farming months with sowing and harvesting patterns for a user.
    /// </summary>
    public class Calendar
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [Required]
        public virtual User User { get; set; }
        public virtual ICollection<FarmingAction> FarmingActions { get; set; } // One-to-many (each calendar has many
    }                                                                          // farming months)
}
