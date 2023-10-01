using Abp.Web.Models;
using MFAE.Jobs.ApplicationForm.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace MFAE.Jobs.Authorization.Accounts.Dto
{
    public class CitizenOutput : ErrorInfo
    {
        public CitizenOutput()
        {
        }
        public CitizenOutput(ErrorInfo error)
        {
            Code = error.Code;
            Details = error.Details;
            Message = error.Message;
            ValidationErrors = error.ValidationErrors;
        }


        public int IdentificationTypeId { get; set; }

        public string IdentityNo { get; set; }

        public DateTime? BirthDate { get; set; }

        public DateTime? IDCardIssuance { get; set; }


        public string BirthPlace { get; set; }


        public string MotherName { get; set; }

        public string Name { get; set; }


        public string SecondName { get; set; }


        public string ThirdName { get; set; }


        public string Surname { get; set; }


        public string UserName { get; set; }

        public string EmailAddress { get; set; }


        public string PhoneNumber { get; set; }

        public string Password { get; set; }

        public bool IsExternalLogin { get; set; }

        public string ExternalLoginAuthSchema { get; set; }
        public string ReturnUrl { get; set; }
        public string SingleSignIn { get; set; }

        public string FirstNameEn { get; set; }
        public string SecondNameEn { get; set; }
        public string ThirdNameEn { get; set; }
        public string FourthNameEn { get; set; }

        public int? DocumentIssuancePlaceId { get; set; }

        public bool IsDate { get; set; }
        public int? QuestionId { get; set; }
        public string QuestionAnswer { get; set; }
        public string MotherNameEn { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string EmailActivationLink { get; set; }
        public DateTime DeathDate { get; set; }
        public string RegionName { get; set; }
        public string RegionNameEn { get; set; }
        public string MaritalStatus { get; set; }
        public string MaritalStatusEn { get; set; }
        public Gender Gender { get; set; }
        public int? GovernorateId { get; set; }
        public int? CountryId { get; set; }
        public string DialCode { get; set; }
        public int CitizenStatusId { get; set; }
        public int FaultCode { get; set; }
        public string FaultDesc { get; set; }
        public string DocPhoto { get; set; }
        public string OtherPhoto { get; set; }
        public bool IsExist { get; set; }
    }
}
