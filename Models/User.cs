using System.Collections.Generic;

namespace Manufacturing.Api.Models
{
    public class User
    {
        private static User _user;

        public User()
        {
            Roles = new List<string>();
        }

        public static User Current
        {
            get { return _user ?? (_user = new User()); }
        }

        public string Name { get; set; }

        public bool IsInRole(string role)
        {
            return true;
        }

        public List<string> Roles { get; set; }
    }
}