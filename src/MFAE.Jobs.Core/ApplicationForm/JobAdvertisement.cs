using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace MFAE.Jobs.ApplicationForm
{
    [Table("JobAdvertisements")]
    [Audited]
    public class JobAdvertisement : FullAuditedEntity
    {

        public virtual string Description { get; set; }

        [Required]
        [StringLength(JobAdvertisementConsts.MaxAdvertisementIdLength, MinimumLength = JobAdvertisementConsts.MinAdvertisementIdLength)]
        public virtual string AdvertisementId { get; set; }

        public virtual DateTime AdvertisementDate { get; set; }

        public virtual DateTime FromDate { get; set; }

        public virtual DateTime ToDate { get; set; }

        public virtual double AllowedAge { get; set; }

        public virtual bool IsActive { get; set; }

    }
}