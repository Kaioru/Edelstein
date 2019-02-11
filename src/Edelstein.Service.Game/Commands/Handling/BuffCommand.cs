using System;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using Edelstein.Service.Game.Fields.User;
using Edelstein.Service.Game.Fields.User.Stats;

namespace Edelstein.Service.Game.Commands.Handling
{
    public class BuffCommand : AbstractGameCommand<BuffCommandOptions>
    {
        public override string Name => "Buff";
        public override string Description => "Gives a cool buff.";

        public BuffCommand(Parser parser) : base(parser)
        {
            Aliases.Add("temporarystat");
            Aliases.Add("ts");
        }

        protected override async Task ExecuteAfter(FieldUser user, BuffCommandOptions option)
        {
            var type = await user.Prompt<int>((self, target) => self.AskMenu(
                "Which temporary stat?",
                Enum.GetValues(typeof(TemporaryStatType))
                    .Cast<TemporaryStatType>()
                    .ToDictionary(
                        t => (int) t,
                        t => t.ToString()
                    )
            ));
            var level = await user.Prompt<int>((self, target) => self.AskNumber(
                "What level?",
                1,
                1,
                short.MaxValue
            ));
            var duration = await user.Prompt<int>((self, target) => self.AskNumber(
                "How long? (in seconds)",
                0,
                0
            ));

            await user.ModifyTemporaryStat(s =>
                s.Set(
                    (TemporaryStatType) type,
                    30001000,
                    (short) level,
                    duration > 0
                        ? DateTime.Now.AddSeconds(duration)
                        : (DateTime?) null)
            );
        }
    }

    public class BuffCommandOptions
    {
    }
}