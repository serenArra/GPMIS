using System.Collections.Generic;

namespace MFAE.Jobs.DynamicEntityPropertyValues.Dto
{
    public class InsertOrUpdateAllValuesInput
    {
        public List<InsertOrUpdateAllValuesInputItem> Items { get; set; }
    }

    public class InsertOrUpdateAllValuesInputItem
    {
        public string EntityId { get; set; }

        public int DynamicEntityPropertyId { get; set; }

        public List<string> Values { get; set; }
    }
}
