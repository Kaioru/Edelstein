using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;
using Edelstein.Service.Game.Fields.Objects.User;
using MoreLinq.Extensions;

namespace Edelstein.Service.Game.Commands
{
    public abstract class AbstractCommand<T> : ICommand
        where T : DefaultCommandContext
    {
        private readonly Parser _parser;

        public abstract string Name { get; }
        public abstract string Description { get; }

        public ICollection<string> Aliases { get; }
        public ICollection<ICommand> Commands { get; }

        protected AbstractCommand(Parser parser)
        {
            _parser = parser;
            Aliases = new List<string>();
            Commands = new List<ICommand>();
        }

        public ICommand GetCommand(string name)
        {
            return Commands.FirstOrDefault(c =>
                c.Name.StartsWith(name, StringComparison.OrdinalIgnoreCase) ||
                c.Aliases.Any(s => s.StartsWith(name, StringComparison.OrdinalIgnoreCase)));
        }

        public Task Process(FieldUser sender, string text)
        {
            var regex = new Regex("([\"'])(?:(?=(\\\\?))\\2.)*?\\1|([^\\s]+)");
            var args = regex.Matches(text)
                .Select(m =>
                {
                    var res = m.Value;

                    if ((!res.StartsWith("'") || !res.EndsWith("'")) &&
                        (!res.StartsWith("\"") || !res.EndsWith("\"")))
                        return res;

                    res = res.Substring(1);
                    res = res.Remove(res.Length - 1);
                    return res;
                })
                .ToList();
            return Process(sender, new Queue<string>(args));
        }

        public Task Process(FieldUser sender, Queue<string> args)
        {
            if (args.Count > 0)
            {
                var first = args.Peek();
                var command = GetCommand(first);

                if (command != null)
                {
                    args.Dequeue();
                    return command.Process(sender, args);
                }
            }

            var result = _parser.ParseArguments<T>(args);

            return Task.Run(() => result
                .WithParsed(o => Run(sender, o))
                .WithNotParsed(errs =>
                {
                    var helpText = HelpText.AutoBuild(result, _parser.Settings.MaximumDisplayWidth);

                    helpText.Heading = string.Empty;
                    helpText.Copyright = string.Empty;

                    helpText.AddPostOptionsLines(
                        Commands.Select(c => $"{c.Name}\t\t\t\t{c.Description}")
                    );

                    Regex
                        .Split(helpText.ToString(), "\r\n|\r|\n")
                        .ForEach((s, i) => sender.Message(s));
                })
            );
        }

        protected abstract Task Run(FieldUser sender, T ctx);
    }
}