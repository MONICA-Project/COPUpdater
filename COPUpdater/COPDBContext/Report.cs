using System;
using System.Collections.Generic;

namespace IO.Swagger.COPDBContext
{
    public partial class Report
    {
        public Report()
        {
            Action = new HashSet<Action>();
        }

        public long Reportid { get; set; }
        public long Eventid { get; set; }
        public short Reporttype { get; set; }
        public DateTime Reporttime { get; set; }
        public string Description { get; set; }
        public short Status { get; set; }
        public long? Thingid { get; set; }
        public string Evidence { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public bool? Allowmultiuserhandle { get; set; }
        public short? Priority { get; set; }
        public string Reportcode { get; set; }

        public Event Event { get; set; }
        public Thing Thing { get; set; }
        public ICollection<Action> Action { get; set; }
    }
}
