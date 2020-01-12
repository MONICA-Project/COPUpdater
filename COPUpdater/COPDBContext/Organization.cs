using System;
using System.Collections.Generic;

namespace IO.Swagger.COPDBContext
{
    public partial class Organization
    {
        public Organization()
        {
            EventOrganizations = new HashSet<EventOrganizations>();
            Person = new HashSet<Person>();
        }

        public int Organizationid { get; set; }
        public string Name { get; set; }

        public ICollection<EventOrganizations> EventOrganizations { get; set; }
        public ICollection<Person> Person { get; set; }
    }
}
