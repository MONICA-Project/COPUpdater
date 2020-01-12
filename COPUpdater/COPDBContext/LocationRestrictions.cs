using System;
using System.Collections.Generic;

namespace IO.Swagger.COPDBContext
{
    public partial class LocationRestrictions
    {
        public long Locationrestrictionid { get; set; }
        public long Locationid { get; set; }
        public int Restrictionid { get; set; }

        public Location Location { get; set; }
        public Restriction Restriction { get; set; }
    }
}
