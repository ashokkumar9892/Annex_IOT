using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPMCRUDAPIs.Models
{
    public class ApptConfirmation
    {
        public string date { get; set; }
        public string appointmentid { get; set; }
        public string starttime { get; set; }
        public string departmentid { get; set; }
        public string appointmentstatus { get; set; }
        public string patientid { get; set; }
        public string patientFName { get; set; }

        public string patientLName { get; set; }

        public int duration { get; set; }
        public string appointmenttypeid { get; set; }
        public string appointmenttype { get; set; }
        public string providerid { get; set; }
        public bool chargeentrynotrequired { get; set; }
        public string patientappointmenttypename { get; set; }
    }
}
