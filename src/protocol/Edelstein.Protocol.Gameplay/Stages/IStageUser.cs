using Edelstein.Protocol.Network.Session;

namespace Edelstein.Protocol.Gameplay.Stages
{
    public interface IStageUser<TStage, TUser> : ISession
        where TStage : IStage<TStage, TUser>
        where TUser : IStageUser<TStage, TUser>
    {
        AccountEntity Account { get; init; }
        AccountWorldEntity AccountWorld { get; init; }
        CharacterEntity CharacterEntity { get; init; }

        TStage Stage { get; set; }
    }
}
