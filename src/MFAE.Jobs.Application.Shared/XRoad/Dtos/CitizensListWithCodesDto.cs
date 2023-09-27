using System;
using System.Collections.Generic;
using System.Text;

namespace MFAE.Jobs.XRoad.Dtos
{
    public class CitizensListWithCodesDto
    {
        public string CardID { get; set; }
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
        public string PreviousFamilyName { get; set; }
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
        /// <summary>
        /// MotherName_EN
        /// </summary>
        public string MotherNameEN { get; set; }
        /// <summary>
        /// FirstName_HE
        /// </summary>
        public string FirstNameHE { get; set; }
        /// <summary>
        /// FatherName_HE
        /// </summary>
        public string FatherNameHE { get; set; }
        /// <summary>
        /// FamilyName_HE
        /// </summary>
        public string FamilyNameHE { get; set; }
        /// <summary>
        /// MotherName_HE
        /// </summary>
        public string MotherNameHE { get; set; }
        /// <summary>
        /// GenderName_EN
        /// </summary>
        public string GenderNameEN { get; set; }
        /// <summary>
        /// GenderName_HE
        /// </summary>
        public string GenderNameHE { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? DeathDate { get; set; }
        public string Governorate { get; set; }
        /// <summary>
        /// Governorate_EN
        /// </summary>
        public string GovernorateEN { get; set; }
        public string BirthCityName { get; set; }
        public string BirthCountryName { get; set; }
        public string BirthPlace
        {
            get
            {
                return string.Format("{0} - {1}", BirthCountryName, BirthCityName);
            }
        }
        public string ReligionName { get; set; }
        /// <summary>
        /// ReligionName_EN
        /// </summary>
        public string ReligionNameEN { get; set; }
        /// <summary>
        /// ReligionName_HE
        /// </summary>
        public string ReligionNameHE { get; set; }
        public string MaritalStatusName { get; set; }
        /// <summary>
        /// MaritalStatusName_EN
        /// </summary>
        public string MaritalStatusNameEN { get; set; }
        /// <summary>
        /// MaritalStatusName_HE
        /// </summary>
        public string MaritalStatusNameHE { get; set; }
        public string CityId { get; set; }
        public string ReligionId { get; set; }
        public string RegiondId { get; set; }
        public string SexId { get; set; }
        /// <summary>
        /// CityName_AR
        /// </summary>
        public string CityNameAR { get; set; }
        /// <summary>
        /// CityName_EN
        /// </summary>
        public string CityNameEN { get; set; }
        //public string ResultCode { get; set; }
        //public string ResultCodeDesc { get; set; }
    }
}
