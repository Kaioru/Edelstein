using System;
using System.Collections.Generic;
using CommandLine;
using Edelstein.Service.Game.Commands.Util;
using Edelstein.Service.Game.Fields.Objects.User;
using MoreLinq.Extensions;

namespace Edelstein.Service.Game.Commands
{
    public class CommandBuilder
    {
        private Parser Parser { get; set; }
        private string Name { get; set; }
        private string Description { get; set; }
        private ICollection<string> Aliases { get; }
        private ICollection<ICommand> Commands { get; }

        public CommandBuilder(Parser parser)
        {
            Parser = parser;
            Aliases = new List<string>();
            Commands = new List<ICommand>();
        }

        public CommandBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

        public CommandBuilder WithDescription(string description)
        {
            Description = description;
            return this;
        }

        public CommandBuilder WithAlias(string alias)
        {
            Aliases.Add(alias);
            return this;
        }

        public CommandBuilder WithCommand(ICommand command)
        {
            Commands.Add(command);
            return this;
        }

        public ICommand Build(Action<FieldUser, DefaultCommandContext> action)
        {
            var command = new ActionCommand(Parser, Name, Description, action);

            Aliases.ForEach(a => command.Aliases.Add(a));
            Commands.ForEach(c => command.Commands.Add(c));
            return command;
        }
    }
}