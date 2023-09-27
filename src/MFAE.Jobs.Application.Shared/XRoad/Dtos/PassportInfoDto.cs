using System;
using System.Collections.Generic;
using System.Text;

namespace MFAE.Jobs.XRoad.Dtos
{
    public class PassportInfoDto
    {
        public string IDNo { get; set; }

        public string PassportNo { get; set; }

        public DateTime? IssueDate { get; set; }

        public DateTime? ExpireDate { get; set; }

        public string FirstName { get; set; }
        public string FatherName { get; set; }
        public string GrandFatherName { get; set; }
        public string FamilyName { get; set; }
        public string FullName
        {
            get
            {
                return string.Format("{0} {1} {2} {3}", FirstName, FatherName, GrandFatherName, FamilyName);
            }
        }
        public string MotherName { get; set; }
        /// <summary>
        /// FirstName_EN
        /// </summary>
        public string FirstNameEN { get; set; }
        /// <summary>
        /// FatherName_EN
        /// </summary>
        public string FatherNameEN { get; set; }
        /// <summary>
        /// GrandFatherName_EN
        /// </summary>
        public string GrandFatherNameEN { get; set; }
        /// <summary>
        /// FamilyName_EN
        /// </summary>
        public string FamilyNameEN { get; set; }

        public string FullNameEn
        {
            get
            {
                return string.Format("{0} {1} {2} {3}", FirstNameEN, FatherNameEN, GrandFatherNameEN, FamilyNameEN);
            }
        }
        /// <summary>
        /// MotherName_EN
        /// </summary>
        public string MotherNameEN { get; set; }


        public DateTime? BirthDate { get; set; }

        public int SexId { get; set; }
    }
}
