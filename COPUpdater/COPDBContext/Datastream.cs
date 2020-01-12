using System;
using System.Collections.Generic;

namespace IO.Swagger.COPDBContext
{
    public partial class Datastream
    {
        public Datastream()
        {
            Observation = new HashSet<Observation>();
        }

        public long Datastreamid { get; set; }
        public long Thingid { get; set; }
        public long Sensorid { get; set; }
        public long Observedpropertyid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Unitofmeasurement { get; set; }

        public ObservedProperty Observedproperty { get; set; }
        public Sensor Sensor { get; set; }
        public Thing Thing { get; set; }
        public ICollection<Observation> Observation { get; set; }
    }
}
