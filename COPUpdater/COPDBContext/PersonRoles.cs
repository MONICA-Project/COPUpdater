using System;
using System.Collections.Generic;

namespace IO.Swagger.COPDBContext
{
    public partial class PersonRoles
    {
        public int Personid { get; set; }
        public int Roleid { get; set; }

        public Person Person { get; set; }
        public Role Role { get; set; }
    }
}
