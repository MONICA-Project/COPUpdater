using System;
using System.Collections.Generic;

namespace IO.Swagger.COPDBContext
{
    public partial class ServiceProperties
    {
        public int Servicepropertyid { get; set; }
        public int Serviceid { get; set; }
        public short Propertytypeid { get; set; }
        public string Metadata { get; set; }

        public ServiceRepository Service { get; set; }
    }
}
