using System;
using System.Collections.Generic;

namespace IO.Swagger.COPDBContext
{
    public partial class ObservedProperty
    {
        public ObservedProperty()
        {
            Datastream = new HashSet<Datastream>();
        }

        public long Observedpropertyid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Metadata { get; set; }

        public ICollection<Datastream> Datastream { get; set; }
    }
}
