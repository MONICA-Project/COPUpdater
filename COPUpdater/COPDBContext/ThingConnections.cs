using System;
using System.Collections.Generic;

namespace IO.Swagger.COPDBContext
{
    public partial class ThingConnections
    {
        public long Thingconid { get; set; }
        public long Thingid { get; set; }
        public long Refthingid { get; set; }

        public Thing Refthing { get; set; }
        public Thing Thing { get; set; }
    }
}
