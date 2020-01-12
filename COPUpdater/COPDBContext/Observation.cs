using System;
using System.Collections.Generic;

namespace IO.Swagger.COPDBContext
{
    public partial class Observation
    {
        public long Observationid { get; set; }
        public long Datastreamid { get; set; }
        public DateTime Phenomenontime { get; set; }
        public string Observationresult { get; set; }
        public DateTime Resulttime { get; set; }
        public long? Featureofinterestid { get; set; }

        public Datastream Datastream { get; set; }
        public FeatureOfInterest Featureofinterest { get; set; }
    }
}
