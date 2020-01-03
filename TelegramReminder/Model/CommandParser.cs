using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TelegramReminder.Model
{
    public class ParseResult
    {
        public string Tag { get; set; }
        public IReadOnlyDictionary<string, string> Arguments { get; set; }

        public override string ToString()
        {
            var template = "Command: {0}\n" +
                           "Args: {1}\n";

            var command = Tag ?? "none";
            var args = "\n-" + string
                .Join("-", Arguments?
                .Select(a => $" {a.Key} : {a.Value} \n") ?? new[] { "none" });

            var res = string.Format(template, command, args);
            return res;
        }
    }

    public class Input
    {
        public const string CommandRegex = @"[\/](?:[^\s@]*)";
        public const string ArgumentsRegex = @"\w+(?::)\"".*?\""";

        public static IEnumerable<string> Errors => _errors;

        private static List<string> _errors = new List<string>();

        public static ParseResult Parse(string text)
        {
            _errors.Clear();

            return new ParseResult
            {
                Tag = CommandIn(text),
                Arguments = ArgumentsIn(text)
            };
        }
        
        private static string CommandIn(string text)
        {
            var matches = Regex.Matches(text, CommandRegex);

            if (matches.Any())
                return matches.First().Value.Replace("/", "");

            _errors.Add("Couldn`t parse command");
            return null;
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
                    {
                        _errors.Add("Couldn`t parse arguments");
                        return null;
                    }

                    var arg = parts[0];
                    var value = parts[1].Replace("\"", string.Empty);

                    valueByArgument[arg] = value;
                }

                return valueByArgument;
            }

            return null;
        }
    }
}
