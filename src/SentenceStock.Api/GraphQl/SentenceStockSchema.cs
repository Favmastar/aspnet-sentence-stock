using System;
using GraphQL.Types;

namespace SentenceStock.Api.GraphQl
{
    public sealed class SentenceStockSchema : Schema
    {
        public SentenceStockSchema(IServiceProvider provider)
            : base(provider)
        {
            Query = (SentenceQuery)provider.GetService(typeof(SentenceQuery));
            Mutation = (SentenceMutation)provider.GetService(typeof(SentenceMutation));
        }
    }
}
