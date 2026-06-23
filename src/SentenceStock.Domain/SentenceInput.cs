using System.Collections.Generic;

namespace SentenceStock.Domain
{
    public sealed class SentenceInput
    {
        public string Text { get; set; } = string.Empty;

        public string Language { get; set; } = string.Empty;

        public string Source { get; set; } = string.Empty;

        public string Notes { get; set; } = string.Empty;

        public List<string> Tags { get; set; } = new List<string>();
    }
}
