using SentenceStock.Domain;

namespace SentenceStock.Domain.Tests;

public class InMemorySentenceRepositoryTests
{
    [Fact]
    public void Search_FindsSentenceByTag()
    {
        var repository = new InMemorySentenceRepository();

        var results = repository.Search(keyword: string.Empty, tag: "planning", favoritesOnly: false);

        Assert.Contains(results, sentence => sentence.Tags.Contains("planning"));
    }

    [Fact]
    public void ToggleFavorite_MarksSentenceAsFavorite()
    {
        var repository = new InMemorySentenceRepository();
        var sentence = repository.Add(new SentenceInput { Text = "A focused test is a useful test." });

        var updated = repository.ToggleFavorite(sentence.Id);

        Assert.True(updated.IsFavorite);
    }
}
