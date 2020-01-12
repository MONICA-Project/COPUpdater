using System;
using System.Collections.Generic;

namespace IO.Swagger.COPDBContext
{
    public partial class ThingServices
    {
        public long Thingserviceid { get; set; }
        public long Thingid { get; set; }
        public int Serviceid { get; set; }

        public ServiceRepository Service { get; set; }
        public Thing Thing { get; set; }
    }
}
