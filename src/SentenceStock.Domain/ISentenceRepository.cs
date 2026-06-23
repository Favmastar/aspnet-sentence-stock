using System.Collections.Generic;

namespace SentenceStock.Domain
{
    public interface ISentenceRepository
    {
        IReadOnlyList<Sentence> Search(string keyword, string tag, bool favoritesOnly);

        Sentence Add(SentenceInput input);

        Sentence ToggleFavorite(int id);

        bool Delete(int id);
    }
}
