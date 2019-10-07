using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using Edelstein.Provider.Templates.String;
using Edelstein.Service.Game.Fields.Objects;
using Edelstein.Service.Game.Fields.Objects.Mob;
using Edelstein.Service.Game.Fields.Objects.NPC;
using Edelstein.Service.Game.Fields.Objects.Reactor;
using Edelstein.Service.Game.Fields.Objects.User;
using MoreLinq.Extensions;

namespace Edelstein.Service.Game.Commands.Handling
{
    public class DebugCommand : AbstractCommand<object>
    {
        public override string Name => "Debug";
        public override string Description => "Debugs various information";

        public DebugCommand(Parser parser) : base(parser)
        {
            Commands.Add(new DebugFieldCommand(parser));
            Commands.Add(new DebugQuestCommand(parser, false));
            Commands.Add(new DebugQuestCommand(parser, true));
        }

        protected override Task Execute(FieldUser sender, object option)
        {
            return Process(sender, new Queue<string>(new[] {"--help"}));
        }
    }

    public class DebugFieldCommand : AbstractCommand<object>
    {
        public override string Name => "Field";
        public override string Description => "Debugs various field information";

        public DebugFieldCommand(Parser parser) : base(parser)
        {
            Commands.Add(new CommandBuilder(parser)
                .WithName("npc")
                .WithDescription("Debugs NPCs in the field")
                .Build(sender => sender.Field
                    .GetObjects<FieldNPC>()
                    .ForEach(npc =>
                        sender.Message(
                            $"{npc.ID} - " +
                            $"{npc.Template.ID} " +
                            $"at {npc.Position} (FH {npc.Foothold})"))));
            Commands.Add(new CommandBuilder(parser)
                .WithName("mob")
                .WithDescription("Debugs mobs in the field")
                .Build(sender => sender.Field
                    .GetObjects<FieldMob>()
                    .ForEach(mob =>
                        sender.Message(
                            $"{mob.ID} - " +
                            $"{mob.Template.ID} " +
                            $"Lv. {mob.Template.Level} " +
                            $"{mob.HP}/{mob.Template.MaxHP} HP " +
                            $"at {mob.Position} (FH {mob.Foothold})"))));
            Commands.Add(new CommandBuilder(parser)
                .WithName("reactors")
                .WithDescription("Debugs reactors in the field")
                .Build(sender => sender.Field
                    .GetObjects<FieldReactor>()
                    .ForEach(reactor =>
                        sender.Message(
                            $"{reactor.ID} - " +
                            $"{reactor.Template.ID} " +
                            $"at {reactor.Position}"))));
            Commands.Add(new CommandBuilder(parser)
                .WithName("portals")
                .WithDescription("Debugs portals in the field")
                .Build(sender => sender.Field.Template.Portals
                    .ForEach(portal => sender.Message(
                        $"{portal.Key} - " +
                        $"{portal.Value.Name} " +
                        $"at {portal.Value.Position}"))));
        }

        protected override async Task Execute(FieldUser sender, object option)
        {
            var field = sender.Field;
            var stringTemplate = sender.Service.TemplateManager.Get<FieldStringTemplate>(field.ID);

            await sender.Message($"{stringTemplate.StreetName}: {stringTemplate.MapName} ({field.ID})");
            await sender.Message(
                $"Object count: {field.GetObjects().Count()} (Controlled: {field.GetControlledObjects(sender).Count()} / {field.GetObjects<IFieldControlledObj>().Count()})");
        }
    }

    public class DebugQuestCommand : AbstractCommand<object>
    {
        public override string Name { get; }
        public override string Description => "Debugs various quest information";

        private readonly bool _ex;

        public DebugQuestCommand(Parser parser, bool ex) : base(parser)
        {
            Name = ex ? "QuestEX" : "Quest";
            _ex = ex;
        }

        protected override async Task Execute(FieldUser sender, object option)
        {
            var records = _ex ? sender.Character.QuestRecordEx : sender.Character.QuestRecord;

            if (!records.Any())
            {
                await sender.Prompt(target => target.Say("You don't have any quest records."));
                return;
            }

            var templateID = await sender.Prompt<int>(target => target.AskMenu(
                $"Here are your quest records",
                records.ToDictionary(
                    r => (int) r.Key,
                    r => $"{r.Key}: {r.Value}"
                )
            ));
            var record = await sender.Prompt<string>(target => target.AskText(
                "What record would you like to set?",
                records[(short) templateID]
            ));

            await sender.ModifyQuests(q =>
            {
                if (_ex) q.UpdateEx((short) templateID, record);
                else q.Update((short) templateID, record);
            });
        }
    }
}