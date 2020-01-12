using System;
using System.Collections.Generic;

namespace IO.Swagger.COPDBContext
{
    public partial class AdminMenu
    {
        public AdminMenu()
        {
            AdminMenuRoles = new HashSet<AdminMenuRoles>();
        }

        public int Menuid { get; set; }
        public string Const { get; set; }
        public string Name { get; set; }

        public ICollection<AdminMenuRoles> AdminMenuRoles { get; set; }
    }
}
