using System;
using System.Collections.Generic;

namespace IO.Swagger.COPDBContext
{
    public partial class ProAcousticFeedback
    {
        public int Id { get; set; }
        public int ProAcousticFeedbackType { get; set; }
        public string Phoneid { get; set; }
        public double? Lat { get; set; }
        public double? Lon { get; set; }
        public double ReportType { get; set; }
        public string Textmessage { get; set; }

        public ProAcousticFeedbackTypes ProAcousticFeedbackTypeNavigation { get; set; }
    }
}
