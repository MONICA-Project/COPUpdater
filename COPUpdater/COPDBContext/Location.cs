using System;
using System.Collections.Generic;

namespace IO.Swagger.COPDBContext
{
    public partial class Location
    {
        public Location()
        {
            EventLocations = new HashSet<EventLocations>();
            LocationRestrictions = new HashSet<LocationRestrictions>();
            LocationServices = new HashSet<LocationServices>();
            LocationThings = new HashSet<LocationThings>();
        }

        public long Locationid { get; set; }
        public short Locationtypeid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long? Parentid { get; set; }
        public string Boundingpolygon { get; set; }
        public string Metadata { get; set; }
        public int? Locationtemplateid { get; set; }
        public int? Capacity { get; set; }

        public LocationTemplates Locationtemplate { get; set; }
        public ICollection<EventLocations> EventLocations { get; set; }
        public ICollection<LocationRestrictions> LocationRestrictions { get; set; }
        public ICollection<LocationServices> LocationServices { get; set; }
        public ICollection<LocationThings> LocationThings { get; set; }
    }
}
