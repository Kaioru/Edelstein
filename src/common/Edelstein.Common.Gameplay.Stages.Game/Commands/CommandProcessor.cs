using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game.Commands;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;

namespace Edelstein.Common.Gameplay.Stages.Game.Commands
{
    public class CommandProcessor : ICommandProcessor
    {
        public ICollection<ICommand> Commands => _commands.ToImmutableList();
        private readonly ICollection<ICommand> _commands;

        public CommandProcessor()
            => _commands = new List<ICommand>();

        public void Register(ICommand command)
            => _commands.Add(command);

        public void Deregister(ICommand command)
            => _commands.Remove(command);

        private ICommand GetCommand(string name)
        {
            return _commands.FirstOrDefault(c =>
                c.Name.StartsWith(name, StringComparison.OrdinalIgnoreCase) ||
                c.Aliases.Any(s => s.StartsWith(name, StringComparison.OrdinalIgnoreCase)));
        }

        public virtual Task<bool> Process(IFieldObjUser user, string text)
        {
            var regex = new Regex("([\"'])(?:(?=(\\\\?))\\2.)*?\\1|([^\\s]+)");
            var args = regex.Matches(text)
                .Select(m =>
                {
                    var res = m.Value;

                    if ((!res.StartsWith("'") || !res.EndsWith("'")) &&
                        (!res.StartsWith("\"") || !res.EndsWith("\"")))
                        return res;

                    res = res[1..];
                    res = res.Remove(res.Length - 1);
                    return res;
                })
                .ToArray();
            return Process(user, args);
        }

        public virtual Task<bool> Process(IFieldObjUser user, string[] args)
        {
            if (args.Length > 0)
            {
                var first = args[0];
                var command = GetCommand(first);

                if (command != null) return command.Process(user, args[1..]);
            }

            return Task.FromResult(false);
        }
    }
}
