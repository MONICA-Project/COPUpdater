using System;
using System.Collections.Generic;

namespace IO.Swagger.COPDBContext
{
    public partial class Action
    {
        public long Actionid { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public short? Actortype { get; set; }
        public short? Actiontype { get; set; }
        public DateTime? Actiontime { get; set; }
        public long? Reportid { get; set; }
        public int? Personid { get; set; }
        public short? Status { get; set; }
        public string Metadata { get; set; }

        public Person Person { get; set; }
        public Report Report { get; set; }
    }
}
