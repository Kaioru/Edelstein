using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using Edelstein.Service.Game.Fields.Continent;
using Edelstein.Service.Game.Fields.User;

namespace Edelstein.Service.Game.Commands.Handling
{
    public class ContiMoveCommand : AbstractGameCommand<ContiMoveCommandOption>
    {
        public override string Name => "contimove";
        public override string Description => "Gets the continent movement schedule";

        public ContiMoveCommand(Parser parser) : base(parser)
        {
            Aliases.Add("continent");
            Aliases.Add("schedule");
        }

        protected override async Task ExecuteAfter(FieldUser user, ContiMoveCommandOption option)
        {
            var continents = user.Socket.WvsGame.ContinentManager.Continents;
            var templateID = await user.Prompt<int>((self, target) => self.AskMenu(
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
                    c => $"{c.Template.Info} ({c.State})"
                )
            ), 0);
            var fieldManager = user.Socket.WvsGame.FieldManager;
            var field = fieldManager.Get(templateID);

            await field.Enter(user);
        }
    }

    public class ContiMoveCommandOption
    {
    }
}