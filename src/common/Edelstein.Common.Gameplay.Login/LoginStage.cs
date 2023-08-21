using Edelstein.Protocol.Gameplay.Login;

namespace Edelstein.Common.Gameplay.Login;

public class LoginStage : AbstractStage<ILoginStageUser>, ILoginStage
{
    public override string ID { get; }
    
    public LoginStage(string id) => ID = id;
}
