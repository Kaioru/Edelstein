using CommandLine;

namespace Edelstein.Service.Game.Commands
{
    public interface ICommandContext
    {
        [Option('v', "verbose", HelpText = "Verbose output.")]
        public bool? Verbose { get; set; }
    }
}