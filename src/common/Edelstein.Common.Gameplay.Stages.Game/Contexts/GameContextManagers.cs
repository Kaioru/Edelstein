using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Contexts;
using Edelstein.Protocol.Util.Commands;

namespace Edelstein.Common.Gameplay.Stages.Game.Contexts;

public record GameContextManagers(
    ICommandManager<IFieldUser> Command
) : IGameContextManagers;
