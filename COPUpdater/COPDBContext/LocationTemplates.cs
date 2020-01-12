using System;
using System.Collections.Generic;

namespace IO.Swagger.COPDBContext
{
    public partial class LocationTemplates
    {
        public LocationTemplates()
        {
            Location = new HashSet<Location>();
        }

        public int Locationtemplateid { get; set; }
        public string Name { get; set; }
        public byte[] Templateimage { get; set; }
        public bool PublicAccess { get; set; }

        public ICollection<Location> Location { get; set; }
    }
}
