using System;
using System.Collections.Generic;

namespace IO.Swagger.COPDBContext
{
    public partial class Restriction
    {
        public Restriction()
        {
            LocationRestrictions = new HashSet<LocationRestrictions>();
            ThingRestrictions = new HashSet<ThingRestrictions>();
        }

        public int Restrictionid { get; set; }
        public string Name { get; set; }
        public short Typeid { get; set; }
        public string Metadata { get; set; }

        public ICollection<LocationRestrictions> LocationRestrictions { get; set; }
        public ICollection<ThingRestrictions> ThingRestrictions { get; set; }
    }
}
