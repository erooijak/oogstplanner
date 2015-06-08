using System;
using System.Collections.Generic;

namespace Oogstplanner.Models
{
    public class User
    {
        public User()
        {
            Calendars = new List<Calendar>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
        public AuthenticatedStatus AuthenticationStatus { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual ICollection<Calendar> Calendars { get; set; }
    }

    public enum AuthenticatedStatus
    {
        Anonymous,
        Authenticated
    }
}
