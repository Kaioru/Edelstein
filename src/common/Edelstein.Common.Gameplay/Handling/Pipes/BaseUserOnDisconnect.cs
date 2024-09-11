using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Entities;
using Edelstein.Protocol.Services.Session;
using Edelstein.Protocol.Services.Session.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Handling.Pipes;

public class BaseUserOnDisconnect<TStageSystem, TStageSystemUser>(
    IAccountRepository accounts,
    IAccountWorldDataRepository accountWorldData,
    ICharacterRepository characters,
    ISessionService sessions
) : IPipe<UserOnDisconnect<TStageSystem, TStageSystemUser>>
    where TStageSystem : IStageSystem<TStageSystem, TStageSystemUser>
    where TStageSystemUser : IStageSystemUser<TStageSystem, TStageSystemUser>
{
    public virtual async Task Handle(IPipelineContext ctx, UserOnDisconnect<TStageSystem, TStageSystemUser> message)
    {
        if (message.User.Character != null)
            await characters.Update(message.User.Character);
        
        if (message.User.AccountWorldData != null)
            await accountWorldData.Update(message.User.AccountWorldData);

        if (message.User.Account != null)
        {
            await accounts.Update(message.User.Account);
            
            if (!message.User.IsMigrating)
                await sessions.End(new SessionServiceEndRequest
                {
                    AccountID = message.User.Account.ID,
                    Secret = message.User.Key
                });
        }
    }
}
