using Abp.Dependency;
using GraphQL.Types;
using GraphQL.Utilities;
using MFAE.Jobs.Queries.Container;
using System;

namespace MFAE.Jobs.Schemas
{
    public class MainSchema : Schema, ITransientDependency
    {
        public MainSchema(IServiceProvider provider) :
            base(provider)
        {
            Query = provider.GetRequiredService<QueryContainer>();
        }
    }
}