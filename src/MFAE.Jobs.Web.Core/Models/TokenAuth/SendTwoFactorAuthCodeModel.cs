﻿using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.Web.Models.TokenAuth
{
    public class SendTwoFactorAuthCodeModel
    {
        [Range(1, long.MaxValue)]
        public long UserId { get; set; }

        [Required]
        public string Provider { get; set; }
    }
}