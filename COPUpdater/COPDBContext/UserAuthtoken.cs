using System;
using System.Collections.Generic;

namespace IO.Swagger.COPDBContext
{
    public partial class UserAuthtoken
    {
        public int Userid { get; set; }
        public string Authtoken { get; set; }
        public DateTime Dateissued { get; set; }

        public UserAuth User { get; set; }
    }
}
