using System;
using System.Collections.Generic;

namespace IO.Swagger.COPDBContext
{
    public partial class Thing
    {
        public Thing()
        {
            Datastream = new HashSet<Datastream>();
            LocationThings = new HashSet<LocationThings>();
            PersonThings = new HashSet<PersonThings>();
            Report = new HashSet<Report>();
            ThingConnectionsRefthing = new HashSet<ThingConnections>();
            ThingConnectionsThing = new HashSet<ThingConnections>();
            ThingRestrictions = new HashSet<ThingRestrictions>();
            ThingServices = new HashSet<ThingServices>();
        }

        public long Thingid { get; set; }
        public int Templateid { get; set; }
        public string Templatevalues { get; set; }
        public short Thingtype { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public short Status { get; set; }
        public double? Lat { get; set; }
        public double? Lon { get; set; }
        public string Ogcid { get; set; }

        public ThingTemplates Template { get; set; }
        public ICollection<Datastream> Datastream { get; set; }
        public ICollection<LocationThings> LocationThings { get; set; }
        public ICollection<PersonThings> PersonThings { get; set; }
        public ICollection<Report> Report { get; set; }
        public ICollection<ThingConnections> ThingConnectionsRefthing { get; set; }
        public ICollection<ThingConnections> ThingConnectionsThing { get; set; }
        public ICollection<ThingRestrictions> ThingRestrictions { get; set; }
        public ICollection<ThingServices> ThingServices { get; set; }
    }
}
