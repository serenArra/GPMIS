using System;
using System.Collections.Generic;
using System.Text;

namespace MFAE.Jobs.XrodService.Dto
{
    public class CitizenInformation
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

        public int GenderTypesId { get; set; }

        public int MaritalStatusId { get; set; }
    }
}
