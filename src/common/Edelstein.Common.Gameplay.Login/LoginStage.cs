using Edelstein.Protocol.Gameplay.Login;

namespace Edelstein.Common.Gameplay.Login;

public class LoginStage : AbstractStage<ILoginStageUser>, ILoginStage
{
    public LoginStage(string id) => ID = id;
    
    public override string ID { get; }
}
