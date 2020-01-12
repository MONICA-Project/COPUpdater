using System;
using System.Collections.Generic;

namespace IO.Swagger.COPDBContext
{
    public partial class UserAuth
    {
        public UserAuth()
        {
            UserAuthtoken = new HashSet<UserAuthtoken>();
        }

        public int Userid { get; set; }
        public string Username { get; set; }
        public string Userpassword { get; set; }

        public Person User { get; set; }
        public ICollection<UserAuthtoken> UserAuthtoken { get; set; }
    }
}
