using MFAE.Jobs.XRoad;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace MFAE.Jobs.XRoad
{
    [Table("XRoadServices")]
    [Audited]
    public class XRoadService : FullAuditedEntity
    {

        [Required]
        [StringLength(XRoadServiceConsts.MaxNameLength, MinimumLength = XRoadServiceConsts.MinNameLength)]
        public virtual string Name { get; set; }

        [Required]
        [StringLength(XRoadServiceConsts.MaxProviderCodeLength, MinimumLength = XRoadServiceConsts.MinProviderCodeLength)]
        public virtual string ProviderCode { get; set; }

        [Required]
        [StringLength(XRoadServiceConsts.MaxResultCodePathLength, MinimumLength = XRoadServiceConsts.MinResultCodePathLength)]
        public virtual string ResultCodePath { get; set; }

        [Required]
        public virtual string ActionName { get; set; }

        [Required]
        public virtual string SoapActionName { get; set; }

        [Required]
        public virtual string VersionNo { get; set; }

        [Required]
        public virtual string ProducerCode { get; set; }

        [StringLength(XRoadServiceConsts.MaxDescriptionLength, MinimumLength = XRoadServiceConsts.MinDescriptionLength)]
        public virtual string Description { get; set; }

        public virtual XRoadServiceStatusEnum Status { get; set; }

        public virtual string Code { get; set; }

    }
}