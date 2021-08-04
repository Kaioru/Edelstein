using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Network.Transport;

namespace Edelstein.Common.Gameplay.Stages.Login
{
    public class LoginStageUser : AbstractServerStageUser<LoginStage, LoginStageUser, LoginStageConfig>, ILoginStageUser<LoginStage, LoginStageUser>
    {
        public override int ID => Account.ID;

        public LoginStageUser(ISocket socket, IPacketProcessor<LoginStage, LoginStageUser> processor) : base(socket, processor)
        {
            IsLoggingIn = true;
        }
    }
}
