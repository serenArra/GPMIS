using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.AspNetZeroCore.Validation;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.Extensions;
using MFAE.Jobs.Authorization.Accounts.Dto;
using MFAE.Jobs.Authorization.Users;
using MFAE.Jobs.Authorization.Users.Dto;
using MFAE.Jobs.Security;

namespace MFAE.Jobs.Web.Models.Account
{
    public class RegisterViewModel : IValidatableObject
    {
        [Required]
        public string DocumentNo { get; set; }

        public int IdentificationTypeId { get; set; }

        [Required]
        [StringLength(User.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(UserConsts.MaxSecondNameLength)]
        public string SecondName { get; set; }

        [Required]
        [StringLength(UserConsts.MaxThirdNameLength)]
        public string ThirdName { get; set; }

        [Required]
        [StringLength(User.MaxSurnameLength)]
        public string Surname { get; set; }

        public string FirstNameEn { get; set; }

        public string SecondNameEn { get; set; }

        public string ThirdNameEn { get; set; }

        public string FourthNameEn { get; set; }

        [StringLength(AbpUserBase.MaxUserNameLength)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxPhoneNumberLength)]
        public string PhoneNumber { get; set; }

        [StringLength(User.MaxPlainPasswordLength)]
        [DisableAuditing]
        public string Password { get; set; }

        public bool IsExternalLogin { get; set; }

        public string ExternalLoginAuthSchema { get; set; }

        public string ReturnUrl { get; set; }

        public string SingleSignIn { get; set; }

        public PasswordComplexitySetting PasswordComplexitySetting { get; set; }

        public List<UserIdentificationTypeLookupTableDto> IdentificationTypeList { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!UserName.IsNullOrEmpty())
            {
                if (!UserName.Equals(EmailAddress, StringComparison.OrdinalIgnoreCase) && ValidationHelper.IsEmail(UserName))
                {
                    yield return new ValidationResult("Username cannot be an email address unless it's same with your email address !");
                }
            }
        }
    }
}