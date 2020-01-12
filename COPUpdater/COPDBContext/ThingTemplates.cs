using System;
using System.Collections.Generic;

namespace IO.Swagger.COPDBContext
{
    public partial class ThingTemplates
    {
        public ThingTemplates()
        {
            Thing = new HashSet<Thing>();
        }

        public int Thingtemplateid { get; set; }
        public string Name { get; set; }
        public string Templateschema { get; set; }

        public ICollection<Thing> Thing { get; set; }
    }
}
