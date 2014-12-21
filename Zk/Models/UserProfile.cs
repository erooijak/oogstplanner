namespace Zk.Models
{
	public class UserProfile
	{
        public int UserProfileId { get; set; }
		public string UserName { get; set; }	
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool IsApproved { get; set; }
	}
}