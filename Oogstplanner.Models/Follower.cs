namespace Oogstplanner.Models
{
    /// <summary>
    /// A many-to-many junction table between Users and Users.
    /// </summary>
    public class Follower
    {
        public int Id { get; set; }
        public virtual User Following { get; set; }
        public virtual User Followed { get; set; }
    }       
}
