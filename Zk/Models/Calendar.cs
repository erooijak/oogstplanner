using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zk.Models
{
	/// <summary>
	/// 	A calendar contains the farming months with sowing and harvesting patterns for a user.
	/// </summary>
	public class Calendar
	{
        public int CalendarId { get; set; }

		[ForeignKey("User")]
		public int UserId { get; set; } // One-to-one relationship (each calendar belongs to one user)

		public virtual User User { get; set; }
		public virtual ICollection<FarmingAction> FarmingActions { get; set; } // One-to-many (each calendar has many
																			 // farming months)

	}
}