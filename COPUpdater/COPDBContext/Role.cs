using System;
using System.Collections.Generic;

namespace IO.Swagger.COPDBContext
{
    public partial class Role
    {
        public Role()
        {
            AdminMenuRoles = new HashSet<AdminMenuRoles>();
            PersonRoles = new HashSet<PersonRoles>();
        }

        public int Roleid { get; set; }
        public string Const { get; set; }
        public string Description { get; set; }

        public ICollection<AdminMenuRoles> AdminMenuRoles { get; set; }
        public ICollection<PersonRoles> PersonRoles { get; set; }
    }
}
