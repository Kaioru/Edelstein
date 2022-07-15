using System.Collections.Immutable;
using Edelstein.Protocol.Gameplay.Stages.Login;

namespace Edelstein.Common.Gameplay.Stages.Login;

public class LoginStage : AbstractStage<ILoginStage, ILoginStageUser>, ILoginStage
{
    public override string ID => "Login-0";
}
