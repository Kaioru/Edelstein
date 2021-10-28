using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Network.Transport;
using Edelstein.Protocol.Services.Contracts.Social;

namespace Edelstein.Common.Gameplay.Stages.Game
{
    public class GameStageUser : AbstractServerStageUser<GameStage, GameStageUser, GameStageConfig>, IGameStageUser<GameStage, GameStageUser>
    {
        public IField Field => FieldUser?.Field;
        public IFieldObjUser FieldUser { get; set; }

        public GameStageUser(ISocket socket, IPacketProcessor<GameStage, GameStageUser> processor) : base(socket, processor) { }

        public override async Task OnDisconnect()
        {
            if (Character != null)
            {
                if (!IsMigrating)
                {
                    _ = Stage.InviteService.DeregisterAll(new InviteDeregisterAllRequest { Invited = ID });

                    if (FieldUser?.Party != null)
                    {
                        _ = Stage.PartyService.UpdateUserMigration(new PartyUpdateUserMigrationRequest
                        {
                            Character = ID,
                            Channel = -2,
                            Field = -1
                        });

                        if (FieldUser?.Party?.Boss == ID)
                        {
                            var nextLeader = FieldUser.Party.Members
                                .Where(m => m.ID != FieldUser.ID)
                                .Where(m => m.Channel >= 0)
                                .OrderByDescending(m => m.Level)
                                .FirstOrDefault();

                            if (nextLeader != null)
                                _ = Stage.PartyService.ChangeBoss(new PartyChangeBossRequest {
                                    Character = nextLeader.ID,
                                    IsDisconnect = true
                                });
                        }
                    }
                }

                if (Field != null && FieldUser != null)
                {
                    Character.FieldID = Field.Info.ForcedReturn ?? Field.ID;
                    Character.FieldPortal = (byte)(Field.Info.ForcedReturn.HasValue
                        ? 0
                        : Field.GetStartPointClosestTo(FieldUser.Position).ID
                    );

                    await FieldUser.EndDialog();
                    await FieldUser.EndConversation();
                }
            }

            await base.OnDisconnect();
        }
    }
}
