using System;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using Edelstein.Core.Types;
using Edelstein.Service.Game.Fields.User;
using Edelstein.Service.Game.Fields.User.Stats;

namespace Edelstein.Service.Game.Commands.Handling
{
    public class BuffCommand : AbstractCommand<object>
    {
        public override string Name => "Buff";
        public override string Description => "Creates a buff with the specific values";

        public BuffCommand(Parser parser) : base(parser)
        {
            Aliases.Add("TemporaryStat");
            Aliases.Add("TS");
        }

        protected override async Task Execute(FieldUser sender, object option)
        {
            var type = await sender.Prompt<int>(target => target.AskMenu(
                "Which temporary stat type?",
                Enum.GetValues(typeof(TemporaryStatType))
                    .Cast<TemporaryStatType>()
                    .ToDictionary(
                        t => (int) t,
                        t => t.ToString()
                    )
            ));
            var options = await sender.Prompt<short>(target => (short) target.AskNumber(
                "What option?",
                1,
                1
            ));
            var templateID = await sender.Prompt<int>(target => target.AskNumber(
                "What template ID?",
                (int) Skill.CitizenCristalThrow
            ));

            await sender.ModifyTemporaryStats(s =>
                s.Set(
                    (TemporaryStatType) type,
                    templateID,
                    options
                ));
        }
    }
}