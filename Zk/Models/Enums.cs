using System;

namespace Zk
{
	/// <summary>
	///     Enumeration for the months of the year. More months can be selected.
	/// </summary>
	[Flags]
	public enum Month
	{
		NotSet 		= 0,		/* 0b000000000000 */
		January		= 1 << 0,	/* 0b000000000001 */
		February 	= 1 << 1,	/* 0b000000000010 */
		March 		= 1 << 2,	/* 0b000000000100 */
		April 		= 1 << 3,	/* 0b000000001000 */
		May 		= 1 << 4,	/* 0b000000010000 */
		June 		= 1 << 5,	/* 0b000000100000 */	
		July 		= 1 << 6,	/* 0b000001000000 */		
		August	    = 1 << 7,	/* 0b000010000000 */	
		September 	= 1 << 8,	/* 0b000100000000 */
		October 	= 1 << 9,	/* 0b001000000000 */
		November 	= 1 << 10, 	/* 0b010000000000 */
		December 	= 1 << 11	/* 0b100000000000 */
	}

}
