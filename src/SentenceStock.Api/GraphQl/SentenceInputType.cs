using GraphQL.Types;

namespace SentenceStock.Api.GraphQl
{
    public sealed class SentenceInputType : InputObjectGraphType
    {
        public SentenceInputType()
        {
            Name = "SentenceInput";

            Field<NonNullGraphType<StringGraphType>>("text");
            Field<StringGraphType>("language");
            Field<StringGraphType>("source");
            Field<StringGraphType>("notes");
            Field<ListGraphType<StringGraphType>>("tags");
        }
    }
}
