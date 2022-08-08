using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Contracts;

public record UserCharacterInfoRequest(
    IFieldUser User,
    int CharacterID
) : IUserCharacterInfoRequest;
