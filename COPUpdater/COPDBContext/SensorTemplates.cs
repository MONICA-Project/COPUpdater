using System;
using System.Collections.Generic;

namespace IO.Swagger.COPDBContext
{
    public partial class SensorTemplates
    {
        public SensorTemplates()
        {
            Sensor = new HashSet<Sensor>();
        }

        public int Sensortemplateid { get; set; }
        public short Sensortype { get; set; }
        public string Name { get; set; }
        public string Templateschema { get; set; }

        public ICollection<Sensor> Sensor { get; set; }
    }
}
