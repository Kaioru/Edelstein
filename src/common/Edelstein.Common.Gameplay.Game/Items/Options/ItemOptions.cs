using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates.Options;
using Edelstein.Protocol.Gameplay.Game.Items.Options;

namespace Edelstein.Common.Gameplay.Game.Items.Options;

public record ItemOptions(
    ItemOptionGrade Grade,
    int Option1,
    int Option2,
    int Option3
) : IItemOptions;
