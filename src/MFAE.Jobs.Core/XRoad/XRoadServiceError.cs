using MFAE.Jobs.XRoad;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace MFAE.Jobs.XRoad
{
    [Table("XRoadServiceErrors")]
    [Audited]
    public class XRoadServiceError : FullAuditedEntity
    {

        [Required]
        public virtual string ErrorCode { get; set; }

        [Required]
        public virtual string ErrorMessageAr { get; set; }

        [Required]
        public virtual string ErrorMessageEn { get; set; }

        public virtual int XRoadServiceId { get; set; }

        [ForeignKey("XRoadServiceId")]
        public XRoadService XRoadServiceFk { get; set; }

    }
}