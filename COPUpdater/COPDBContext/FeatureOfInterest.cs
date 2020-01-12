using System;
using System.Collections.Generic;

namespace IO.Swagger.COPDBContext
{
    public partial class FeatureOfInterest
    {
        public FeatureOfInterest()
        {
            Observation = new HashSet<Observation>();
        }

        public long Featureofinterestid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Observation> Observation { get; set; }
    }
}
