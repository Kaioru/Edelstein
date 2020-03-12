using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using Edelstein.Service.Game.Fields.Continent;
using Edelstein.Service.Game.Fields.Objects.User;
using MoreLinq.Extensions;

namespace Edelstein.Service.Game.Commands.Impl
{
    public class ContinentCommand : AbstractCommand<DefaultCommandContext>
    {
        public override string Name => "Continent";
        public override string Description => "Shows the continent movement schedule";

        public ContinentCommand(Parser parser) : base(parser)
        {
            Aliases.Add("ContiMove");
            Aliases.Add("Schedule");
        }

        protected override async Task Run(FieldUser sender, DefaultCommandContext ctx)
        {
            var continents = sender.Service.ContinentManager.All();
            var templateID = await sender.Prompt<int>(target => target.AskMenu(
                "Here are the ship schedules",
                continents.ToDictionary(
                    c =>
                    {
                        switch (c.State)
                        {
                            case ContinentState.Wait:
                                return c.Template.WaitFieldID;
                            case ContinentState.Move:
                                return c.Template.MoveFieldID;
                            default:
                                return c.Template.StartShipMoveFieldID;
                        }
                    },
                    c =>
                    {
                        var ret = $"{c.Template.Info} ({c.State})";

                        if (c.NextEvent.HasValue)
                            ret += $" #r(Event at {c.NextEvent.Value:H:mm tt})#b";
                        if (c.EventDoing)
                            ret += " #r(Event ongoing)#b";

                        return ret;
                    })
            ));
            var fieldManager = sender.Service.FieldManager;
            var field = fieldManager.Get(templateID);

            await field.Enter(sender);
        }
    }
}