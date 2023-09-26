using MFAE.Jobs.ApplicationForm;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFAE.Jobs.XrodService.Dto
{
    public class XrodCitizenInformation
    {
        public string CardNo { get; set; }

        public virtual string ArFirstName { get; set; }


        public virtual string ArSecondName { get; set; }


        public virtual string ArThirdName { get; set; }


        public virtual string ArFourthName { get; set; }


        public virtual string EnFirstName { get; set; }


        public virtual string EnSecondName { get; set; }


        public virtual string EnThirdName { get; set; }


        public virtual string EnFourthName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string BirthPlace { get; set; }
        [Lookup("GenderTypes")]
        public string Gender { get; set; }
        [Lookup("MaritalStatuses")]
        public string MaritalStatus { get; set; }
    }

    public class mystring
    {
        public string name { get; set; }
    }
}
