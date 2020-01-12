using System;
using System.Collections.Generic;

namespace IO.Swagger.COPDBContext
{
    public partial class AdminLayer
    {
        public AdminLayer()
        {
            AdminLayerDets = new HashSet<AdminLayerDets>();
        }

        public int Layerid { get; set; }
        public string Name { get; set; }

        public ICollection<AdminLayerDets> AdminLayerDets { get; set; }
    }
}
