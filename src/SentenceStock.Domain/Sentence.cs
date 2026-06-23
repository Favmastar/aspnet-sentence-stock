using System;
using System.Collections.Generic;

namespace SentenceStock.Domain
{
    public sealed class Sentence
    {
        public int Id { get; set; }

        public string Text { get; set; } = string.Empty;

        public string Language { get; set; } = string.Empty;

        public string Source { get; set; } = string.Empty;

        public string Notes { get; set; } = string.Empty;

        public List<string> Tags { get; set; } = new List<string>();

        public bool IsFavorite { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
