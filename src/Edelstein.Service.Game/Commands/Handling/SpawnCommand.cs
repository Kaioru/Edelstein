using System.Threading.Tasks;
using CommandLine;
using Edelstein.Provider.Templates.Field.Life.Mob;
using Edelstein.Provider.Templates.String;
using Edelstein.Service.Game.Fields.Objects.Mob;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Commands.Handling
{
    public class SpawnCommand : AbstractTemplateCommand<MobTemplate, MobStringTemplate, MobCommandOption>
    {
        public override string Name => "Spawn";
        public override string Description => "Spawns one or more mobs in the current field";

        public SpawnCommand(Parser parser) : base(parser)
        {
        }

        protected override async Task ExecuteAfter(
            FieldUser sender,
            MobTemplate template,
            MobCommandOption option
        )
        {
            var mob = new FieldMob(template, false)
            {
                Position = sender.Position,
                Foothold = (short)sender.Foothold,
                HomeFoothold = (short)sender.Foothold
            };

            await sender.Field.Enter(mob, () => mob.GetEnterFieldPacket(MobAppearType.Regen));
            return;
        }
    }

    public class MobCommandOption : TemplateCommandOption
    {
        [Option('q', "quantity", HelpText = "The mob quantity to spawn.")]
        public short? Quantity { get; set; }
    }
}