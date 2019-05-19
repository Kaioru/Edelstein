using System.Threading.Tasks;
using CommandLine;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Commands.Handling
{
    public class CashCommand : AbstractCommand<CashCommandOption>
    {
        public override string Name => "Cash";
        public override string Description => "Sets your account's cash to the specified value.";

        public CashCommand(Parser parser) : base(parser)
        {
        }

        protected override async Task Execute(FieldUser sender, CashCommandOption option)
        {
            var account = sender.Socket.Account;
            var cash = option.Type switch {
                0x2 => "Maple Point",
                0x4 => "Prepaid NX Cash",
                _ => "Nexon Cash"
                };

            account.IncCash(option.Type, option.Value - account.GetCash(option.Type));
            await sender.Message($"Successfully set {cash} to {option.Value}");
        }
    }

    public class CashCommandOption
    {
        [Value(0, MetaName = "value", HelpText = "The cash value.", Required = true)]
        public int Value { get; set; }

        [Option('t', "type", HelpText = "The cash type.", Default = 1)]
        public int Type { get; set; }
    }
}