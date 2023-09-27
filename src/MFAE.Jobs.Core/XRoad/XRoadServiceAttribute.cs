using MFAE.Jobs.XRoad;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace MFAE.Jobs.XRoad
{
    [Table("XRoadServiceAttributes")]
    [Audited]
    public class XRoadServiceAttribute : FullAuditedEntity
    {

        public virtual XRoadServiceAttributeTypeEnum ServiceAttributeType { get; set; }

        [StringLength(XRoadServiceAttributeConsts.MaxAttributeCodeLength, MinimumLength = XRoadServiceAttributeConsts.MinAttributeCodeLength)]
        public virtual string AttributeCode { get; set; }

        [Required]
        [StringLength(XRoadServiceAttributeConsts.MaxXMLPathLength, MinimumLength = XRoadServiceAttributeConsts.MinXMLPathLength)]
        public virtual string XMLPath { get; set; }

        [Required]
        [StringLength(XRoadServiceAttributeConsts.MaxNameLength, MinimumLength = XRoadServiceAttributeConsts.MinNameLength)]
        public virtual string Name { get; set; }

        [StringLength(XRoadServiceAttributeConsts.MaxDescriptionLength, MinimumLength = XRoadServiceAttributeConsts.MinDescriptionLength)]
        public virtual string Description { get; set; }

        [StringLength(XRoadServiceAttributeConsts.MaxFormatTransitionLength, MinimumLength = XRoadServiceAttributeConsts.MinFormatTransitionLength)]
        public virtual string FormatTransition { get; set; }

        public virtual int? XRoadServiceID { get; set; }

        [ForeignKey("XRoadServiceID")]
        public XRoadService XRoadServiceFk { get; set; }

    }
}