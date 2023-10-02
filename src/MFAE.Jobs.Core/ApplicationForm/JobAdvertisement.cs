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

    }
}