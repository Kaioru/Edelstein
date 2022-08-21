using Edelstein.Protocol.Gameplay.Stages.Contexts;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC.Contracts.Pipelines;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts.Pipelines;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Contexts;

public interface IGameContextPipelines : IStageContextPipelines<IGameStageUser>
{
    IPipeline<IUserTransferChannelRequest> UserTransferChannelRequest { get; }
    IPipeline<IUserMove> UserMove { get; }
    IPipeline<IUserChat> UserChat { get; }
    IPipeline<IUserEmotion> UserEmotion { get; }
    IPipeline<IUserSelectNPC> UserSelectNPC { get; }
    IPipeline<IUserGatherItemRequest> UserGatherItemRequest { get; }
    IPipeline<IUserSortItemRequest> UserSortItemRequest { get; }
    IPipeline<IUserChangeSlotPositionRequest> UserChangeSlotPositionRequest { get; }
    IPipeline<IUserCharacterInfoRequest> UserCharacterInfoRequest { get; }

    IPipeline<INPCMove> NPCMove { get; }
}
