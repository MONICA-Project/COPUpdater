using System;
using System.Collections.Generic;

namespace IO.Swagger.COPDBContext
{
    public partial class AdminMenuRoles
    {
        public int Menuid { get; set; }
        public int Roleid { get; set; }
        public bool Hasaccess { get; set; }

        public AdminMenu Menu { get; set; }
        public Role Role { get; set; }
    }
}
