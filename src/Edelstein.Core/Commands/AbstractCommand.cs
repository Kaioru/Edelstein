using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;
using MoreLinq;

namespace Edelstein.Core.Commands
{
    public abstract class AbstractCommand<T> : ICommand
    {
        public abstract string Name { get; }
        public abstract string Description { get; }

        private readonly Parser _parser;
        public ICollection<string> Aliases { get; }
        public ICollection<ICommand> Commands { get; }

        public AbstractCommand(Parser parser)
        {
            _parser = parser;
            Aliases = new List<string>();
            Commands = new List<ICommand>();
        }

        public Task Process(ICommandSender sender, string text)
        {
            var regex = new Regex("([\"'])(?:(?=(\\\\?))\\2.)*?\\1|([^\\s]+)");
            var args = regex.Matches(text)
                .OfType<Match>()
                .Select(m =>
                {
                    var res = m.Value;

                    if (res.StartsWith("'") && res.EndsWith("'") ||
                        res.StartsWith("\"") && res.EndsWith("\""))
                    {
                        res = res.Substring(1);
                        res = res.Remove(res.Length - 1);
                    }

                    return res;
                })
                .ToList();

            return Process(sender, new Queue<string>(args));
        }

        public Task Process(ICommandSender sender, IEnumerable<string> args)
        {
            var result = _parser.ParseArguments<T>(args);

            return Task.Run(() => result
                .WithParsed(o => Execute(sender, o))
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
                    }
                ));
        }

        protected abstract Task Execute(ICommandSender sender, T option);
    }
}