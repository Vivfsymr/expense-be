using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ExpenseBe.Core.Models;

namespace ExpenseBe.Core.Services
{
    public static class WordBodyParser
    {
        private static readonly Regex SenseLineRegex = new(
            @"^\((noun|verb|adjective|adverb)\)\s*(.+)$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex FirstEnglishRegex = new(
            @"[A-Za-z]+(?:'[A-Za-z]+)?",
            RegexOptions.Compiled);

        public static WordQuizItem? TryParseQuizItem(Word word)
        {
            if (word == null || string.IsNullOrWhiteSpace(word.body) || string.IsNullOrWhiteSpace(word._id))
                return null;

            var text = NormalizeNewlines(word.body);
            var lines = text
                .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if (lines.Length == 0)
                return null;

            var englishMatch = FirstEnglishRegex.Match(lines[0]);
            if (!englishMatch.Success)
                return null;

            var english = englishMatch.Value.Trim().ToLowerInvariant();
            var hints = new List<string>();
            string? partOfSpeech = null;

            foreach (var line in lines.Skip(1))
            {
                // Skip related-words / grammar sections
                if (line.StartsWith("📌") || line.StartsWith("✏️") ||
                    line.StartsWith("Grammar", StringComparison.OrdinalIgnoreCase) ||
                    Regex.IsMatch(line, @"^[0-9]+[️⃣.]"))
                {
                    break;
                }

                var sense = SenseLineRegex.Match(line);
                if (!sense.Success)
                    continue;

                var pos = sense.Groups[1].Value.ToLowerInvariant();
                partOfSpeech ??= pos;
                var meaning = CleanVietnameseMeaning(sense.Groups[2].Value);
                if (string.IsNullOrWhiteSpace(meaning))
                    continue;

                // Keep (pos) for EVERY sense, not only the first
                hints.Add($"({pos}) {meaning}");

                if (hints.Count >= 3)
                    break;
            }

            if (hints.Count == 0)
                return null;

            return new WordQuizItem
            {
                id = word._id!,
                english = english,
                vietnameseHint = string.Join("; ", hints),
                partOfSpeech = partOfSpeech,
                acceptedAnswers = new List<string> { english }
            };
        }

        private static string NormalizeNewlines(string body)
        {
            return body
                .Replace("\\n", "\n", StringComparison.Ordinal)
                .Replace("\r\n", "\n", StringComparison.Ordinal)
                .Replace('\r', '\n');
        }

        private static string CleanVietnameseMeaning(string raw)
        {
            var meaning = raw.Trim();

            // Cut trailing notes like "✅ (nghĩa chính – ...)" or "📌 ..."
            var markers = new[] { "✅", "📌", "🔬", "⚖️", "✏️", "🧠" };
            foreach (var marker in markers)
            {
                var cutIdx = meaning.IndexOf(marker, StringComparison.Ordinal);
                if (cutIdx >= 0)
                {
                    meaning = meaning[..cutIdx];
                    break;
                }
            }

            meaning = Regex.Replace(meaning, @"\s*\([^)]*(nghĩa|note|main)[^)]*\)\s*$", "", RegexOptions.IgnoreCase);

            return Regex.Replace(meaning, @"\s+", " ").Trim().Trim(',', ';', '.', '·');
        }
    }
}
