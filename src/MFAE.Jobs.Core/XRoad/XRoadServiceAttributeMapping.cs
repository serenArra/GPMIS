using MFAE.Jobs.XRoad;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace MFAE.Jobs.XRoad
{
    [Table("XRoadServiceAttributeMappings")]
    [Audited]
    public class XRoadServiceAttributeMapping : FullAuditedEntity
    {

        public virtual XRoadAttributeTypeEnum ServiceAttributeType { get; set; }

        public virtual string SourceValue { get; set; }

        public virtual string DestinationValue { get; set; }

        public virtual int? AttributeID { get; set; }

        [ForeignKey("AttributeID")]
        public XRoadServiceAttribute AttributeFk { get; set; }

    }
}