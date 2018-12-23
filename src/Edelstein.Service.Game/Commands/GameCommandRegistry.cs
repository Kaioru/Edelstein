using System;
using System.Linq;
using System.Reflection;
using CommandLine;
using CSharpx;
using Edelstein.Core.Commands;

namespace Edelstein.Service.Game.Commands
{
    public class GameCommandRegistry : CommandRegistry
    {
        public GameCommandRegistry(Parser parser) : base(parser)
        {
            Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.Namespace.Equals($"{GetType().Namespace}.Handling"))
                .Where(t => t.GetInterfaces().Contains(typeof(ICommand)))
                .Where(t => t.GetConstructor(Type.EmptyTypes) != null)
                .Select(Activator.CreateInstance)
                .Cast<ICommand>()
                .ForEach(Commands.Add);
        }
    }
}