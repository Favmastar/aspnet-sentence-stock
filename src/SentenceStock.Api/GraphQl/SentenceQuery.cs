using GraphQL.Types;
using GraphQL;
using SentenceStock.Domain;

namespace SentenceStock.Api.GraphQl
{
    public sealed class SentenceQuery : ObjectGraphType
    {
        public SentenceQuery(ISentenceRepository repository)
        {
            Field<ListGraphType<SentenceType>>("sentences")
                .Argument<StringGraphType>("keyword")
                .Argument<StringGraphType>("tag")
                .Argument<BooleanGraphType>("favoritesOnly")
                .Resolve(context => repository.Search(
                    context.GetArgument<string>("keyword"),
                    context.GetArgument<string>("tag"),
                    context.GetArgument<bool>("favoritesOnly")));
        }
    }
}
