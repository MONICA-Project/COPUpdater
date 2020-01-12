using System;
using System.Collections.Generic;

namespace IO.Swagger.COPDBContext
{
    public partial class PersonThings
    {
        public int Personid { get; set; }
        public long Thingid { get; set; }
        public DateTime? Timepoint { get; set; }

        public Person Person { get; set; }
        public Thing Thing { get; set; }
    }
}
