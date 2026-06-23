using System;
using System.Collections.Generic;
using System.Linq;

namespace SentenceStock.Domain
{
    public sealed class InMemorySentenceRepository : ISentenceRepository
    {
        private readonly List<Sentence> sentences = new List<Sentence>();
        private int nextId = 1;

        public InMemorySentenceRepository()
        {
            Add(new SentenceInput
            {
                Text = "The earlier we start, the more calmly we can finish.",
                Language = "English",
                Source = "Sample",
                Notes = "Useful for project planning.",
                Tags = new List<string> { "planning", "work" }
            });

            Add(new SentenceInput
            {
                Text = "小さく作って、早く試す。",
                Language = "Japanese",
                Source = "Sample",
                Notes = "A compact product-building reminder.",
                Tags = new List<string> { "learning", "product" }
            });
        }

        public IReadOnlyList<Sentence> Search(string keyword, string tag, bool favoritesOnly)
        {
            IEnumerable<Sentence> query = sentences;

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(sentence =>
                    Contains(sentence.Text, keyword) ||
                    Contains(sentence.Language, keyword) ||
                    Contains(sentence.Source, keyword) ||
                    Contains(sentence.Notes, keyword));
            }

            if (!string.IsNullOrWhiteSpace(tag))
            {
                query = query.Where(sentence =>
                    sentence.Tags.Any(value => Contains(value, tag)));
            }

            if (favoritesOnly)
            {
                query = query.Where(sentence => sentence.IsFavorite);
            }

            return query
                .OrderByDescending(sentence => sentence.IsFavorite)
                .ThenByDescending(sentence => sentence.UpdatedAt)
                .ToList();
        }

        public Sentence Add(SentenceInput input)
        {
            if (string.IsNullOrWhiteSpace(input.Text))
            {
                throw new ArgumentException("Sentence text is required.", nameof(input));
            }

            var now = DateTime.UtcNow;
            var sentence = new Sentence
            {
                Id = nextId++,
                Text = input.Text.Trim(),
                Language = Normalize(input.Language),
                Source = Normalize(input.Source),
                Notes = Normalize(input.Notes),
                Tags = input.Tags
                    .Where(tag => !string.IsNullOrWhiteSpace(tag))
                    .Select(tag => tag.Trim())
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList(),
                CreatedAt = now,
                UpdatedAt = now
            };

            sentences.Add(sentence);
            return sentence;
        }

        public Sentence ToggleFavorite(int id)
        {
            var sentence = sentences.FirstOrDefault(value => value.Id == id);
            if (sentence == null)
            {
                throw new KeyNotFoundException("Sentence was not found.");
            }

            sentence.IsFavorite = !sentence.IsFavorite;
            sentence.UpdatedAt = DateTime.UtcNow;
            return sentence;
        }

        public bool Delete(int id)
        {
            var sentence = sentences.FirstOrDefault(value => value.Id == id);
            if (sentence == null)
            {
                return false;
            }

            sentences.Remove(sentence);
            return true;
        }

        private static bool Contains(string value, string search)
        {
            return value != null &&
                value.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static string Normalize(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
        }
    }
}
