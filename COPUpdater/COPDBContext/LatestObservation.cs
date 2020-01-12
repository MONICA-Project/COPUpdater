using System;
using System.Collections.Generic;

namespace IO.Swagger.COPDBContext
{
    public partial class LatestObservation
    {
        public long Thingid { get; set; }
        public string Datastreamid { get; set; }
        public DateTime? Phenomentime { get; set; }
        public string Observationresult { get; set; }
        public string Type { get; set; }
        public int? Personid { get; set; }
        public int? Locationid { get; set; }
    }
}
