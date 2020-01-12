using System;
using System.Collections.Generic;

namespace IO.Swagger.COPDBContext
{
    public partial class Event
    {
        public Event()
        {
            EventLocations = new HashSet<EventLocations>();
            EventOrganizations = new HashSet<EventOrganizations>();
            Report = new HashSet<Report>();
        }

        public long Eventid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Fromdate { get; set; }
        public DateTime Todate { get; set; }
        public double? Lat { get; set; }
        public double? Lon { get; set; }
        public int? Zoom { get; set; }

        public ICollection<EventLocations> EventLocations { get; set; }
        public ICollection<EventOrganizations> EventOrganizations { get; set; }
        public ICollection<Report> Report { get; set; }
    }
}
