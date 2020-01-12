using System;
using System.Collections.Generic;

namespace IO.Swagger.COPDBContext
{
    public partial class ThingRestrictions
    {
        public long Thingrestrictionid { get; set; }
        public long Thingid { get; set; }
        public int Restrictionid { get; set; }

        public Restriction Restriction { get; set; }
        public Thing Thing { get; set; }
    }
}
