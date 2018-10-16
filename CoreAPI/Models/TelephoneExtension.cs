using System;

namespace CoreAPI.Models
{
    public class TelephoneExtension
    {
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public int RecordId { get; set; }
        public int ParentId { get; set; }
        public string ExtensionType { get; set; }
        public string ExtensionName{ get; set; }
        public string Number { get; set; }
        public DateTime ChangeDate { get; set; }
        public string UserChanger { get; set; }
    }
}
