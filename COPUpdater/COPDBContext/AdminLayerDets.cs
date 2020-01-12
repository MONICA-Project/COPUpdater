using System;
using System.Collections.Generic;

namespace IO.Swagger.COPDBContext
{
    public partial class AdminLayerDets
    {
        public long Layerdetid { get; set; }
        public int Layerid { get; set; }
        public string Thingids { get; set; }
        public string Obspropertyids { get; set; }

        public AdminLayer Layer { get; set; }
    }
}
