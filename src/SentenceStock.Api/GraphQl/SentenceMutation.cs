using System.Collections.Generic;
using GraphQL;
using GraphQL.Types;
using SentenceStock.Domain;

namespace SentenceStock.Api.GraphQl
{
    public sealed class SentenceMutation : ObjectGraphType
    {
        public SentenceMutation(ISentenceRepository repository)
        {
            Field<SentenceType>("createSentence")
                .Argument<NonNullGraphType<SentenceInputType>>("input")
                .Resolve(context =>
                {
                    var input = context.GetArgument<SentenceInput>("input");
                    if (input.Tags == null)
                    {
                        input.Tags = new List<string>();
                    }

                    return repository.Add(input);
                });

            Field<SentenceType>("toggleFavorite")
                .Argument<NonNullGraphType<IntGraphType>>("id")
                .Resolve(context => repository.ToggleFavorite(context.GetArgument<int>("id")));

            Field<BooleanGraphType>("deleteSentence")
                .Argument<NonNullGraphType<IntGraphType>>("id")
                .Resolve(context => repository.Delete(context.GetArgument<int>("id")));
        }
    }
}
