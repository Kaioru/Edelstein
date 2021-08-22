using System.Collections.Generic;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Social
{
    public interface IPartyInfo
    {
        int ID { get; }
        int BossCharacterID { get; }

        ICollection<IPartyMemberInfo> Members { get; }
    }
}
