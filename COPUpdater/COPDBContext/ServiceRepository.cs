using System;
using System.Collections.Generic;

namespace IO.Swagger.COPDBContext
{
    public partial class ServiceRepository
    {
        public ServiceRepository()
        {
            LocationServices = new HashSet<LocationServices>();
            ServiceProperties = new HashSet<ServiceProperties>();
            ThingServices = new HashSet<ThingServices>();
        }

        public int Serviceid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }

        public ICollection<LocationServices> LocationServices { get; set; }
        public ICollection<ServiceProperties> ServiceProperties { get; set; }
        public ICollection<ThingServices> ThingServices { get; set; }
    }
}
