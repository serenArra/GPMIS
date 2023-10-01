using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace MFAE.Jobs.Authorization.Accounts.Dto
{
    public class CitizenInput
    {
        public int IdentificationTypeId { get; set; }

        public string IdentityNo { get; set; }

        public string MotherName { get; set; }

        public string BirthDate { get; set; }

        public string DocIssueDate { get; set; }

        public string DocIssuePlace { get; set; }
    }
}
