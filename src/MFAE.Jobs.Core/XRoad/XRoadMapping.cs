using MFAE.Jobs.XRoad;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace MFAE.Jobs.XRoad
{
    [Table("XRoadMappings")]
    [Audited]
    public class XRoadMapping : FullAuditedEntity
    {

        public virtual XRoadLookupEnum Lookup { get; set; }

        public virtual XRoadServicesEnum ServiceName { get; set; }

        [Required]
        [StringLength(XRoadMappingConsts.MaxCodeLength, MinimumLength = XRoadMappingConsts.MinCodeLength)]
        public virtual string Code { get; set; }

        public virtual long SystemId { get; set; }

    }
}