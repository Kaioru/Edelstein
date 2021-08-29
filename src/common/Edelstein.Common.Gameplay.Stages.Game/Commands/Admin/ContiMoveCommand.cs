using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game.Continent;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;

namespace Edelstein.Common.Gameplay.Stages.Game.Commands.Admin
{
    public class ContiMoveCommand : AbstractCommand
    {
        private readonly IContiMoveRepository _contiMoveRepository;

        public override string Name => "ContiMove";
        public override string Description => "Shows the contimove schedule";

        public ContiMoveCommand(IContiMoveRepository contiMoveReposistory)
        {
            _contiMoveRepository = contiMoveReposistory;
        }

        public override async Task Execute(IFieldObjUser user, string[] args)
        {
            var contiMoves = await _contiMoveRepository.RetrieveAll();
            var contiMoveID = await user.Prompt(target => target.AskMenu(
                "Here are the ship schedules",
                contiMoves.ToDictionary(
                    c => c.ID,
                    c =>
                    {
                        var ret = $"{c.Info.Name} ({c.State})";

                        if (c.State == ContiMoveState.Event)
                            ret += " #r(Event ongoing)#b";

                        return ret;
                    }
                )
            ));
            var contiMove = await _contiMoveRepository.Retrieve(contiMoveID);

            await contiMove.Enter(user);
        }
    }
}
