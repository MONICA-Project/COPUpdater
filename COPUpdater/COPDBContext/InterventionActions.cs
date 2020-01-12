using System;
using System.Collections.Generic;

namespace IO.Swagger.COPDBContext
{
    public partial class InterventionActions
    {
        public int Interventionactionid { get; set; }
        public int Interventionplanid { get; set; }
        public string Name { get; set; }
        public short? Priority { get; set; }

        public InterventionPlan Interventionplan { get; set; }
    }
}
