namespace Oogstplanner.Models
{
    /// <summary>
    /// A calendar can be liked by a user. 
    /// This is a many-to-many junction table between calendars and users
    /// </summary>
    public class Like
    {
        public int Id { get; set; }
        public virtual Calendar Calendar { get; set; }
        public virtual User User { get; set; }
    }       
}
