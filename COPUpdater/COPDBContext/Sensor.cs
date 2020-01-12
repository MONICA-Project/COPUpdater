using System;
using System.Collections.Generic;

namespace IO.Swagger.COPDBContext
{
    public partial class Sensor
    {
        public Sensor()
        {
            Datastream = new HashSet<Datastream>();
        }

        public long Sensorid { get; set; }
        public int Templateid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Metadata { get; set; }
        public short Status { get; set; }

        public SensorTemplates Template { get; set; }
        public ICollection<Datastream> Datastream { get; set; }
    }
}
