using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using Edelstein.Core.Types;
using Edelstein.Service.Game.Fields.User;
using Edelstein.Service.Game.Fields.User.Stats;

namespace Edelstein.Service.Game.Commands.Handling
{
    public class BuffCommand : AbstractCommand<BuffCommandOption>
    {
        public override string Name => "Buff";
        public override string Description => "Creates a buff with the specific values";

        public BuffCommand(Parser parser) : base(parser)
        {
            Aliases.Add("TemporaryStat");
            Aliases.Add("TS");
        }

        protected override async Task Execute(FieldUser sender, BuffCommandOption option)
        {
            var type = (TemporaryStatType) (option.List
                ? await sender.Prompt<int>(target => target.AskMenu(
                    "These are your current temporary stats",
                    sender.TemporaryStats
                        .ToDictionary(
                            kv => (int) kv.Key,
                            kv => $"{kv.Key}: {kv.Value.Option}"
                        )
                ))
                : await sender.Prompt<int>(target => target.AskMenu(
                    "Which temporary stat type?",
                    Enum.GetValues(typeof(TemporaryStatType))
                        .Cast<TemporaryStatType>()
                        .ToDictionary(
                            t => (int) t,
                            t => t.ToString()
                        )
                )));

            if (option.List &&
                await sender.Prompt<int>(target => target.AskMenu(
                    $"What would you like to do with {type}?",
                    new Dictionary<int, string>
                    {
                        [0] = "Modify the stat option",
                        [1] = "#rRemove the stat"
                    }
                )) > 0)
            {
                await sender.ModifyTemporaryStats(s => s.Reset(type));
                return;
            }

            var options = await sender.Prompt<int>(target => target.AskNumber(
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
                    type,
                    templateID,
                    options
                ));
        }
    }

    public class BuffCommandOption
    {
        [Option("list", HelpText = "List the current buffs.")]
        public bool List { get; set; }
    }
}