using System;
using System.Collections.Generic;

namespace IO.Swagger.COPDBContext
{
    public partial class ProAcousticFeedbackTypes
    {
        public ProAcousticFeedbackTypes()
        {
            ProAcousticFeedback = new HashSet<ProAcousticFeedback>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<ProAcousticFeedback> ProAcousticFeedback { get; set; }
    }
}
