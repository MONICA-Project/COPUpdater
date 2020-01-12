using System;
using System.Collections.Generic;

namespace IO.Swagger.COPDBContext
{
    public partial class InterventionPlan
    {
        public InterventionPlan()
        {
            InterventionActions = new HashSet<InterventionActions>();
        }

        public int Interventionplanid { get; set; }
        public short Interventiontype { get; set; }
        public string Name { get; set; }

        public ICollection<InterventionActions> InterventionActions { get; set; }
    }
}
