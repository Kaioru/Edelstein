using Edelstein.Protocol.Gameplay.Shop;

namespace Edelstein.Application.Server.Configs;

public record ProgramConfigStageShop : ProgramConfigStage, IShopStageOptions
{
    public int WorldID { get; set; }
}
