using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using Edelstein.Service.Game.Fields.Continents;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Commands.Handling
{
    public class ContinentCommand : AbstractCommand<object>
    {
        public override string Name => "Continent";
        public override string Description => "Shows the continent movement schedule";

        public ContinentCommand(Parser parser) : base(parser)
        {
            Aliases.Add("ContiMove");
            Aliases.Add("Schedule");
        }

        protected override async Task Execute(FieldUser sender, object option)
        {
            var continents = sender.Service.ContinentManager.Continents;
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