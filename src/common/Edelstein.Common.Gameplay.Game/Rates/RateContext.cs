using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Game.Rates;

namespace Edelstein.Common.Gameplay.Game.Rates;

public sealed record RateContext(IFieldUser? User, IGameStageOptions? Options) : IRateContext;
