using GraphQL.Types;
using SentenceStock.Domain;

namespace SentenceStock.Api.GraphQl
{
    public sealed class SentenceType : ObjectGraphType<Sentence>
    {
        public SentenceType()
        {
            Field(sentence => sentence.Id);
            Field(sentence => sentence.Text);
            Field(sentence => sentence.Language);
            Field(sentence => sentence.Source, nullable: true);
            Field(sentence => sentence.Notes, nullable: true);
            Field(sentence => sentence.IsFavorite);
            Field(sentence => sentence.CreatedAt);
            Field(sentence => sentence.UpdatedAt);
            Field<ListGraphType<StringGraphType>>("tags")
                .Resolve(context => context.Source.Tags);
        }
    }
}
