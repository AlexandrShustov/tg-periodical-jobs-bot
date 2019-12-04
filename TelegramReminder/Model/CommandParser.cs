using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TelegramReminder.Model
{
    public class ParseResult
    {
        public string Mention { get; set; }
        public string Command { get; set; }

        public IReadOnlyDictionary<string, string> Arguments { get; set; }

        public bool IsValid => Mention != null && Command != null; //Arguments can be null, because command can be without arguments

        public override string ToString()
        {
            var template = "Mention: {0}\n" +
                           "Command: {1}\n" +
                           "Args: {2}\n";

            var mention = Mention ?? "none";
            var command = Command ?? "none";
            var args = string
                .Join("-", Arguments?
                .Select(a => $"k:{a.Key} v:{a.Value}") ?? new[] { "none" });

            var res = string.Format(template, mention, command, args);

            return res;
        }

        public string ArgumentOrEmpty (string key)
        {
            if (Arguments == null || !Arguments.Any())
                return string.Empty;

            var contains = Arguments.TryGetValue(key, out var result);

            if (contains is false)
                return string.Empty;

            return result;
        }
    }

    public class CommandParser
    {
        public const string MentionRegex = @"^[@](?:[^\s]*)";
        public const string CommandRegex = @"[\/](?:[^\s]*)";
        public const string ArgumentsRegex = @"\w+(?::)\"".*?\""";

        public static ParseResult Parse(string text)
        {
            return new ParseResult
            {
                Mention = MentionIn(text),
                Command = CommandIn(text),
                Arguments = ArgumentsIn(text)
            };
        }

        private static IReadOnlyDictionary<string, string> ArgumentsIn(string text)
        {
            var matches = Regex.Matches(text, ArgumentsRegex).ToList();
            
            if(matches.Any())
            {
                var valueByArgument = new Dictionary<string, string>();

                //match should be "arg:"value"
                foreach (var match in matches)
                {
                    var parts = match.Value.Split(':');

                    if (parts.Count() < 2)
                        return null;

                    var arg = parts[0];
                    var value = parts[1].Replace("\"", string.Empty);

                    valueByArgument[arg] = value;
                }

                return valueByArgument;
            }

            return null;
        }

        private static string MentionIn(string text)
        {
            var matches = Regex.Matches(text, MentionRegex);

            if (matches.Any())
                return matches.First().Value.Replace("@", "");

            return null;
        }

        private static string CommandIn(string text)
        {
            var matches = Regex.Matches(text, CommandRegex);

            if (matches.Any())
                return matches.First().Value.Replace("/", "");

            return null;
        }
    }
}
