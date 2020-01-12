using System;
using System.Collections.Generic;

namespace IO.Swagger.COPDBContext
{
    public partial class Incident
    {
        public int Incidentid { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Position { get; set; }
        public int? Prio { get; set; }
        public string Status { get; set; }
        public double? Probability { get; set; }
        public string Interventionplan { get; set; }
        public DateTime Incidenttime { get; set; }
        public string WearablePhysicalId { get; set; }
        public string PhoneNumber { get; set; }
        public string AdditionalMedia { get; set; }
        public string AdditionalMediaType { get; set; }
        public string Area { get; set; }
    }
}
