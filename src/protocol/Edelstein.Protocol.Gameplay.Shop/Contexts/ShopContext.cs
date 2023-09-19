namespace Edelstein.Protocol.Gameplay.Shop.Contexts;

public record ShopContext(
    IShopStageOptions Options,
    ShopContextManagers Managers,
    ShopContextServices Services,
    ShopContextRepositories Repositories,
    ShopContextTemplates Templates,
    ShopContextPipelines Pipelines
);
